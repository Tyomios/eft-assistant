using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace TarkovAssistant.Client.Features.GameWindowDetection;

/// <summary>
/// Finds the capture-eligible Tarkov top-level window without accessing the game process memory.
/// </summary>
internal sealed class TarkovGameWindowDetector : ITarkovGameWindowDetector
{
    private readonly string _processName;

    /// <summary>
    /// Initializes a detector for the configured Tarkov executable name.
    /// </summary>
    /// <param name="options">The supported game process identity.</param>
    /// <exception cref="ArgumentException">The configured process name is empty or contains a path.</exception>
    public TarkovGameWindowDetector(TarkovGameWindowDetectionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.ProcessName)
            || options.ProcessName.IndexOfAny([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar]) >= 0)
        {
            throw new ArgumentException("The Tarkov process name must not be empty or include a path.", nameof(options));
        }

        _processName = Path.GetFileNameWithoutExtension(options.ProcessName);
    }

    /// <inheritdoc />
    public GameWindowDetectionResult Detect()
    {
        try
        {
            var foregroundWindow = NativeTarkovWindowMethods.GetForegroundWindow();
            var candidates = new List<TarkovGameWindow>();

            foreach (var windowHandle in NativeTarkovWindowMethods.EnumerateTopLevelWindows())
            {
                if (!NativeTarkovWindowMethods.IsVisible(windowHandle))
                {
                    continue;
                }

                var processId = NativeTarkovWindowMethods.GetProcessId(windowHandle);
                if (!IsTarkovProcess(processId))
                {
                    continue;
                }

                candidates.Add(
                    new TarkovGameWindow(
                        windowHandle,
                        processId,
                        NativeTarkovWindowMethods.GetTitle(windowHandle),
                        NativeTarkovWindowMethods.GetBounds(windowHandle),
                        windowHandle == foregroundWindow,
                        NativeTarkovWindowMethods.IsMinimized(windowHandle)));
            }

            if (candidates.Count == 0)
            {
                return new GameWindowDetectionResult(GameWindowDetectionState.NotFound, null, null);
            }

            var selectedWindow = candidates
                .OrderByDescending(candidate => candidate.IsForeground)
                .ThenByDescending(candidate => (long)candidate.Bounds.Width * candidate.Bounds.Height)
                .First();
            var state = selectedWindow.IsMinimized
                ? GameWindowDetectionState.Minimized
                : GameWindowDetectionState.Found;
            return new GameWindowDetectionResult(state, selectedWindow, null);
        }
        catch (Win32Exception)
        {
            return Failed();
        }
        catch (InvalidOperationException)
        {
            return Failed();
        }
    }

    private static GameWindowDetectionResult Failed()
    {
        return new GameWindowDetectionResult(
            GameWindowDetectionState.Failed,
            null,
            "The Tarkov game window could not be inspected through Windows.");
    }

    private bool IsTarkovProcess(int processId)
    {
        try
        {
            using var process = Process.GetProcessById(processId);
            return string.Equals(process.ProcessName, _processName, StringComparison.OrdinalIgnoreCase);
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
        catch (Win32Exception)
        {
            return false;
        }
    }
}
