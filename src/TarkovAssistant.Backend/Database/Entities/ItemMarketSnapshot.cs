namespace TarkovAssistant.Backend.Database.Entities;

internal sealed class ItemMarketSnapshot : Entity
{
    internal Guid ItemId { get; set; }

    internal int? Average24HourPrice { get; set; }

    internal int? LastLowPrice { get; set; }

    internal int? Low24HourPrice { get; set; }

    internal int? High24HourPrice { get; set; }

    internal decimal? ChangeLast48Hours { get; set; }

    internal decimal? ChangeLast48HoursPercent { get; set; }

    internal int? LastOfferCount { get; set; }

    internal DateTimeOffset ObservedAt { get; set; }
}
