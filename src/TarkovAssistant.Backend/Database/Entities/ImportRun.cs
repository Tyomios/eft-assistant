namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class ImportRun : Entity
{
    internal string ImportName { get; set; } = string.Empty;

    internal ImportStatus Status { get; set; }

    internal DateTimeOffset StartedAt { get; set; }

    internal DateTimeOffset? FinishedAt { get; set; }

    internal int ImportedRecordCount { get; set; }

    internal string? FailureReason { get; set; }
}
