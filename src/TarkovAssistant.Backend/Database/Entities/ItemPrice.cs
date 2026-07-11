namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class ItemPrice : Entity
{
    internal Guid ItemId { get; set; }

    internal Guid VendorId { get; set; }

    internal PriceDirection Direction { get; set; }

    internal int? Price { get; set; }

    internal string? CurrencyCode { get; set; }

    internal int? PriceRoubles { get; set; }

    internal DateTimeOffset ObservedAt { get; set; }
}
