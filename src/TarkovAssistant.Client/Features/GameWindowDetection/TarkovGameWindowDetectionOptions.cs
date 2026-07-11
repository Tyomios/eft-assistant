namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Defines the expected executable name for the supported Tarkov game client.
/// </summary>
internal sealed class TarkovGameWindowDetectionOptions
{
    /// <summary>
    /// Gets the process name without an executable extension used by the Tarkov game client.
    /// </summary>
    public string ProcessName { get; init; } = "EscapeFromTarkov";
}
