using Avalonia.Controls;
using Click2Fetch.ViewModels;

namespace Click2Fetch.Views;

public partial class SettingsDialog : Window
{
    public SettingsDialog()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextSet;
    }

    private void OnDataContextSet(object? sender, EventArgs e)
    {
        if (DataContext is SettingsDialogViewModel vm)
        {
            vm.CloseAction = Close;
        }
    }
}
