namespace TarkovAssistant.Client.Features.CatalogCache;

/// <summary>
/// Identifies one image beneath the configured cache root.
/// </summary>
/// <param name="RelativePath">The platform-relative path stored in the local catalog.</param>
/// <param name="ExpectedSha256">The optional expected SHA-256 hash encoded as hexadecimal text.</param>
internal readonly record struct CatalogImageReference(string RelativePath, string? ExpectedSha256);
