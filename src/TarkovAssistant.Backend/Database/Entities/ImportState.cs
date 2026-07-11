namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class ImportState : Entity
{
    internal string ImportName { get; set; } = string.Empty;

    internal ImportStatus Status { get; set; }

    internal DateTimeOffset? LastAttemptStartedAt { get; set; }

    internal DateTimeOffset? LastAttemptFinishedAt { get; set; }

    internal DateTimeOffset? LastSucceededAt { get; set; }

    internal string? FailureReason { get; set; }

    internal Guid? CurrentRunId { get; set; }
}
