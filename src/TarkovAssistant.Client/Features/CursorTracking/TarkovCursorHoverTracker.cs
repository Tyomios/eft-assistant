using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TarkovAssistant.Client.Features.GameWindowDetection;

namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Polls the physical cursor and emits one recognition trigger after dwell inside the active Tarkov window.
/// </summary>
internal sealed class TarkovCursorHoverTracker : ITarkovCursorHoverTracker
{
    private readonly ITarkovGameWindowDetector _gameWindowDetector;
    private readonly IPhysicalCursorPositionProvider _cursorPositionProvider;
    private readonly CursorTrackingOptions _options;

    /// <summary>
    /// Initializes the hover tracker.
    /// </summary>
    /// <param name="gameWindowDetector">The detector for the supported Tarkov top-level window.</param>
    /// <param name="cursorPositionProvider">The documented Windows cursor position provider.</param>
    /// <param name="options">The polling, dwell, and positional tolerance settings.</param>
    public TarkovCursorHoverTracker(
        ITarkovGameWindowDetector gameWindowDetector,
        IPhysicalCursorPositionProvider cursorPositionProvider,
        CursorTrackingOptions options)
    {
        ArgumentNullException.ThrowIfNull(gameWindowDetector);
        ArgumentNullException.ThrowIfNull(cursorPositionProvider);
        ArgumentNullException.ThrowIfNull(options);

        if (options.PollInterval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "The cursor polling interval must be positive.");
        }

        _gameWindowDetector = gameWindowDetector;
        _cursorPositionProvider = cursorPositionProvider;
        _options = options;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<CursorTrackingUpdate> TrackAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var dwellEvaluator = new CursorDwellEvaluator(_options);
        var stopwatch = Stopwatch.StartNew();
        using var timer = new PeriodicTimer(_options.PollInterval);

        while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
        {
            var windowResult = _gameWindowDetector.Detect();
            if (windowResult.State != GameWindowDetectionState.Found || windowResult.Window is not { } gameWindow)
            {
                dwellEvaluator.Reset();
                yield return GameUnavailable(windowResult);
                continue;
            }

            if (!gameWindow.IsForeground)
            {
                dwellEvaluator.Reset();
                yield return new CursorTrackingUpdate(
                    CursorTrackingState.GameInactive,
                    null,
                    gameWindow,
                    TimeSpan.Zero,
                    false,
                    "The Tarkov window is not the foreground window.");
                continue;
            }

            var cursorResult = _cursorPositionProvider.GetPosition();
            if (cursorResult.State != CursorPositionReadState.Available || cursorResult.Position is not { } cursorPosition)
            {
                dwellEvaluator.Reset();
                yield return new CursorTrackingUpdate(
                    CursorTrackingState.CursorUnavailable,
                    null,
                    gameWindow,
                    TimeSpan.Zero,
                    false,
                    cursorResult.Detail ?? "The Windows cursor position is unavailable.");
                continue;
            }

            if (!gameWindow.Bounds.Contains(cursorPosition))
            {
                dwellEvaluator.Reset();
                yield return new CursorTrackingUpdate(
                    CursorTrackingState.OutsideGameWindow,
                    cursorPosition,
                    gameWindow,
                    TimeSpan.Zero,
                    false,
                    null);
                continue;
            }

            var dwell = dwellEvaluator.Evaluate(cursorPosition, stopwatch.Elapsed);
            yield return new CursorTrackingUpdate(
                dwell.State == CursorDwellState.Satisfied
                    ? CursorTrackingState.DwellSatisfied
                    : CursorTrackingState.WaitingForDwell,
                cursorPosition,
                gameWindow,
                dwell.StableFor,
                dwell.ShouldTriggerRecognition,
                null);
        }
    }

    private static CursorTrackingUpdate GameUnavailable(GameWindowDetectionResult windowResult)
    {
        var detail = windowResult.State switch
        {
            GameWindowDetectionState.Minimized => "The Tarkov window is minimized.",
            GameWindowDetectionState.NotFound => "The Tarkov game window was not found.",
            _ => windowResult.Detail ?? "The Tarkov game window is unavailable.",
        };
        return new CursorTrackingUpdate(
            CursorTrackingState.GameUnavailable,
            null,
            windowResult.Window,
            TimeSpan.Zero,
            false,
            detail);
    }
}
