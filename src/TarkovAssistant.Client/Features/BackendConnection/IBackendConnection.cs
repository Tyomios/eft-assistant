namespace TarkovAssistant.Client.Features.BackendConnection;

/// <summary>
/// Defines the process boundary used to check the local backend without coupling UI code to HTTP.
/// </summary>
internal interface IBackendConnection
{
    /// <summary>
    /// Checks whether the configured local backend is available.
    /// </summary>
    /// <param name="cancellationToken">Stops the check when application shutdown or caller cancellation is requested.</param>
    /// <returns>The observed connection state and its UTC observation time.</returns>
    public ValueTask<BackendConnectionStatus> CheckAsync(CancellationToken cancellationToken);
}
