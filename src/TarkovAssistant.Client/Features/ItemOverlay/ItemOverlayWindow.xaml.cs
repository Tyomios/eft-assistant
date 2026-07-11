using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TarkovAssistant.Client.Features.CursorTracking;

namespace TarkovAssistant.Client.Features.ItemOverlay;

/// <summary>
/// Renders the non-interactive item overlay on the WPF dispatcher thread.
/// </summary>
internal sealed partial class ItemOverlayWindow : Window
{
    /// <summary>
    /// Initializes the overlay window.
    /// </summary>
    public ItemOverlayWindow()
    {
        InitializeComponent();
    }

    internal void ShowContent(ItemOverlayContent content, ScreenPoint cursorPosition)
    {
        TitleText.Text = string.IsNullOrWhiteSpace(content.Title) ? "Item recognition" : content.Title;
        SummaryText.Text = content.Summary ?? string.Empty;
        DetailText.Text = content.Detail ?? string.Empty;
        DetailText.Visibility = string.IsNullOrWhiteSpace(content.Detail) ? Visibility.Collapsed : Visibility.Visible;
        ConfidenceText.Text = content.Confidence is { } confidence
            ? $"CONFIDENCE {confidence:P0}"
            : string.Empty;
        ConfidenceText.Visibility = content.Confidence.HasValue ? Visibility.Visible : Visibility.Collapsed;
        ToneIndicator.Background = CreateToneBrush(content.Tone);
        ItemImage.Source = DecodeImage(content.ImageContent);

        if (!IsVisible)
        {
            Show();
        }

        UpdateLayout();
        NativeOverlayWindowMethods.PositionNearCursor(new WindowInteropHelper(this).Handle, cursorPosition);
    }

    protected override void OnSourceInitialized(EventArgs eventArgs)
    {
        base.OnSourceInitialized(eventArgs);
        NativeOverlayWindowMethods.ConfigureClickThrough(new WindowInteropHelper(this).Handle);
    }

    private static BitmapImage? DecodeImage(ReadOnlyMemory<byte> content)
    {
        if (content.IsEmpty)
        {
            return null;
        }

        try
        {
            using var stream = new MemoryStream(content.ToArray(), writable: false);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }
        catch (IOException)
        {
            return null;
        }
        catch (NotSupportedException)
        {
            return null;
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    private static SolidColorBrush CreateToneBrush(OverlayTone tone)
    {
        var color = tone switch
        {
            OverlayTone.Positive => Color.FromRgb(112, 166, 120),
            OverlayTone.Warning => Color.FromRgb(200, 169, 106),
            OverlayTone.Error => Color.FromRgb(196, 105, 105),
            _ => Color.FromRgb(112, 146, 166),
        };

        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }
}
