using OpenCvSharp;
using System.IO;
using System.Runtime.InteropServices;
using TarkovAssistant.Client.Features.CatalogCache;
using TarkovAssistant.Client.Features.ScreenCapture;

namespace TarkovAssistant.Client.Features.ItemRecognition;

/// <summary>
/// Identifies a cropped stash icon with normalized grayscale perceptual hashes and pixel-template similarity.
/// </summary>
internal sealed class DeterministicHoveredItemRecognizer : IHoveredItemRecognizer
{
    private const int NormalizedIconSize = 32;
    private readonly ICatalogGridImageIndex _catalogIndex;
    private readonly ICatalogImageCache _imageCache;
    private readonly double _minimumConfidence;
    private readonly int _maximumAlternatives;

    /// <summary>
    /// Initializes the local-only deterministic recognizer.
    /// </summary>
    /// <param name="catalogIndex">The local SQLite grid-image index.</param>
    /// <param name="imageCache">The bounded local image cache reader.</param>
    /// <param name="options">The required confidence threshold and alternative limit.</param>
    /// <exception cref="ArgumentOutOfRangeException">A recognition setting is invalid.</exception>
    public DeterministicHoveredItemRecognizer(
        ICatalogGridImageIndex catalogIndex,
        ICatalogImageCache imageCache,
        HoverRecognitionOptions options)
    {
        ArgumentNullException.ThrowIfNull(catalogIndex);
        ArgumentNullException.ThrowIfNull(imageCache);
        ArgumentNullException.ThrowIfNull(options);
        if (options.MinimumStrongConfidence is < 0 or > 1 || options.MaximumAlternatives <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "The recognition settings are invalid.");
        }

