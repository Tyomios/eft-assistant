namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Evaluates cursor stability using monotonic elapsed time and physical-pixel tolerance.
/// </summary>
internal sealed class CursorDwellEvaluator
{
    private readonly TimeSpan _dwellDuration;
    private readonly long _toleranceSquared;
    private ScreenPoint? _stablePosition;
    private TimeSpan? _stableSince;
    private bool _recognitionTriggered;

    /// <summary>
    /// Initializes the dwell evaluator.
    /// </summary>
    /// <param name="options">The configured dwell duration and positional tolerance.</param>
    /// <exception cref="ArgumentOutOfRangeException">A configured duration or tolerance is invalid.</exception>
    public CursorDwellEvaluator(CursorTrackingOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.DwellDuration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "The cursor dwell duration must be positive.");
        }

        if (options.PositionTolerancePixels < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "The cursor tolerance cannot be negative.");
        }

        _dwellDuration = options.DwellDuration;
        _toleranceSquared = (long)options.PositionTolerancePixels * options.PositionTolerancePixels;
    }

    /// <summary>
    /// Evaluates one cursor sample at a monotonic elapsed time.
    /// </summary>
    /// <param name="position">The current cursor position in physical pixels.</param>
    /// <param name="observedAt">The elapsed monotonic time at which the cursor was sampled.</param>
    /// <returns>The current dwell state and a one-time recognition trigger when it becomes satisfied.</returns>
    public CursorDwellEvaluation Evaluate(ScreenPoint position, TimeSpan observedAt)
    {
        if (_stablePosition is not { } stablePosition
            || _stableSince is not { } stableSince
            || stablePosition.DistanceSquaredTo(position) > _toleranceSquared
            || observedAt < stableSince)
        {
            _stablePosition = position;
            _stableSince = observedAt;
            _recognitionTriggered = false;
            return new CursorDwellEvaluation(CursorDwellState.Pending, TimeSpan.Zero, false);
        }

        var stableFor = observedAt - stableSince;
        if (stableFor < _dwellDuration)
        {
            return new CursorDwellEvaluation(CursorDwellState.Pending, stableFor, false);
        }

        var shouldTriggerRecognition = !_recognitionTriggered;
        _recognitionTriggered = true;
        return new CursorDwellEvaluation(CursorDwellState.Satisfied, stableFor, shouldTriggerRecognition);
    }

    /// <summary>
    /// Discards the current dwell candidate when the cursor leaves the game or tracking becomes unavailable.
    /// </summary>
    public void Reset()
    {
        _stablePosition = null;
        _stableSince = null;
        _recognitionTriggered = false;
    }
}
