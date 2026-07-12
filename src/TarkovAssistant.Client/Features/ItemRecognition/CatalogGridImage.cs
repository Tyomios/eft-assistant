namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Identifies one catalog item and its cache-relative grid image.
/// </summary>
/// <param name="ItemId">The stable local catalog item identifier.</param>
/// <param name="RelativePath">The grid image path beneath the configured local cache root.</param>
/// <param name="Width">The item's catalog width in stash cells.</param>
/// <param name="Height">The item's catalog height in stash cells.</param>
internal readonly record struct CatalogGridImage(Guid ItemId, string RelativePath, int Width, int Height);
