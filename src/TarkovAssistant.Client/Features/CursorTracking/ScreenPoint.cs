namespace TarkovAssistant.Client.Features.CursorTracking;

/// <summary>
/// Identifies a physical-pixel position in virtual-screen coordinates.
/// </summary>
/// <param name="X">The horizontal coordinate in physical pixels.</param>
/// <param name="Y">The vertical coordinate in physical pixels.</param>
internal readonly record struct ScreenPoint(int X, int Y)
{
    /// <summary>
    /// Calculates the squared physical-pixel distance to another screen point.
    /// </summary>
    /// <param name="other">The comparison point in the same virtual-screen coordinate system.</param>
    /// <returns>The squared distance in physical pixels.</returns>
    public long DistanceSquaredTo(ScreenPoint other)
    {
        var horizontalDistance = (long)X - other.X;
        var verticalDistance = (long)Y - other.Y;
        return (horizontalDistance * horizontalDistance) + (verticalDistance * verticalDistance);
    }
}
