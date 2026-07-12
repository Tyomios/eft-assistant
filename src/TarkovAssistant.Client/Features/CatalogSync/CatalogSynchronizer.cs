using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Data.Sqlite;

namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>
/// Downloads the backend catalog into a staging folder and changes the active pointer only after it is complete.
/// </summary>
internal sealed class CatalogSynchronizer : ICatalogSynchronizer, IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;
    private readonly long _maximumImageBytes;
    private readonly int _maximumConcurrentImageDownloads;
    private readonly string _rootPath;
    private readonly string _databasePath;

    /// <summary>
    /// Initializes a catalog synchronizer using the configured local backend and cache root.
    /// </summary>
    /// <param name="options">The backend address, storage boundary, and image-size safety limit.</param>
    /// <exception cref="ArgumentException">The configured cache root is not absolute or backend address is invalid.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The image-size safety limit is not positive.</exception>
    public CatalogSynchronizer(CatalogSyncOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (!Path.IsPathFullyQualified(options.RootPath) || !options.BaseAddress.IsAbsoluteUri)
        {
            throw new ArgumentException("The catalog cache root and backend address must be absolute.", nameof(options));
        }

        if (options.MaximumImageBytes <= 0 || options.MaximumConcurrentImageDownloads <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "The image size and download concurrency must be positive.");
        }

        _rootPath = Path.GetFullPath(options.RootPath);
        _databasePath = Path.Combine(_rootPath, "catalog.db");
        _maximumImageBytes = options.MaximumImageBytes;
        _maximumConcurrentImageDownloads = Math.Max(1, checked((int)Math.Floor(options.MaximumConcurrentImageDownloads * 0.8)));
        _httpClient = new HttpClient { BaseAddress = options.BaseAddress, Timeout = TimeSpan.FromMinutes(2) };
    }

    /// <inheritdoc />
    public async Task<CatalogSyncResult> SynchronizeAsync(CancellationToken cancellationToken)
    {
        CatalogVersionDto? remoteVersion;
        try
        {
            using var response = await _httpClient.GetAsync("api/catalog/version", cancellationToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new CatalogSyncResult(CatalogSyncState.CatalogUnavailable, null, "The backend has not published a catalog yet.");
            }

            response.EnsureSuccessStatusCode();
            remoteVersion = await response.Content.ReadFromJsonAsync<CatalogVersionDto>(JsonOptions, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            return new CatalogSyncResult(CatalogSyncState.Unavailable, null, "The local backend is unavailable.");
        }

        if (remoteVersion is null || remoteVersion.Version <= 0 || string.IsNullOrWhiteSpace(remoteVersion.ContentHash))
        {
            return new CatalogSyncResult(CatalogSyncState.Failed, null, "The backend returned an invalid catalog version.");
        }

        var localVersion = await ReadStoredVersionAsync(cancellationToken).ConfigureAwait(false);
        if (localVersion == remoteVersion.Version)
        {
            return new CatalogSyncResult(CatalogSyncState.UpToDate, remoteVersion.Version, null);
        }

        try
        {
            Directory.CreateDirectory(_rootPath);
            var items = await DownloadChangesAsync(localVersion ?? 0, cancellationToken).ConfigureAwait(false);
            var imageSources = await ReadImageSourcesAsync(cancellationToken).ConfigureAwait(false);
            await DownloadImagesAsync(items, imageSources, cancellationToken).ConfigureAwait(false);
            await UpsertIndexAsync(remoteVersion, items, cancellationToken).ConfigureAwait(false);
            return new CatalogSyncResult(CatalogSyncState.Updated, remoteVersion.Version, null);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException)
        {
            return new CatalogSyncResult(CatalogSyncState.Unavailable, remoteVersion.Version, "The catalog or one of its images could not be downloaded.");
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or SqliteException or JsonException or InvalidDataException)
        {
            return new CatalogSyncResult(CatalogSyncState.Failed, remoteVersion.Version, "The downloaded catalog could not be stored safely.");
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private async Task<IReadOnlyList<CatalogItemDto>> DownloadChangesAsync(long version, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync($"api/catalog/changes?sinceVersion={version}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var changes = await response.Content.ReadFromJsonAsync<CatalogChangesDto>(JsonOptions, cancellationToken).ConfigureAwait(false);
        return changes?.Items ?? throw new InvalidDataException("The backend returned an invalid catalog changes payload.");
    }

    private async Task UpsertIndexAsync(CatalogVersionDto version, IReadOnlyList<CatalogItemDto> items, CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandText = """
            PRAGMA journal_mode=WAL;
            PRAGMA busy_timeout=10000;
            CREATE TABLE IF NOT EXISTS catalog_metadata (id INTEGER PRIMARY KEY CHECK (id = 1), version INTEGER NOT NULL, published_at TEXT NOT NULL, content_hash TEXT NOT NULL);
            CREATE TABLE IF NOT EXISTS catalog_items (id TEXT PRIMARY KEY, external_id TEXT NOT NULL, name TEXT NOT NULL, normalized_name TEXT NOT NULL, short_name TEXT NOT NULL, width INTEGER NOT NULL, height INTEGER NOT NULL, icon_path TEXT NULL, grid_image_path TEXT NULL);
            CREATE INDEX IF NOT EXISTS ix_catalog_items_normalized_name ON catalog_items(normalized_name);
            CREATE TABLE IF NOT EXISTS catalog_images (item_id TEXT NOT NULL, kind TEXT NOT NULL, source_url TEXT NOT NULL, PRIMARY KEY (item_id, kind));
            """;
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO catalog_metadata (id, version, published_at, content_hash) VALUES (1, $version, $publishedAt, $contentHash) ON CONFLICT(id) DO UPDATE SET version = excluded.version, published_at = excluded.published_at, content_hash = excluded.content_hash;";
        command.Parameters.AddWithValue("$version", version.Version);
        command.Parameters.AddWithValue("$publishedAt", version.PublishedAt.UtcDateTime.ToString("O", System.Globalization.CultureInfo.InvariantCulture));
        command.Parameters.AddWithValue("$contentHash", version.ContentHash);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

        command.Parameters.Clear();
        command.CommandText = "INSERT INTO catalog_items (id, external_id, name, normalized_name, short_name, width, height, icon_path, grid_image_path) VALUES ($id, $externalId, $name, $normalizedName, $shortName, $width, $height, $iconPath, $gridImagePath) ON CONFLICT(id) DO UPDATE SET external_id = excluded.external_id, name = excluded.name, normalized_name = excluded.normalized_name, short_name = excluded.short_name, width = excluded.width, height = excluded.height, icon_path = excluded.icon_path, grid_image_path = excluded.grid_image_path;";
        foreach (var item in items)
        {
            ValidateItem(item);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("$id", item.Id.ToString("D"));
            command.Parameters.AddWithValue("$externalId", item.ExternalId);
            command.Parameters.AddWithValue("$name", item.Name);
            command.Parameters.AddWithValue("$normalizedName", item.NormalizedName);
            command.Parameters.AddWithValue("$shortName", item.ShortName);
            command.Parameters.AddWithValue("$width", item.Width);
            command.Parameters.AddWithValue("$height", item.Height);
            command.Parameters.AddWithValue("$iconPath", ImagePath(item.Id, "icon", item.IconUrl));
            command.Parameters.AddWithValue("$gridImagePath", ImagePath(item.Id, "grid", item.GridImageUrl));
            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            await UpsertImageSourceAsync(connection, transaction, item.Id, "icon", item.IconUrl, cancellationToken).ConfigureAwait(false);
            await UpsertImageSourceAsync(connection, transaction, item.Id, "grid", item.GridImageUrl, cancellationToken).ConfigureAwait(false);
        }

        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task DownloadImagesAsync(IReadOnlyList<CatalogItemDto> items, IReadOnlyDictionary<string, string> imageSources, CancellationToken cancellationToken)
    {
        await Parallel.ForEachAsync(
            items,
            new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = _maximumConcurrentImageDownloads
            },
            async (item, token) =>
            {
                await DownloadImageAsync(item.Id, "icon", item.IconUrl, imageSources, token).ConfigureAwait(false);
                await DownloadImageAsync(item.Id, "grid", item.GridImageUrl, imageSources, token).ConfigureAwait(false);
            }).ConfigureAwait(false);
    }

    private async Task DownloadImageAsync(Guid itemId, string kind, string? sourceUrl, IReadOnlyDictionary<string, string> imageSources, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl))
        {
            return;
        }

        if (!Uri.TryCreate(sourceUrl, UriKind.Absolute, out var uri) || uri.Scheme is not ("http" or "https"))
        {
            throw new InvalidDataException("The backend returned an invalid image URL.");
        }

        using var response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        if (response.Content.Headers.ContentLength is long length && length > _maximumImageBytes)
        {
            throw new InvalidDataException("A catalog image exceeds the configured size limit.");
        }

        var relativePath = ImagePath(itemId, kind, sourceUrl) ?? throw new InvalidDataException("The image path could not be determined.");
        var finalPath = Path.Combine(_rootPath, relativePath);
        var sourceKey = $"{itemId:D}:{kind}";
        if (imageSources.TryGetValue(sourceKey, out var storedUrl) && storedUrl == sourceUrl && File.Exists(finalPath))
        {
            return;
        }
        Directory.CreateDirectory(Path.GetDirectoryName(finalPath)!);
        var temporaryPath = finalPath + $".{Guid.NewGuid():N}.part";
        try
        {
            await using (var source = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
            await using (var destination = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, useAsync: true))
            {
                var total = 0L;
                var buffer = new byte[81920];
                int read;
                while ((read = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) > 0)
                {
                    total += read;
                    if (total > _maximumImageBytes)
                    {
                        throw new InvalidDataException("A catalog image exceeds the configured size limit.");
                    }

                    await destination.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(false);
                }

                await destination.FlushAsync(cancellationToken).ConfigureAwait(false);
            }

            File.Move(temporaryPath, finalPath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    private async Task<long?> ReadStoredVersionAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(_databasePath))
            {
                return null;
            }
            await using var connection = CreateConnection();
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT version FROM catalog_metadata WHERE id = 1;";
            var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            return result is long version && version > 0 ? version : null;
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }

    private SqliteConnection CreateConnection() => new($"Data Source={_databasePath};Mode=ReadWriteCreate;Pooling=False;Default Timeout=10");

    private async Task<IReadOnlyDictionary<string, string>> ReadImageSourcesAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_databasePath))
        {
            return new Dictionary<string, string>();
        }
        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        var command = connection.CreateCommand();
        command.CommandText = "SELECT item_id, kind, source_url FROM catalog_images;";
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add($"{reader.GetString(0)}:{reader.GetString(1)}", reader.GetString(2));
        }
        return result;
    }

    private static async Task UpsertImageSourceAsync(SqliteConnection connection, SqliteTransaction transaction, Guid itemId, string kind, string? sourceUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl))
        {
            return;
        }
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO catalog_images (item_id, kind, source_url) VALUES ($itemId, $kind, $sourceUrl) ON CONFLICT(item_id, kind) DO UPDATE SET source_url = excluded.source_url;";
        command.Parameters.AddWithValue("$itemId", itemId.ToString("D"));
        command.Parameters.AddWithValue("$kind", kind);
        command.Parameters.AddWithValue("$sourceUrl", sourceUrl);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static string? ImagePath(Guid itemId, string kind, string? sourceUrl)
    {
        return string.IsNullOrWhiteSpace(sourceUrl) ? null : Path.Combine("images", kind, $"{itemId:N}.bin");
    }

    private static void ValidateItem(CatalogItemDto item)
    {
        if (item.Id == Guid.Empty || string.IsNullOrWhiteSpace(item.ExternalId) || string.IsNullOrWhiteSpace(item.Name)
            || string.IsNullOrWhiteSpace(item.NormalizedName) || string.IsNullOrWhiteSpace(item.ShortName)
            || item.Width <= 0 || item.Height <= 0)
        {
            throw new InvalidDataException("The backend returned an invalid catalog item.");
        }
    }
}
