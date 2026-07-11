namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class GraphQlError
{
    /// <summary>Gets the GraphQL error message.</summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>Gets the response path at which the error occurred.</summary>
    public IReadOnlyList<object> Path { get; init; } = [];

    /// <summary>Gets the query locations associated with the error.</summary>
    public IReadOnlyList<GraphQlLocation> Locations { get; init; } = [];
}
