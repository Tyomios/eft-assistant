using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Infrastructure;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

public static class TarkovDataEndpoints
{
    private static readonly HashSet<string> Languages =
    [
        "cs", "de", "en", "es", "fr", "hu", "it", "ja", "ko", "pl", "pt", "ro", "ru", "sk", "tr", "zh"
    ];

    private static readonly HashSet<string> GameModes = ["regular", "pve"];
    private static readonly HashSet<string> ItemTypes =
    [
        "ammo", "ammoBox", "any", "armor", "armorPlate", "backpack", "barter", "container", "glasses",
        "grenade", "gun", "headphones", "helmet", "injectors", "keys", "markedOnly", "meds", "mods",
        "noFlea", "pistolGrip", "poster", "preset", "provisions", "rig", "specialSlot", "suppressor", "wearable"
    ];

    public static IEndpointRouteBuilder MapTarkovDataEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/tarkov")
            .WithTags("Tarkov.dev");

        group.MapGet("/items", GetItemsAsync)
            .WithName("GetTarkovItems")
            .WithSummary("Returns a page of items from Tarkov.dev")
            .Produces<PageResponse<ItemDto>>();

        group.MapGet("/items/{id}", GetItemAsync)
            .WithName("GetTarkovItem")
            .WithSummary("Returns an item by its Tarkov.dev ID")
            .Produces<ItemDto>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/weapons", GetWeaponsAsync)
            .WithName("GetTarkovWeapons")
            .WithSummary("Returns weapons and ammunition compatibility")
            .Produces<PageResponse<ItemDto>>();

        group.MapGet("/ammo", GetAmmoAsync)
            .WithName("GetTarkovAmmo")
            .WithSummary("Returns ammunition statistics")
            .Produces<PageResponse<AmmoDto>>();

        group.MapGet("/tasks", GetTasksAsync)
            .WithName("GetTarkovTasks")
            .WithSummary("Returns quests and their item objectives")
            .Produces<PageResponse<TaskDto>>();

        group.MapGet("/traders", GetTradersAsync)
            .WithName("GetTarkovTraders")
            .WithSummary("Returns traders and loyalty levels")
            .Produces<PageResponse<TraderDto>>();

        group.MapGet("/hideout-stations", GetHideoutStationsAsync)
            .WithName("GetTarkovHideoutStations")
            .WithSummary("Returns hideout stations and item requirements")
            .Produces<PageResponse<HideoutStationDto>>();

        return endpoints;
    }

    private static async Task<IResult> GetItemsAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery(allowItemType: true);
        var items = await client.GetItemsAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponse<ItemDto>.From(query, items));
    }

    private static async Task<IResult> GetItemAsync(
        string id,
        ITarkovDevClient client,
        [AsParameters] TarkovLocaleParameters parameters,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new RequestValidationException("Item ID is required.");
        }

        var query = parameters.ToQuery();
        var item = await client.GetItemAsync(id, query, cancellationToken);
        return item is null ? TypedResults.NotFound() : TypedResults.Ok(item);
    }

    private static async Task<IResult> GetWeaponsAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var items = await client.GetWeaponsAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponse<ItemDto>.From(query, items));
    }

    private static async Task<IResult> GetAmmoAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var ammo = await client.GetAmmoAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponse<AmmoDto>.From(query, ammo));
    }

    private static async Task<IResult> GetTasksAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var tasks = await client.GetTasksAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponse<TaskDto>.From(query, tasks));
    }

    private static async Task<IResult> GetTradersAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var traders = await client.GetTradersAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponse<TraderDto>.From(query, traders));
    }

    private static async Task<IResult> GetHideoutStationsAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var stations = await client.GetHideoutStationsAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponse<HideoutStationDto>.From(query, stations));
    }

    public sealed record TarkovLocaleParameters(string Language = "en", string GameMode = "regular")
    {
        public TarkovDevQuery ToQuery()
        {
            ValidateLocale(Language, GameMode);
            return new TarkovDevQuery(Language, GameMode);
        }
    }

    public sealed record TarkovPageParameters(
        string Language = "en",
        string GameMode = "regular",
        int Limit = 100,
        int Offset = 0)
    {
        public TarkovDevQuery ToQuery()
        {
            Validate(Language, GameMode, Limit, Offset, null, false);
            return new TarkovDevQuery(Language, GameMode, Limit, Offset);
        }
    }

    public sealed record TarkovQueryParameters(
        string Language = "en",
        string GameMode = "regular",
        int Limit = 100,
        int Offset = 0,
        string? ItemType = null)
    {
        public TarkovDevQuery ToQuery(bool allowItemType)
        {
            Validate(Language, GameMode, Limit, Offset, ItemType, allowItemType);
            return new TarkovDevQuery(Language, GameMode, Limit, Offset, ItemType ?? "any");
        }
    }

    private static void Validate(
        string language,
        string gameMode,
        int limit,
        int offset,
        string? itemType,
        bool allowItemType)
    {
        ValidateLocale(language, gameMode);

        if (limit is < 1 or > 500)
        {
            throw new RequestValidationException("Limit must be between 1 and 500.");
        }

        if (offset < 0)
        {
            throw new RequestValidationException("Offset cannot be negative.");
        }

        if (itemType is not null && (!allowItemType || !ItemTypes.Contains(itemType)))
        {
            throw new RequestValidationException($"Unsupported item type '{itemType}'.");
        }
    }

    private static void ValidateLocale(string language, string gameMode)
    {
        if (!Languages.Contains(language))
        {
            throw new RequestValidationException($"Unsupported language '{language}'.");
        }

        if (!GameModes.Contains(gameMode))
        {
            throw new RequestValidationException($"Unsupported game mode '{gameMode}'.");
        }
    }
}

public sealed record PageResponse<T>(int Offset, int Limit, int Count, IReadOnlyList<T> Items)
{
    public static PageResponse<T> From(TarkovDevQuery query, IReadOnlyList<T> items) =>
        new(query.Offset, query.Limit, items.Count, items);
}
