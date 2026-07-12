namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Reads the locally synchronized catalog entries that have grid images available for matching.
/// </summary>
internal interface ICatalogGridImageIndex
{
    /// <summary>
    /// Reads the current local grid-image catalog without contacting the backend.
    /// </summary>
    /// <param name="cancellationToken">Stops the local index read when recognition is superseded.</param>
    /// <returns>The locally indexed grid image entries.</returns>
    public ValueTask<IReadOnlyList<CatalogGridImage>> ReadAsync(CancellationToken cancellationToken);
}
