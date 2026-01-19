using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Styling;

namespace Click2Fetch.ViewModels;

public partial class AddEntryDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _notes = string.Empty;

    [ObservableProperty]
    private bool _isConfirmed;

    [ObservableProperty]
    private bool _isLightTheme;

    public ThemeVariant ActualThemeVariant => IsLightTheme ? ThemeVariant.Light : ThemeVariant.Dark;

    [ObservableProperty]
    private bool _isEnglish;

    // Localized strings
    public string L_Title => IsEnglish ? "Add New Credential" : "添加新凭证";
    public string L_TitleLabel => IsEnglish ? "Title" : "标题";
    public string L_TitlePlaceholder => IsEnglish ? "Credential name..." : "凭证名称...";
    public string L_NotesLabel => IsEnglish ? "Description (optional)" : "描述 (可选)";
    public string L_NotesPlaceholder => IsEnglish ? "Add description..." : "添加描述...";
    public string L_Cancel => IsEnglish ? "Cancel" : "取消";
    public string L_Create => IsEnglish ? "Create" : "创建";

    // Theme colors for dialog
    public IBrush DialogBg => Brush.Parse(IsLightTheme ? "#F5F5F7" : "#0D0D0F");
    public IBrush DialogCardBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#1A1A1F");
    public IBrush DialogBorder => Brush.Parse(IsLightTheme ? "#E5E7EB" : "#2A2A35");
    public IBrush DialogTextPrimary => Brush.Parse(IsLightTheme ? "#1F2937" : "#FFFFFF");
    public IBrush DialogTextSecondary => Brush.Parse(IsLightTheme ? "#6B7280" : "#6B7280");
    public IBrush DialogInputBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#1F1F28");
    public IBrush DialogCaretBrush => Brush.Parse(IsLightTheme ? "#1F2937" : "#F9FAFB");
    public IBrush DialogButtonBg => Brush.Parse(IsLightTheme ? "#3B82F6" : "#7C3AED");
    public IBrush DialogSecondaryButtonBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#1F1F28");
    public IBrush DialogSecondaryButtonBorder => Brush.Parse(IsLightTheme ? "#D1D5DB" : "#2A2A35");

    partial void OnIsLightThemeChanged(bool value)
    {
        OnPropertyChanged(nameof(DialogBg));
        OnPropertyChanged(nameof(DialogCardBg));
        OnPropertyChanged(nameof(DialogBorder));
        OnPropertyChanged(nameof(DialogTextPrimary));
        OnPropertyChanged(nameof(DialogTextSecondary));
        OnPropertyChanged(nameof(DialogInputBg));
        OnPropertyChanged(nameof(DialogCaretBrush));
        OnPropertyChanged(nameof(DialogButtonBg));
        OnPropertyChanged(nameof(DialogSecondaryButtonBg));
        OnPropertyChanged(nameof(DialogSecondaryButtonBorder));
        OnPropertyChanged(nameof(ActualThemeVariant));
    }

    partial void OnIsEnglishChanged(bool value)
    {
        OnPropertyChanged(nameof(L_Title));
        OnPropertyChanged(nameof(L_TitleLabel));
        OnPropertyChanged(nameof(L_TitlePlaceholder));
        OnPropertyChanged(nameof(L_NotesLabel));
        OnPropertyChanged(nameof(L_NotesPlaceholder));
        OnPropertyChanged(nameof(L_Cancel));
        OnPropertyChanged(nameof(L_Create));
    }

    public Action? CloseAction { get; set; }

    [RelayCommand]
    private void Confirm()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            Title = "New Entry";
        }
        IsConfirmed = true;
        CloseAction?.Invoke();
    }

    [RelayCommand]
    private void Cancel()
    {
        IsConfirmed = false;
        CloseAction?.Invoke();
    }
}
