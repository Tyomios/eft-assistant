using System.IO;

namespace TarkovAssistant.Client.Features.CatalogCache;

/// <summary>
/// Defines the local catalog image cache boundary and per-file safety limit.
/// </summary>
internal sealed class CatalogCacheOptions
{
    private const long DefaultMaximumImageBytes = 8 * 1024 * 1024;

    /// <summary>
    /// Gets the default cache root in the current Windows user's local application data.
    /// </summary>
    public static string DefaultRootPath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TarkovAssistant",
        "Catalog");

    /// <summary>
    /// Gets the absolute directory containing versioned catalog files.
    /// </summary>
    public string RootPath { get; init; } = DefaultRootPath;

    /// <summary>
    /// Gets the maximum accepted size of one cached image, in bytes.
    /// </summary>
    public long MaximumImageBytes { get; init; } = DefaultMaximumImageBytes;
}
