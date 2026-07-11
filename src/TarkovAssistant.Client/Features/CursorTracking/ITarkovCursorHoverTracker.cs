using System.Collections.Generic;

namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Streams dwell-aware global cursor observations only while monitoring the supported Tarkov window.
/// </summary>
internal interface ITarkovCursorHoverTracker
{
    /// <summary>
    /// Begins cursor observation until the caller cancels the asynchronous stream.
    /// </summary>
    /// <param name="cancellationToken">Stops global cursor polling and releases the timer.</param>
    /// <returns>A sequence of classified cursor states sampled at the configured interval.</returns>
    public IAsyncEnumerable<CursorTrackingUpdate> TrackAsync(CancellationToken cancellationToken);
}
