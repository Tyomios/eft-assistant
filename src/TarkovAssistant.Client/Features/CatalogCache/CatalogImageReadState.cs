namespace TarkovAssistant.Client.Features.CatalogCache;

/// <summary>
/// Describes the outcome of reading an image from the local catalog cache.
/// </summary>
internal enum CatalogImageReadState
{
    /// <summary>The image was read and passed the configured validation checks.</summary>
    Available,

    /// <summary>The catalog reference was empty, rooted, or escaped the cache boundary.</summary>
    InvalidReference,

    /// <summary>The referenced image is not present in the local cache.</summary>
    Missing,

    /// <summary>The image exceeds the configured per-file size limit.</summary>
    TooLarge,

    /// <summary>The image content does not match its catalog hash.</summary>
    HashMismatch,

    /// <summary>The image could not be read from the local file system.</summary>
    Unreadable,
}
