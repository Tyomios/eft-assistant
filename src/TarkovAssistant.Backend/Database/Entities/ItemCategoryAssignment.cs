namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class ItemCategoryAssignment : Entity
{
    internal Guid ItemId { get; set; }

    internal Guid CategoryId { get; set; }
}
