using Avalonia.Media;
using Click2Fetch.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Styling;

namespace Click2Fetch.ViewModels;

public partial class EditAttributeDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _key = string.Empty;

    [ObservableProperty]
    private string _value = string.Empty;

    [ObservableProperty]
    private bool _isEditMode;

    [ObservableProperty]
    private bool _isConfirmed;

    [ObservableProperty]
    private bool _isDeleted;

    [ObservableProperty]
    private bool _isLightTheme;

    public ThemeVariant ActualThemeVariant => IsLightTheme ? ThemeVariant.Light : ThemeVariant.Dark;

    [ObservableProperty]
    private bool _isEnglish;

    // Localized strings
    public string L_EditTitle => IsEnglish ? "Edit Attribute" : "编辑属性";
    public string L_AddTitle => IsEnglish ? "Add Attribute" : "添加属性";
    public string L_KeyLabel => IsEnglish ? "Attribute Name" : "属性名称";
    public string L_KeyPlaceholder => IsEnglish ? "e.g. url, username, password..." : "例如: url, 用户名, 密码...";
    public string L_ValueLabel => IsEnglish ? "Value" : "值";
    public string L_ValuePlaceholder => IsEnglish ? "Enter value..." : "输入属性值...";
    public string L_Browse => IsEnglish ? "Browse" : "浏览";
    public string L_Delete => IsEnglish ? "Delete" : "删除";
    public string L_Cancel => IsEnglish ? "Cancel" : "取消";
    public string L_Save => IsEnglish ? "Save" : "保存";

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
    public IBrush DialogDeleteButtonBg => Brush.Parse(IsLightTheme ? "#FEE2E2" : "#2D1515");
    public IBrush DialogDeleteButtonBorder => Brush.Parse(IsLightTheme ? "#FECACA" : "#4A2020");

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
        OnPropertyChanged(nameof(DialogDeleteButtonBg));
        OnPropertyChanged(nameof(DialogDeleteButtonBorder));
        OnPropertyChanged(nameof(ActualThemeVariant));
    }

    partial void OnIsEnglishChanged(bool value)
    {
        OnPropertyChanged(nameof(DialogTitle));
        OnPropertyChanged(nameof(L_EditTitle));
        OnPropertyChanged(nameof(L_AddTitle));
        OnPropertyChanged(nameof(L_KeyLabel));
        OnPropertyChanged(nameof(L_KeyPlaceholder));
        OnPropertyChanged(nameof(L_ValueLabel));
        OnPropertyChanged(nameof(L_ValuePlaceholder));
        OnPropertyChanged(nameof(L_Browse));
        OnPropertyChanged(nameof(L_Delete));
        OnPropertyChanged(nameof(L_Cancel));
        OnPropertyChanged(nameof(L_Save));
    }

    public string DialogTitle => IsEditMode 
        ? (IsEnglish ? "Edit Attribute" : "编辑属性") 
        : (IsEnglish ? "Add Attribute" : "添加属性");

    public Action? CloseAction { get; set; }
    
    /// <summary>
    /// Callback to browse for file, set by View
    /// </summary>
    public Func<Task<string?>>? BrowseFileAction { get; set; }

    /// <summary>
    /// Show browse button when key suggests application
    /// </summary>
    public bool ShowBrowseButton => Key.ToLowerInvariant().Contains("app") || 
                                    Key.ToLowerInvariant().Contains("程序") ||
                                    Key.ToLowerInvariant().Contains("应用") ||
                                    IsApplicationPath(Value);

    partial void OnKeyChanged(string value)
    {
        OnPropertyChanged(nameof(ShowBrowseButton));
    }

    partial void OnValueChanged(string value)
    {
        OnPropertyChanged(nameof(ShowBrowseButton));
    }

    private bool IsApplicationPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return false;
        var lower = path.ToLowerInvariant();
        return lower.EndsWith(".exe") || lower.EndsWith(".app") || 
               lower.Contains("program files") || lower.Contains("/applications/");
    }

    /// <summary>
    /// Detect attribute type based on key name and value
    /// </summary>
    public AttributeType DetectedType
    {
        get
        {
            var keyLower = Key.ToLowerInvariant();

            // Check if it's an application path
            if (IsApplicationPath(Value) || keyLower.Contains("app") || 
                keyLower.Contains("程序") || keyLower.Contains("应用"))
            {
                return AttributeType.Application;
            }

            // Check by value (URL detection)
            if (Uri.TryCreate(Value, UriKind.Absolute, out var uri) &&
                (uri.Scheme == "http" || uri.Scheme == "https"))
            {
                return AttributeType.Url;
            }

            // Check by key name
            if (keyLower.Contains("password") || keyLower.Contains("pwd") || keyLower.Contains("secret"))
                return AttributeType.Password;
            if (keyLower.Contains("user") || keyLower.Contains("login") || keyLower.Contains("account"))
                return AttributeType.Username;
            if (keyLower.Contains("api") || keyLower.Contains("key") || keyLower.Contains("token"))
                return AttributeType.ApiKey;
            if (keyLower.Contains("email") || keyLower.Contains("mail"))
                return AttributeType.Email;
            if (keyLower.Contains("port"))
                return AttributeType.Port;
            if (keyLower.Contains("host") || keyLower.Contains("server") || keyLower.Contains("ip"))
                return AttributeType.Host;
            if (keyLower.Contains("url") || keyLower.Contains("link") || keyLower.Contains("address"))
                return AttributeType.Url;

            return AttributeType.Text;
        }
    }

    [RelayCommand]
    private async Task BrowseFileAsync()
    {
        if (BrowseFileAction != null)
        {
            var path = await BrowseFileAction();
            if (!string.IsNullOrEmpty(path))
            {
                Value = path;
            }
        }
    }

    [RelayCommand]
    private void Confirm()
    {
        if (string.IsNullOrWhiteSpace(Key))
        {
            Key = "Attribute";
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

    [RelayCommand]
    private void Delete()
    {
        IsDeleted = true;
        CloseAction?.Invoke();
    }
}
