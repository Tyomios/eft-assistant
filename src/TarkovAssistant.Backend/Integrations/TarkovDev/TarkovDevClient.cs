using System.Net.Http.Json;
using TarkovAssistant.Backend.Integrations.TarkovDev.Contracts;

namespace TarkovAssistant.Backend.Integrations.TarkovDev;

public sealed class TarkovDevClient(HttpClient httpClient, ILogger<TarkovDevClient> logger)
    : ITarkovDevClient
{
    public async Task<IReadOnlyList<ItemDto>> GetItemsAsync(
        TarkovDevQuery query,
        CancellationToken cancellationToken)
    {
        var data = await SendAsync<ItemsData>(
            TarkovDevQueries.Items,
            new
            {
                lang = query.Language,
                gameMode = query.GameMode,
                limit = query.Limit,
                offset = query.Offset,
                type = query.ItemType
            },
            cancellationToken);

        return data.Items;
    }

    public async Task<ItemDto?> GetItemAsync(
        string id,
        TarkovDevQuery query,
        CancellationToken cancellationToken)
    {
        var variables = new
        {
            id,
            lang = query.Language,
            gameMode = query.GameMode
        };
        var data = await SendAsync<ItemData>(TarkovDevQueries.Item, variables, cancellationToken);
        return data.Item;
    }

    public Task<IReadOnlyList<ItemDto>> GetWeaponsAsync(
        TarkovDevQuery query,
        CancellationToken cancellationToken) =>
        GetItemsAsync(query with { ItemType = "gun" }, cancellationToken);

    public async Task<IReadOnlyList<AmmoDto>> GetAmmoAsync(
        TarkovDevQuery query,
        CancellationToken cancellationToken)
    {
        var data = await SendAsync<AmmoData>(
            TarkovDevQueries.Ammo,
            CreatePageVariables(query),
            cancellationToken);

        return data.Ammo;
    }

    public async Task<IReadOnlyList<TaskDto>> GetTasksAsync(
        TarkovDevQuery query,
        CancellationToken cancellationToken)
    {
        var data = await SendAsync<TasksData>(
            TarkovDevQueries.Tasks,
            CreatePageVariables(query),
            cancellationToken);

        return data.Tasks;
    }

    public async Task<IReadOnlyList<TraderDto>> GetTradersAsync(
        TarkovDevQuery query,
        CancellationToken cancellationToken)
    {
        var data = await SendAsync<TradersData>(
            TarkovDevQueries.Traders,
            CreatePageVariables(query),
            cancellationToken);

        return data.Traders;
    }

    public async Task<IReadOnlyList<HideoutStationDto>> GetHideoutStationsAsync(
        TarkovDevQuery query,
        CancellationToken cancellationToken)
    {
        var data = await SendAsync<HideoutStationsData>(
            TarkovDevQueries.HideoutStations,
            CreatePageVariables(query),
            cancellationToken);

        return data.HideoutStations;
    }

    private async Task<TData> SendAsync<TData>(
        string query,
        object variables,
        CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(
                string.Empty,
                new GraphQlRequest(query, variables),
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var graphQlResponse = await response.Content.ReadFromJsonAsync<GraphQlResponse<TData>>(
                cancellationToken: cancellationToken)
                ?? throw new TarkovDevException("Tarkov.dev returned an empty response.");

            if (graphQlResponse.Errors.Count > 0)
            {
                throw new TarkovDevGraphQlException([.. graphQlResponse.Errors.Select(error => error.Message)]);
            }

            if (graphQlResponse.Data is null)
            {
                throw new TarkovDevException("Tarkov.dev returned no data.");
            }

            return graphQlResponse.Data;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (TarkovDevException)
        {
            throw;
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Request to Tarkov.dev failed after resilience policies were applied");
            throw new TarkovDevException("Unable to retrieve data from Tarkov.dev.", exception);
        }
    }

    private static object CreatePageVariables(TarkovDevQuery query) => new
    {
        lang = query.Language,
        gameMode = query.GameMode,
        limit = query.Limit,
        offset = query.Offset
    };
}
