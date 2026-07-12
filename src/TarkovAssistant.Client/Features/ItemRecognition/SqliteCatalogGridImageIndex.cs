using Microsoft.Data.Sqlite;
using System.IO;
using TarkovAssistant.Client.Features.CatalogCache;

namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Reads grid-image references from the local catalog SQLite index.
/// </summary>
internal sealed class SqliteCatalogGridImageIndex : ICatalogGridImageIndex
{
    private readonly string _databasePath;

    /// <summary>
    /// Initializes a reader for the local synchronized catalog index.
    /// </summary>
    /// <param name="options">The catalog cache location containing <c>catalog.db</c>.</param>
    /// <exception cref="ArgumentException">The configured cache root is not an absolute path.</exception>
    public SqliteCatalogGridImageIndex(CatalogCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (!Path.IsPathFullyQualified(options.RootPath))
        {
            throw new ArgumentException("The catalog cache root must be an absolute path.", nameof(options));
        }

        _databasePath = Path.Combine(Path.GetFullPath(options.RootPath), "catalog.db");
    }

    /// <inheritdoc />
    public async ValueTask<IReadOnlyList<CatalogGridImage>> ReadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_databasePath))
        {
            return Array.Empty<CatalogGridImage>();
        }

        await using var connection = new SqliteConnection($"Data Source={_databasePath};Mode=ReadOnly;Pooling=False;Default Timeout=10");
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT id, grid_image_path, width, height FROM catalog_items WHERE grid_image_path IS NOT NULL AND grid_image_path <> '';";
        List<CatalogGridImage> images = [];
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            if (Guid.TryParse(reader.GetString(0), out var itemId)
                && reader.GetInt32(2) > 0
                && reader.GetInt32(3) > 0)
            {
                images.Add(new CatalogGridImage(itemId, reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
            }
        }

        return images;
    }
}
