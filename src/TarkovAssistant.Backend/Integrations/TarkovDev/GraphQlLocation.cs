namespace TarkovAssistant.Backend.Integrations.TarkovDev;

internal sealed class GraphQlLocation
{
    /// <summary>Gets the one-based query line number.</summary>
    public int Line { get; init; }

    /// <summary>Gets the one-based query column number.</summary>
    public int Column { get; init; }
}
