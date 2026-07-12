using System.IO;
using TarkovAssistant.Client.Features.ItemOverlay;

namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Coordinates one captured hover region through recognition and safe overlay presentation.
/// </summary>
internal sealed class HoverRecognitionCoordinator
{
    private readonly IHoveredItemRecognizer _recognizer;
    private readonly IItemOverlay _overlay;
    private readonly double _minimumStrongConfidence;

    /// <summary>
    /// Initializes the hover recognition boundary.
    /// </summary>
    /// <param name="recognizer">The deterministic local item recognizer.</param>
    /// <param name="overlay">The non-interactive result overlay.</param>
    /// <param name="options">The threshold controlling strong result presentation.</param>
    /// <exception cref="ArgumentOutOfRangeException">The configured confidence threshold is outside zero through one.</exception>
    public HoverRecognitionCoordinator(
        IHoveredItemRecognizer recognizer,
        IItemOverlay overlay,
        HoverRecognitionOptions options)
    {
        ArgumentNullException.ThrowIfNull(recognizer);
        ArgumentNullException.ThrowIfNull(overlay);
        ArgumentNullException.ThrowIfNull(options);

        if (options.MinimumStrongConfidence is < 0 or > 1 || options.MaximumAlternatives <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "The recognition settings are invalid.");
        }

        _recognizer = recognizer;
        _overlay = overlay;
        _minimumStrongConfidence = options.MinimumStrongConfidence;
    }

    /// <summary>
    /// Processes one captured hover region and displays a user-safe result.
    /// </summary>
    /// <param name="request">The stationary cursor position and captured BGRA32 pixels.</param>
    /// <param name="cancellationToken">Stops work when the cursor moves or shutdown is requested.</param>
    /// <returns>The recognition and overlay processing outcome.</returns>
    public async ValueTask<HoverProcessingResult> ProcessAsync(
        HoverRecognitionRequest request,
        CancellationToken cancellationToken)
    {
        var progressOverlayResult = await _overlay
            .ShowAsync(CreateRecognitionInProgressContent(), request.CursorPosition, CancellationToken.None)
            .ConfigureAwait(false);
        if (progressOverlayResult.State != OverlayOperationState.Completed)
        {
            return new HoverProcessingResult(HoverProcessingState.OverlayFailed, progressOverlayResult.Detail);
        }

        HoverRecognitionResult recognition;
        try
        {
            recognition = await _recognizer.RecognizeAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (IsExpectedRecognitionFailure(exception))
        {
            var errorContent = CreateRecognitionFailureContent();
            var errorOverlayResult = await _overlay
                .ShowAsync(errorContent, request.CursorPosition, CancellationToken.None)
                .ConfigureAwait(false);
            return errorOverlayResult.State == OverlayOperationState.Completed
                ? new HoverProcessingResult(
                    HoverProcessingState.RecognitionFailed,
                    "Recognition could not process the captured region.")
                : new HoverProcessingResult(HoverProcessingState.OverlayFailed, errorOverlayResult.Detail);
        }

        var content = CreateOverlayContent(recognition);
        var overlayResult = await _overlay
            .ShowAsync(content, request.CursorPosition, cancellationToken)
            .ConfigureAwait(false);

        return overlayResult.State == OverlayOperationState.Completed
            ? new HoverProcessingResult(HoverProcessingState.Displayed, null)
            : new HoverProcessingResult(HoverProcessingState.OverlayFailed, overlayResult.Detail);
    }

    /// <summary>
    /// Hides any current result after the cursor leaves the recognized inventory cell.
    /// </summary>
    /// <param name="cancellationToken">Stops the operation before it reaches the UI thread.</param>
    /// <returns>The safe overlay operation result.</returns>
    public Task<OverlayOperationResult> HideAsync(CancellationToken cancellationToken)
    {
        return _overlay.HideAsync(cancellationToken);
    }

    private static ItemOverlayContent CreateRecognitionFailureContent()
    {
        return new ItemOverlayContent(
            "Recognition unavailable",
            "The captured inventory region could not be processed.",
            "Move the cursor away and try again. The client will continue using only local data.",
            null,
            OverlayTone.Error,
            ReadOnlyMemory<byte>.Empty);
    }

    private static ItemOverlayContent CreateRecognitionInProgressContent()
    {
        return new ItemOverlayContent(
            "Recognizing item",
            "Searching the local catalog…",
            "No game data is read and no network request is being made.",
            null,
            OverlayTone.Informational,
            ReadOnlyMemory<byte>.Empty);
    }

    private static bool IsExpectedRecognitionFailure(Exception exception)
    {
        return exception is IOException
            or UnauthorizedAccessException
            or InvalidOperationException
            or NotSupportedException
            or ArgumentException;
    }

    private ItemOverlayContent CreateOverlayContent(HoverRecognitionResult recognition)
    {
        if (!double.IsFinite(recognition.Confidence) || recognition.Confidence is < 0 or > 1)
        {
            return CreateRecognitionFailureContent();
        }

        if (recognition.State == HoverRecognitionState.Recognized
            && recognition.ItemId.HasValue
            && recognition.Confidence >= _minimumStrongConfidence)
        {
            return new ItemOverlayContent(
                "Item recognized",
                recognition.ItemId.Value.ToString("D"),
                "Local evaluation will replace the catalog identifier with an explainable recommendation.",
                recognition.Confidence,
                OverlayTone.Positive,
                ReadOnlyMemory<byte>.Empty);
        }

        return recognition.State switch
        {
            HoverRecognitionState.CacheUnavailable => new ItemOverlayContent(
                "Local catalog unavailable",
                "Recognition requires a synchronized image cache.",
                recognition.Detail ?? "Synchronize the catalog before using hover recognition.",
                null,
                OverlayTone.Error,
                ReadOnlyMemory<byte>.Empty),
            HoverRecognitionState.Failed => CreateRecognitionFailureContent(),
            HoverRecognitionState.NotRecognized => new ItemOverlayContent(
                "Item not recognized",
                "No local catalog image matched this inventory cell.",
                recognition.Detail,
                recognition.Confidence,
                OverlayTone.Warning,
                ReadOnlyMemory<byte>.Empty),
            _ => new ItemOverlayContent(
                "Recognition uncertain",
                "No strong recommendation is available for this item.",
                recognition.Detail ?? "Move the cursor away and hover again for a new capture.",
                recognition.Confidence,
                OverlayTone.Warning,
                ReadOnlyMemory<byte>.Empty),
        };
    }
}
