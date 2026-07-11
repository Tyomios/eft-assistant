namespace TarkovAssistant.Client.Features.BackendConnection;

/// <summary>
/// Defines the local backend endpoint used by future catalog synchronization operations.
/// </summary>
internal sealed class BackendConnectionOptions
{
    /// <summary>
    /// Gets the default backend address exposed to Windows by Docker Compose.
    /// </summary>
    public static Uri DefaultBaseAddress { get; } = new("http://localhost:5080");

    /// <summary>
    /// Gets the configured backend base address.
    /// </summary>
    public Uri BaseAddress { get; init; } = DefaultBaseAddress;
}
