namespace TarkovAssistant.Backend.Features.DataImport;

/// <summary>
/// Describes the latest catalog-import state for manual inspection.
/// </summary>
/// <param name="Status">The latest import status.</param>
/// <param name="LastAttemptStartedAt">The UTC start time of the latest attempt.</param>
/// <param name="LastAttemptFinishedAt">The UTC completion time of the latest attempt.</param>
/// <param name="LastSucceededAt">The UTC completion time of the latest successful attempt.</param>
/// <param name="ImportedRecordCount">The count stored by the latest completed run, when available.</param>
/// <param name="FailureReason">A concise failure reason, when the latest run failed.</param>
public sealed record DataImportStatusResponse(
    string Status,
    DateTimeOffset? LastAttemptStartedAt,
    DateTimeOffset? LastAttemptFinishedAt,
    DateTimeOffset? LastSucceededAt,
    int? ImportedRecordCount,
    string? FailureReason);
