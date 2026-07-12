using System.IO;

namespace TarkovAssistant.Client.Features.CatalogSync;

/// <summary>
/// Defines the backend and local storage boundaries for catalog synchronization.
/// </summary>
internal sealed class CatalogSyncOptions
{
    private const long DefaultMaximumImageBytes = 8 * 1024 * 1024;
    private const int DefaultMaximumConcurrentImageDownloads = 16;

    /// <summary>Gets the default root under the current user's local application data directory.</summary>
    public static string DefaultRootPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TarkovAssistant",
        "Catalog");

    /// <summary>Gets the default local backend address exposed by Docker Compose.</summary>
    public static Uri DefaultBaseAddress { get; } = new("http://localhost:5080/");

    /// <summary>Gets the absolute directory containing versioned catalog folders.</summary>
    public string RootPath { get; init; } = DefaultRootPath;

    /// <summary>Gets the base address for catalog API requests.</summary>
    public Uri BaseAddress { get; init; } = DefaultBaseAddress;

    /// <summary>Gets the maximum accepted image download size in bytes.</summary>
    public long MaximumImageBytes { get; init; } = DefaultMaximumImageBytes;

    /// <summary>Gets the maximum image-download concurrency supported by the current local backend and network setup.</summary>
    public int MaximumConcurrentImageDownloads { get; init; } = DefaultMaximumConcurrentImageDownloads;
}
