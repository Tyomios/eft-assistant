using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace TarkovAssistant.Client.Features.CatalogCache;

/// <summary>
/// Reads catalog images from the current user's local file-system cache.
/// </summary>
internal sealed class FileCatalogImageCache : ICatalogImageCache
{
    private readonly long _maximumImageBytes;
    private readonly string _rootPathWithSeparator;

    /// <summary>
    /// Initializes a local catalog image reader.
    /// </summary>
    /// <param name="options">The cache boundary and per-file safety limit.</param>
    /// <exception cref="ArgumentException">The cache root is not an absolute path.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The maximum image size is not positive.</exception>
    public FileCatalogImageCache(CatalogCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (!Path.IsPathFullyQualified(options.RootPath))
        {
            throw new ArgumentException("The catalog cache root must be an absolute path.", nameof(options));
        }

        if (options.MaximumImageBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "The maximum image size must be positive.");
        }

        _maximumImageBytes = options.MaximumImageBytes;
        _rootPathWithSeparator = Path.TrimEndingDirectorySeparator(Path.GetFullPath(options.RootPath))
            + Path.DirectorySeparatorChar;
    }

    /// <inheritdoc />
    public async ValueTask<CatalogImageReadResult> ReadAsync(
        CatalogImageReference reference,
        CancellationToken cancellationToken)
    {
        var fullPath = ResolvePath(reference.RelativePath);
        if (fullPath is null)
        {
            return Failure(CatalogImageReadState.InvalidReference, "The catalog image reference is invalid.");
        }

        try
        {
            var file = new FileInfo(fullPath);
            if (!file.Exists)
            {
                return Failure(CatalogImageReadState.Missing, "The catalog image is not available locally.");
            }

            if (file.Length > _maximumImageBytes)
            {
                return Failure(CatalogImageReadState.TooLarge, "The cached image exceeds the configured size limit.");
            }

            var content = await File.ReadAllBytesAsync(fullPath, cancellationToken).ConfigureAwait(false);
            if (content.LongLength > _maximumImageBytes)
            {
                return Failure(CatalogImageReadState.TooLarge, "The cached image exceeds the configured size limit.");
            }

            if (!HasExpectedHash(content, reference.ExpectedSha256))
            {
                return Failure(CatalogImageReadState.HashMismatch, "The cached image failed its integrity check.");
            }

            return new CatalogImageReadResult(CatalogImageReadState.Available, content, null);
        }
        catch (IOException)
        {
            return Failure(CatalogImageReadState.Unreadable, "The cached image could not be read.");
        }
        catch (UnauthorizedAccessException)
        {
            return Failure(CatalogImageReadState.Unreadable, "Access to the cached image was denied.");
        }
        catch (SecurityException)
        {
            return Failure(CatalogImageReadState.Unreadable, "Access to the cached image was denied.");
        }
    }

    private static CatalogImageReadResult Failure(CatalogImageReadState state, string detail)
    {
        return new CatalogImageReadResult(state, ReadOnlyMemory<byte>.Empty, detail);
    }

    private static bool HasExpectedHash(byte[] content, string? expectedSha256)
    {
        if (string.IsNullOrWhiteSpace(expectedSha256))
        {
            return true;
        }

        var actualHash = Convert.ToHexString(SHA256.HashData(content));
        return string.Equals(actualHash, expectedSha256, StringComparison.OrdinalIgnoreCase);
    }

    private string? ResolvePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || Path.IsPathFullyQualified(relativePath))
        {
            return null;
        }

        try
        {
            var fullPath = Path.GetFullPath(Path.Combine(_rootPathWithSeparator, relativePath));
            return fullPath.StartsWith(_rootPathWithSeparator, StringComparison.OrdinalIgnoreCase)
                ? fullPath
                : null;
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (NotSupportedException)
        {
            return null;
        }
        catch (PathTooLongException)
        {
            return null;
        }
    }
}
