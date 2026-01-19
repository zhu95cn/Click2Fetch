using Avalonia;
using Avalonia.Input.Platform;

namespace Click2Fetch.Services;

/// <summary>
/// Interface for clipboard operations with auto-clear functionality
/// </summary>
public interface IClipboardService
{
    Task CopyToClipboardAsync(string value, bool autoClear = true);
    Task ClearClipboardAsync();
    void CancelPendingClear();
}

/// <summary>
/// Clipboard service with 30-second auto-clear
/// </summary>
public class ClipboardService : IClipboardService
{
    private readonly int _clearDelaySeconds;
    private CancellationTokenSource? _clearCts;
    private string? _lastCopiedValue;

    public ClipboardService(int clearDelaySeconds = 30)
    {
        _clearDelaySeconds = clearDelaySeconds;
    }

    public async Task CopyToClipboardAsync(string value, bool autoClear = true)
    {
        var clipboard = GetClipboard();
        if (clipboard == null) return;

        CancelPendingClear();
        
        _lastCopiedValue = value;
        await clipboard.SetTextAsync(value);

        if (autoClear)
        {
            _clearCts = new CancellationTokenSource();
            _ = ClearAfterDelayAsync(_clearCts.Token);
        }
    }

    public async Task ClearClipboardAsync()
    {
        var clipboard = GetClipboard();
        if (clipboard == null) return;

        try
        {
            var currentText = await clipboard.GetTextAsync();
            if (currentText == _lastCopiedValue)
            {
                await clipboard.ClearAsync();
            }
        }
        catch
        {
            // Clipboard may not be accessible
        }
        
        _lastCopiedValue = null;
    }

    public void CancelPendingClear()
    {
        _clearCts?.Cancel();
        _clearCts?.Dispose();
        _clearCts = null;
    }

    private async Task ClearAfterDelayAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(_clearDelaySeconds), token);
            await ClearClipboardAsync();
        }
        catch (OperationCanceledException)
        {
            // Cancelled, ignore
        }
    }

    private static IClipboard? GetClipboard()
    {
        return Application.Current?.GetTopLevel()?.Clipboard;
    }
}

public static class ApplicationExtensions
{
    public static Avalonia.Controls.TopLevel? GetTopLevel(this Application? app)
    {
        return app?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
    }
}
