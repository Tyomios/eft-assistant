namespace TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

/// <summary>
/// Represents a vendor price for an item.
/// </summary>
public sealed class ItemPriceDto
{
    /// <summary>Gets the vendor offering the price.</summary>
    public VendorDto Vendor { get; init; } = new();

    /// <summary>Gets the price in its original currency.</summary>
    public int? Price { get; init; }

    /// <summary>Gets the original currency code.</summary>
    public string? Currency { get; init; }

    /// <summary>Gets the price converted to roubles.</summary>
    public int? PriceRUB { get; init; }
}
