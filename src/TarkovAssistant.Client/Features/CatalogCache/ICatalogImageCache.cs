namespace TarkovAssistant.Client.Features.CatalogCache;

/// <summary>
/// Reads catalog images exclusively from the configured local cache boundary.
/// </summary>
internal interface ICatalogImageCache
{
    /// <summary>
    /// Reads and validates a locally cached catalog image.
    /// </summary>
    /// <param name="reference">The cache-relative image reference and optional integrity hash.</param>
    /// <param name="cancellationToken">Stops pending file I/O when cancellation is requested.</param>
    /// <returns>The image bytes or a classified expected failure.</returns>
    public ValueTask<CatalogImageReadResult> ReadAsync(
        CatalogImageReference reference,
        CancellationToken cancellationToken);
}
