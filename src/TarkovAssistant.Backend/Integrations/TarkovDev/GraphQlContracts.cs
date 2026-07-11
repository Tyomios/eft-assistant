using System.Text.Json.Serialization;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed record GraphQlRequest(string Query, object Variables);

internal sealed class GraphQlResponse<TData>
{
    public TData? Data { get; init; }
    public IReadOnlyList<GraphQlError> Errors { get; init; } = [];
}

internal sealed class GraphQlError
{
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<object> Path { get; init; } = [];
    public IReadOnlyList<GraphQlLocation> Locations { get; init; } = [];
}

internal sealed class GraphQlLocation
{
    public int Line { get; init; }
    public int Column { get; init; }
}

internal sealed class ItemsData
{
    public IReadOnlyList<Contracts.ItemDto> Items { get; init; } = [];
}

internal sealed class ItemData
{
    public Contracts.ItemDto? Item { get; init; }
}

internal sealed class AmmoData
{
    public IReadOnlyList<Contracts.AmmoDto> Ammo { get; init; } = [];
}

internal sealed class TasksData
{
    public IReadOnlyList<Contracts.TaskDto> Tasks { get; init; } = [];
}

internal sealed class TradersData
{
    public IReadOnlyList<Contracts.TraderDto> Traders { get; init; } = [];
}

internal sealed class HideoutStationsData
{
    [JsonPropertyName("hideoutStations")]
    public IReadOnlyList<Contracts.HideoutStationDto> HideoutStations { get; init; } = [];
}
