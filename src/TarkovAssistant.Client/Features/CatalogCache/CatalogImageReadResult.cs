namespace TarkovAssistant.Client.Features.CatalogCache;

/// <summary>
/// Contains cached image bytes or a user-safe explanation of an expected read failure.
/// </summary>
/// <param name="State">The image read outcome.</param>
/// <param name="Content">The image bytes when <paramref name="State"/> is available.</param>
/// <param name="Detail">An optional user-safe failure detail without local path information.</param>
internal readonly record struct CatalogImageReadResult(
    CatalogImageReadState State,
    ReadOnlyMemory<byte> Content,
    string? Detail);
