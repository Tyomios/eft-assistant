using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

/// <summary>
/// Defines read operations supported by the Tarkov.dev integration.
/// </summary>
public interface ITarkovDevClient
{
    /// <summary>Retrieves a page of catalog items.</summary>
    /// <param name="query">The localization, game mode, paging, and item-type parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The items returned by Tarkov.dev.</returns>
    public Task<IReadOnlyList<ItemDto>> GetItemsAsync(TarkovDevQuery query, CancellationToken cancellationToken);

    /// <summary>Retrieves an item by its Tarkov.dev identifier.</summary>
    /// <param name="id">The Tarkov.dev item identifier.</param>
    /// <param name="query">The localization and game-mode parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The matching item, or <see langword="null"/> when no item exists.</returns>
    public Task<ItemDto?> GetItemAsync(string id, TarkovDevQuery query, CancellationToken cancellationToken);

    /// <summary>Retrieves a page of weapons and their ammunition compatibility.</summary>
    /// <param name="query">The localization, game mode, and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The weapons returned by Tarkov.dev.</returns>
    public Task<IReadOnlyList<ItemDto>> GetWeaponsAsync(TarkovDevQuery query, CancellationToken cancellationToken);

    /// <summary>Retrieves a page of ammunition records.</summary>
    /// <param name="query">The localization, game mode, and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The ammunition records returned by Tarkov.dev.</returns>
    public Task<IReadOnlyList<AmmoDto>> GetAmmoAsync(TarkovDevQuery query, CancellationToken cancellationToken);

    /// <summary>Retrieves a page of tasks.</summary>
    /// <param name="query">The localization, game mode, and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The tasks returned by Tarkov.dev.</returns>
    public Task<IReadOnlyList<TaskDto>> GetTasksAsync(TarkovDevQuery query, CancellationToken cancellationToken);

    /// <summary>Retrieves a page of traders.</summary>
    /// <param name="query">The localization, game mode, and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The traders returned by Tarkov.dev.</returns>
    public Task<IReadOnlyList<TraderDto>> GetTradersAsync(TarkovDevQuery query, CancellationToken cancellationToken);

    /// <summary>Retrieves a page of hideout stations and their item requirements.</summary>
    /// <param name="query">The localization, game mode, and paging parameters.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>The hideout stations returned by Tarkov.dev.</returns>
    public Task<IReadOnlyList<HideoutStationDto>> GetHideoutStationsAsync(TarkovDevQuery query, CancellationToken cancellationToken);
}
