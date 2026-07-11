using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Backend.Infrastructure;
using TarkovAssistant.Backend.Integrations.TarkovDev;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Features.TarkovData;

internal static class TarkovDataEndpoints
{
    internal static IEndpointRouteBuilder MapTarkovDataEndpoints(this IEndpointRouteBuilder endpoints)
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
        return TypedResults.Ok(PageResponseFactory.Create(query, items));
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
        return TypedResults.Ok(PageResponseFactory.Create(query, items));
    }

    private static async Task<IResult> GetAmmoAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var ammo = await client.GetAmmoAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponseFactory.Create(query, ammo));
    }

    private static async Task<IResult> GetTasksAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var tasks = await client.GetTasksAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponseFactory.Create(query, tasks));
    }

    private static async Task<IResult> GetTradersAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var traders = await client.GetTradersAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponseFactory.Create(query, traders));
    }

    private static async Task<IResult> GetHideoutStationsAsync(
        ITarkovDevClient client,
        [AsParameters] TarkovPageParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = parameters.ToQuery();
        var stations = await client.GetHideoutStationsAsync(query, cancellationToken);
        return TypedResults.Ok(PageResponseFactory.Create(query, stations));
    }

}