        _catalogIndex = catalogIndex;
        _imageCache = imageCache;
        _minimumConfidence = options.MinimumStrongConfidence;
        _maximumAlternatives = options.MaximumAlternatives;
    }

    /// <inheritdoc />
    public async ValueTask<HoverRecognitionResult> RecognizeAsync(
        HoverRecognitionRequest request,
        CancellationToken cancellationToken)
    {
        using var capturedRegion = NormalizeCapturedRegion(request.CapturedRegion);

        var catalogImages = await _catalogIndex.ReadAsync(cancellationToken).ConfigureAwait(false);
        if (catalogImages.Count == 0)
        {
            return new HoverRecognitionResult(HoverRecognitionState.CacheUnavailable, null, 0, [], "No synchronized grid images are available locally.");
        }

        var candidates = new List<RecognitionCandidate>();
        foreach (var catalogImage in catalogImages)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var image = await _imageCache.ReadAsync(new CatalogImageReference(catalogImage.RelativePath, null), cancellationToken).ConfigureAwait(false);
            if (image.State != CatalogImageReadState.Available)
            {
                continue;
            }

            using var referenceImage = DecodeToGrayscale(image.Content.Span);
            if (referenceImage is null)
            {
                continue;
            }

            var expectedWidth = checked(catalogImage.Width * request.ItemCell.CellWidth);
            var expectedHeight = checked(catalogImage.Height * request.ItemCell.CellHeight);
            if (expectedWidth > capturedRegion.Width || expectedHeight > capturedRegion.Height)
            {
                continue;
            }

            using var scaledReference = new Mat();
            Cv2.Resize(referenceImage, scaledReference, new Size(expectedWidth, expectedHeight), 0, 0, InterpolationFlags.Area);
            Cv2.EqualizeHist(scaledReference, scaledReference);
            using var similarityMap = new Mat();
            Cv2.MatchTemplate(capturedRegion, scaledReference, similarityMap, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(similarityMap, out _, out var templateSimilarity, out _, out var bestLocation);
            using var matchedRegion = new Mat(capturedRegion, new Rect(bestLocation.X, bestLocation.Y, expectedWidth, expectedHeight));
            var hashSimilarity = 1d - (HammingDistance(CalculateAverageHash(matchedRegion), CalculateAverageHash(scaledReference)) / 64d);
            candidates.Add(new RecognitionCandidate(catalogImage.ItemId, Math.Clamp((hashSimilarity * 0.65) + (templateSimilarity * 0.35), 0, 1)));
        }

        var alternatives = candidates
            .OrderByDescending(candidate => candidate.Confidence)
            .ThenBy(candidate => candidate.ItemId)
            .Take(_maximumAlternatives)
            .ToArray();
        if (alternatives.Length == 0)
        {
            return new HoverRecognitionResult(HoverRecognitionState.CacheUnavailable, null, 0, alternatives, "Local grid images could not be read.");
        }

        var best = alternatives[0];
        if (best.Confidence < _minimumConfidence)
        {
            return new HoverRecognitionResult(HoverRecognitionState.NotRecognized, null, best.Confidence, alternatives, "The best local image match is below the configured confidence threshold.");
        }

        return new HoverRecognitionResult(HoverRecognitionState.Recognized, best.ItemId, best.Confidence, alternatives, null);
    }

    private static Mat NormalizeCapturedRegion(CapturedInventoryRegion region)
    {
        using var pixels = new Mat(region.Height, region.Width, MatType.CV_8UC4);
        var rowBytes = checked(region.Width * 4);
        for (var row = 0; row < region.Height; row++)
        {
            Marshal.Copy(
                region.Pixels.Slice(checked(row * region.Stride), rowBytes).ToArray(),
                0,
                IntPtr.Add(pixels.Data, checked(row * (int)pixels.Step())),
                rowBytes);
        }
        return ConvertToGrayscale(pixels);
    }

    private static Mat? DecodeToGrayscale(ReadOnlySpan<byte> encodedImage)
    {
        var source = Cv2.ImDecode(encodedImage.ToArray(), ImreadModes.Unchanged);
        if (source.Empty())
        {
            source.Dispose();
            return null;
        }

        using (source)
        {
            return ConvertToGrayscale(source);
        }
    }

    private static Mat ConvertToGrayscale(Mat source)
    {
        return source.Channels() switch
        {
            4 => ConvertBgraToGrayscale(source),
            3 => ConvertBgrToGrayscale(source),
            1 => EqualizeGrayscale(source),
            _ => throw new InvalidDataException("The local grid image has an unsupported pixel format.")
        };
    }

    private static Mat ConvertBgraToGrayscale(Mat source)
    {
        using var bgr = new Mat();
        Cv2.CvtColor(source, bgr, ColorConversionCodes.BGRA2BGR);
        return ConvertBgrToGrayscale(bgr);
    }

    private static Mat ConvertBgrToGrayscale(Mat source)
    {
        using var grayscale = new Mat();
        Cv2.CvtColor(source, grayscale, ColorConversionCodes.BGR2GRAY);
        return EqualizeGrayscale(grayscale);
    }

    private static Mat EqualizeGrayscale(Mat source)
    {
        var equalized = new Mat();
        Cv2.EqualizeHist(source, equalized);
        return equalized;
    }

    private static ulong CalculateAverageHash(Mat normalizedIcon)
    {
        using var thumbnail = new Mat();
        Cv2.Resize(normalizedIcon, thumbnail, new Size(8, 8), 0, 0, InterpolationFlags.Area);
        var total = 0;
        for (var row = 0; row < thumbnail.Rows; row++)
        {
            for (var column = 0; column < thumbnail.Cols; column++)
            {
                total += thumbnail.At<byte>(row, column);
            }
        }

        var average = total / 64;
        ulong hash = 0;
        for (var row = 0; row < thumbnail.Rows; row++)
        {
            for (var column = 0; column < thumbnail.Cols; column++)
            {
                hash = (hash << 1) | (thumbnail.At<byte>(row, column) >= average ? 1UL : 0UL);
            }
        }

        return hash;
    }

    private static int HammingDistance(ulong left, ulong right)
    {
        return System.Numerics.BitOperations.PopCount(left ^ right);
    }

    private static double CalculateTemplateSimilarity(Mat capturedIcon, Mat referenceIcon)
    {
        using var difference = new Mat();
        Cv2.Absdiff(capturedIcon, referenceIcon, difference);
        return 1d - (Cv2.Mean(difference).Val0 / byte.MaxValue);
    }
}
