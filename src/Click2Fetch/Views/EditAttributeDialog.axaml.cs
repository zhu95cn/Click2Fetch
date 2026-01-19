using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Click2Fetch.ViewModels;

namespace Click2Fetch.Views;

public partial class EditAttributeDialog : Window
{
    public EditAttributeDialog()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is EditAttributeDialogViewModel vm)
        {
            vm.CloseAction = Close;
            vm.BrowseFileAction = BrowseForFileAsync;
        }
    }

    private async Task<string?> BrowseForFileAsync()
    {
        var options = new FilePickerOpenOptions
        {
            Title = "选择程序",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("应用程序")
                {
                    Patterns = OperatingSystem.IsWindows() 
                        ? new[] { "*.exe" } 
                        : OperatingSystem.IsMacOS() 
                            ? new[] { "*.app" } 
                            : new[] { "*" }
                },
                FilePickerFileTypes.All
            }
        };

        var result = await StorageProvider.OpenFilePickerAsync(options);
        if (result.Count > 0)
        {
            return result[0].Path.LocalPath;
        }
        return null;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (sender == KeyInput)
            {
                ValueInput.Focus();
                e.Handled = true;
            }
            else if (sender == ValueInput)
            {
                ConfirmButton.Focus();
                e.Handled = true;
            }
        }
    }

    private void OnConfirmKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is EditAttributeDialogViewModel vm)
            {
                vm.ConfirmCommand.Execute(null);
            }
            e.Handled = true;
        }
    }
}
