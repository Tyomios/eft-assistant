namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class GraphQlRequest
{
    internal GraphQlRequest(string query, object variables)
    {
        Query = query;
        Variables = variables;
    }

    /// <summary>Gets the GraphQL query document.</summary>
    public string Query { get; }

    /// <summary>Gets the variables supplied to the query.</summary>
    public object Variables { get; }
}
