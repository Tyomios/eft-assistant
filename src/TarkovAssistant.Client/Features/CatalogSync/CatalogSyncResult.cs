namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>
/// Reports a catalog synchronization result without exposing local file-system paths.
/// </summary>
/// <param name="State">The completed synchronization state.</param>
/// <param name="Version">The active or attempted catalog version when known.</param>
/// <param name="Detail">An optional user-safe diagnostic explanation.</param>
internal readonly record struct CatalogSyncResult(CatalogSyncState State, long? Version, string? Detail);
