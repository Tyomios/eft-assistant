namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>
/// Synchronizes the backend catalog into an offline, versioned local cache.
/// </summary>
internal interface ICatalogSynchronizer
{
    /// <summary>
    /// Downloads and atomically activates the latest complete catalog when it differs from the local one.
    /// </summary>
    /// <param name="cancellationToken">Stops pending network or disk work without changing the active catalog.</param>
    /// <returns>The completed synchronization outcome.</returns>
    public Task<CatalogSyncResult> SynchronizeAsync(CancellationToken cancellationToken);
}
