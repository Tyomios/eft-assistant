namespace TarkovAssistant.Backend.Features.DataImport;

/// <summary>
/// Describes a completed catalog-import attempt.
/// </summary>
/// <param name="ImportedRecordCount">The number of source items stored during the attempt.</param>
/// <param name="CatalogVersion">The current published catalog version.</param>
/// <param name="Published">Whether this attempt published a new catalog version.</param>
/// <param name="FinishedAt">The UTC time at which the import completed.</param>
public sealed record DataImportRunResponse(int ImportedRecordCount, long CatalogVersion, bool Published, DateTimeOffset FinishedAt);
