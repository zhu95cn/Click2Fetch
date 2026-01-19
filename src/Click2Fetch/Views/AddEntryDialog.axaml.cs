using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Click2Fetch.ViewModels;

namespace Click2Fetch.Views;

public partial class AddEntryDialog : Window
{
    public AddEntryDialog()
    {
        InitializeComponent();
        
        AddHandler(KeyDownEvent, OnPreviewKeyDown, RoutingStrategies.Tunnel);
    }

    private void OnPreviewKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var focusedElement = FocusManager?.GetFocusedElement();
            if (focusedElement == TitleInput)
            {
                NotesInput.Focus();
                e.Handled = true;
            }
            else if (focusedElement == NotesInput)
            {
                ConfirmButton.Focus();
                e.Handled = true;
            }
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is AddEntryDialogViewModel vm)
        {
            vm.CloseAction = Close;
        }
    }
}
