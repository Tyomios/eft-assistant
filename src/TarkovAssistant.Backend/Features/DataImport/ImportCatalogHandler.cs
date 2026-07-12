using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Backend.Database;
using TarkovAssistant.Backend.Database.Entities;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.DataImport;

internal sealed class ImportCatalogHandler
{
    private const string ImportName = "catalog";
    private const int PageSize = 500;
    private static readonly SemaphoreSlim ImportLock = new(1, 1);
    private readonly TarkovAssistantDbContext _database;
    private readonly ITarkovDevClient _client;

    public ImportCatalogHandler(TarkovAssistantDbContext database, ITarkovDevClient client)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(client);
        _database = database;
        _client = client;
    }

    internal async Task<DataImportRunResponse?> RunAsync(CancellationToken cancellationToken)
    {
        if (!await ImportLock.WaitAsync(0, cancellationToken).ConfigureAwait(false))
        {
            return null;
        }

        try
        {
            var run = await StartRunAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var sourceItems = await DownloadItemsAsync(cancellationToken).ConfigureAwait(false);
                var catalogVersion = await StoreAndPublishAsync(sourceItems, cancellationToken).ConfigureAwait(false);
                var finishedAt = DateTimeOffset.UtcNow;
                await CompleteRunAsync(run, sourceItems.Count, finishedAt, cancellationToken).ConfigureAwait(false);
                return new DataImportRunResponse(sourceItems.Count, catalogVersion.Version, catalogVersion.Published, finishedAt);
            }
            catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
            {
                await FailRunAsync(run, exception, CancellationToken.None).ConfigureAwait(false);
                throw;
            }
        }
        finally
        {
            ImportLock.Release();
        }
    }

    internal async Task<DataImportStatusResponse?> GetStatusAsync(CancellationToken cancellationToken)
    {
        var state = await _database.ImportStates
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.ImportName == ImportName, cancellationToken)
            .ConfigureAwait(false);
        if (state is null)
        {
            return null;
        }

        var importedRecordCount = state.CurrentRunId is null
            ? null
            : await _database.ImportRuns
                .AsNoTracking()
                .Where(run => run.Id == state.CurrentRunId.Value)
                .Select(run => (int?)run.ImportedRecordCount)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

        return new DataImportStatusResponse(
            state.Status.ToString(), state.LastAttemptStartedAt, state.LastAttemptFinishedAt,
            state.LastSucceededAt, importedRecordCount, state.FailureReason);
    }

    private async Task<ImportRun> StartRunAsync(CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var run = new ImportRun { ImportName = ImportName, Status = ImportStatus.Running, StartedAt = now };
        var state = await _database.ImportStates
            .SingleOrDefaultAsync(candidate => candidate.ImportName == ImportName, cancellationToken)
            .ConfigureAwait(false);
        if (state is null)
        {
            state = new ImportState { ImportName = ImportName };
            _database.ImportStates.Add(state);
        }

        state.Status = ImportStatus.Running;
        state.LastAttemptStartedAt = now;
        state.LastAttemptFinishedAt = null;
        state.FailureReason = null;
        state.CurrentRunId = run.Id;
        _database.ImportRuns.Add(run);
        await _database.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return run;
    }

    private async Task<IReadOnlyList<ItemDto>> DownloadItemsAsync(CancellationToken cancellationToken)
    {
        var items = new List<ItemDto>();
        for (var offset = 0; ; offset += PageSize)
        {
            var page = await _client.GetItemsAsync(new TarkovDevQuery(limit: PageSize, offset: offset), cancellationToken).ConfigureAwait(false);
            items.AddRange(page.Where(IsUsable));
            if (page.Count < PageSize)
            {
                break;
            }
        }

        if (items.Count == 0)
        {
            throw new InvalidDataException("Tarkov.dev returned no usable catalog items.");
        }

        return [.. items
            .GroupBy(item => item.Id, StringComparer.Ordinal)
            .Select(group => group.First())];
    }

    private async Task<(long Version, bool Published)> StoreAndPublishAsync(IReadOnlyList<ItemDto> sourceItems, CancellationToken cancellationToken)
    {
        var externalIds = sourceItems.Select(item => item.Id).ToArray();
        var existingItems = await _database.Items
            .Where(item => item.ExternalSource == "tarkov.dev" && externalIds.Contains(item.ExternalId))
            .ToDictionaryAsync(item => item.ExternalId, StringComparer.Ordinal, cancellationToken)
            .ConfigureAwait(false);

        foreach (var sourceItem in sourceItems)
        {
            if (!existingItems.TryGetValue(sourceItem.Id, out var item))
            {
                item = new Item { ExternalSource = "tarkov.dev", ExternalId = sourceItem.Id };
                _database.Items.Add(item);
                existingItems.Add(item.ExternalId, item);
            }

            Apply(sourceItem, item);
        }

        await _database.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await UpsertImagesAsync(sourceItems, existingItems, cancellationToken).ConfigureAwait(false);
        await _database.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var contentHash = CreateContentHash(sourceItems);
        var latest = await _database.CatalogVersions
            .OrderByDescending(version => version.Version)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        if (latest is not null && latest.ContentHash == contentHash)
        {
            return (latest.Version, false);
        }

        var nextVersion = latest is null ? 1 : checked(latest.Version + 1);
        _database.CatalogVersions.Add(new CatalogVersion
        {
            Version = nextVersion,
            PublishedAt = DateTimeOffset.UtcNow,
            ContentHash = contentHash
        });
        await _database.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return (nextVersion, true);
    }

    private async Task UpsertImagesAsync(
        IReadOnlyList<ItemDto> sourceItems,
        IReadOnlyDictionary<string, Item> items,
        CancellationToken cancellationToken)
    {
        var itemIds = items.Values.Select(item => item.Id).ToArray();
        var images = await _database.ItemImages
            .Where(image => itemIds.Contains(image.ItemId) && (image.Kind == ItemImageKind.Icon || image.Kind == ItemImageKind.Grid))
            .ToDictionaryAsync(image => (image.ItemId, image.Kind), cancellationToken)
            .ConfigureAwait(false);

        foreach (var sourceItem in sourceItems)
        {
            var item = items[sourceItem.Id];
            UpsertImage(images, item.Id, ItemImageKind.Icon, sourceItem.IconLink);
            UpsertImage(images, item.Id, ItemImageKind.Grid, sourceItem.GridImageLink);
        }
    }

    private void UpsertImage(Dictionary<(Guid ItemId, ItemImageKind Kind), ItemImage> images, Guid itemId, ItemImageKind kind, string? sourceUrl)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl))
        {
            return;
        }

        if (!images.TryGetValue((itemId, kind), out var image))
        {
            image = new ItemImage { ItemId = itemId, Kind = kind };
            _database.ItemImages.Add(image);
            images.Add((itemId, kind), image);
        }

        image.SourceUrl = sourceUrl;
    }

    private async Task CompleteRunAsync(ImportRun run, int importedRecordCount, DateTimeOffset finishedAt, CancellationToken cancellationToken)
    {
        var state = await _database.ImportStates.SingleAsync(candidate => candidate.ImportName == ImportName, cancellationToken).ConfigureAwait(false);
        run.Status = ImportStatus.Succeeded;
        run.ImportedRecordCount = importedRecordCount;
        run.FinishedAt = finishedAt;
        state.Status = ImportStatus.Succeeded;
        state.LastAttemptFinishedAt = finishedAt;
        state.LastSucceededAt = finishedAt;
        state.FailureReason = null;
        await _database.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task FailRunAsync(ImportRun run, Exception exception, CancellationToken cancellationToken)
    {
        var finishedAt = DateTimeOffset.UtcNow;
        var state = await _database.ImportStates.SingleAsync(candidate => candidate.ImportName == ImportName, cancellationToken).ConfigureAwait(false);
        var reason = exception.Message.Length <= 2_048 ? exception.Message : exception.Message[..2_048];
        run.Status = ImportStatus.Failed;
        run.FinishedAt = finishedAt;
        run.FailureReason = reason;
        state.Status = ImportStatus.Failed;
        state.LastAttemptFinishedAt = finishedAt;
        state.FailureReason = reason;
        await _database.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static bool IsUsable(ItemDto item) => !string.IsNullOrWhiteSpace(item.Id)
        && !string.IsNullOrWhiteSpace(item.Name)
        && !string.IsNullOrWhiteSpace(item.NormalizedName)
        && !string.IsNullOrWhiteSpace(item.ShortName)
        && item.Width > 0
        && item.Height > 0;

    private static void Apply(ItemDto source, Item target)
    {
        target.Name = source.Name!;
        target.NormalizedName = source.NormalizedName!;
        target.ShortName = source.ShortName!;
        target.Description = source.Description;
        target.BasePrice = source.BasePrice;
        target.Width = source.Width;
        target.Height = source.Height;
        target.BackgroundColor = source.BackgroundColor;
        target.WikiUrl = source.WikiLink;
        target.WeightKilograms = source.Weight is null ? null : (decimal)source.Weight.Value;
        target.MinimumFleaLevel = source.MinLevelForFlea;
        target.FleaMarketFee = source.FleaMarketFee;
        target.SourceUpdatedAt = DateTimeOffset.TryParse(source.Updated, out var updated) ? updated : null;
        target.UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static string CreateContentHash(IEnumerable<ItemDto> items)
    {
        var content = string.Join('\n', items
            .OrderBy(item => item.Id, StringComparer.Ordinal)
            .Select(item => string.Join('|', item.Id, item.Name, item.NormalizedName, item.ShortName, item.Width, item.Height, item.IconLink, item.GridImageLink)));
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content)));
    }
}
