namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class GraphQlResponse<TData>
{
    /// <summary>Gets the response data.</summary>
    public TData? Data { get; init; }

    /// <summary>Gets the GraphQL execution errors.</summary>
    public IReadOnlyList<GraphQlError> Errors { get; init; } = [];
}
