using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Click2Fetch.Services;
using Click2Fetch.ViewModels;
using Click2Fetch.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;

namespace Click2Fetch;

public partial class App : Application
{
    // Services
    private static IEncryptionService? _encryptionService;
    private static IDatabaseService? _databaseService;
    private static IClipboardService? _clipboardService;
    private static MainWindow? _mainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        // Initialize services
        _encryptionService = new EncryptionService();
        _databaseService = new DatabaseService(_encryptionService);
        _clipboardService = new ClipboardService(30);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = new MainWindowViewModel(
                _databaseService!,
                _encryptionService!,
                _clipboardService!);

            var mainWindow = new MainWindow
            {
                DataContext = viewModel
            };
            _mainWindow = mainWindow;

            // Intercept close to minimize to tray
            mainWindow.Closing += (s, e) =>
            {
                e.Cancel = true;
                mainWindow.Hide();
            };

            // Set up dialog callback
            viewModel.ShowAddEntryDialog = async (dialogVm) =>
            {
                dialogVm.IsLightTheme = viewModel.IsLightTheme;
                var dialog = new AddEntryDialog
                {
                    DataContext = dialogVm
                };
                await dialog.ShowDialog(mainWindow);
            };

            viewModel.ShowEditAttributeDialog = async (dialogVm) =>
            {
                dialogVm.IsLightTheme = viewModel.IsLightTheme;
                var dialog = new EditAttributeDialog
                {
                    DataContext = dialogVm
                };
                await dialog.ShowDialog(mainWindow);
            };

            viewModel.ShowSettingsDialog = async (dialogVm) =>
            {
                var dialog = new SettingsDialog
                {
                    DataContext = dialogVm
                };
                dialogVm.CloseAction = () => dialog.Close();
                await dialog.ShowDialog(mainWindow);
            };

            viewModel.ShowConfirmDialog = async (title, message) =>
            {
                var box = MessageBoxManager.GetMessageBoxStandard(
                    title,
                    message,
                    ButtonEnum.YesNo,
                    Icon.Question);
                var result = await box.ShowWindowDialogAsync(mainWindow);
                return result == ButtonResult.Yes;
            };

            desktop.MainWindow = mainWindow;

            // Initialize database and check first-time setup
            await viewModel.InitializeAsync();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public void ShowWindow_Click(object? sender, EventArgs e)
    {
        _mainWindow?.Show();
        _mainWindow?.Activate();
    }

    public void TrayIcon_Clicked(object? sender, EventArgs e)
    {
        _mainWindow?.Show();
        _mainWindow?.Activate();
    }

    public void Exit_Click(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}
