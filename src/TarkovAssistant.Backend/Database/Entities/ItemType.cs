namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class ItemType : Entity
{
    internal Guid ItemId { get; set; }

    internal string Value { get; set; } = string.Empty;
}
