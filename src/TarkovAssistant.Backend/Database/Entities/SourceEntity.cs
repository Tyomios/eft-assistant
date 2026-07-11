using System.ComponentModel.DataAnnotations.Schema;

namespace TarkovAssistant.Backend.Database.Entities;

[NotMapped]
internal abstract class SourceEntity : Entity
{
    internal string ExternalSource { get; set; } = string.Empty;

    internal string ExternalId { get; set; } = string.Empty;

    internal DateTimeOffset? SourceUpdatedAt { get; set; }

    internal DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    internal DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
