using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

public interface ITarkovDevClient
{
    Task<IReadOnlyList<ItemDto>> GetItemsAsync(TarkovDevQuery query, CancellationToken cancellationToken);
    Task<ItemDto?> GetItemAsync(string id, TarkovDevQuery query, CancellationToken cancellationToken);
    Task<IReadOnlyList<ItemDto>> GetWeaponsAsync(TarkovDevQuery query, CancellationToken cancellationToken);
    Task<IReadOnlyList<AmmoDto>> GetAmmoAsync(TarkovDevQuery query, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskDto>> GetTasksAsync(TarkovDevQuery query, CancellationToken cancellationToken);
    Task<IReadOnlyList<TraderDto>> GetTradersAsync(TarkovDevQuery query, CancellationToken cancellationToken);
    Task<IReadOnlyList<HideoutStationDto>> GetHideoutStationsAsync(TarkovDevQuery query, CancellationToken cancellationToken);
}

public sealed record TarkovDevQuery(
    string Language = "en",
    string GameMode = "regular",
    int Limit = 100,
    int Offset = 0,
    string ItemType = "any");
