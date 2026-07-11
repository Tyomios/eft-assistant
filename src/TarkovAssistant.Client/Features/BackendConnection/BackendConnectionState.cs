namespace TarkovAssistant.Client.Features.BackendConnection;

/// <summary>
/// Describes the client-visible state of the local backend connection.
/// </summary>
internal enum BackendConnectionState
{
    /// <summary>The connection has not been checked during this application session.</summary>
    NotChecked,

    /// <summary>The local backend answered the connection check.</summary>
    Available,

    /// <summary>The local backend did not answer the connection check.</summary>
    Unavailable,
}
