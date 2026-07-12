namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>
/// Describes the externally observable result of a catalog synchronization attempt.
/// </summary>
internal enum CatalogSyncState
{
    /// <summary>A new complete catalog version was activated.</summary>
    Updated,

    /// <summary>The active catalog already matches the backend version.</summary>
    UpToDate,

    /// <summary>The backend has not published a catalog yet.</summary>
    CatalogUnavailable,

    /// <summary>The backend or an image source could not be reached.</summary>
    Unavailable,

    /// <summary>The received catalog was invalid or could not be stored safely.</summary>
    Failed,
}
