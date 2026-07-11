namespace TarkovAssistant.Client.Features.BackendConnection;

/// <summary>
/// Represents the result of a future local backend availability check.
/// </summary>
/// <param name="State">The observed connection state.</param>
/// <param name="CheckedAt">The UTC time at which the state was observed.</param>
/// <param name="Detail">An optional user-safe diagnostic message.</param>
internal readonly record struct BackendConnectionStatus(
    BackendConnectionState State,
    DateTimeOffset CheckedAt,
    string? Detail);
