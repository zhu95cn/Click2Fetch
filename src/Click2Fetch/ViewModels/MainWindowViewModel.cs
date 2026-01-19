using System.Collections.ObjectModel;
using Avalonia.Media;
using Click2Fetch.Models;
using Click2Fetch.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Styling;

namespace Click2Fetch.ViewModels;

/// <summary>
/// Main window view model handling the list and selection
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IDatabaseService _databaseService;
    private readonly IEncryptionService _encryptionService;
    private readonly IClipboardService _clipboardService;

    [ObservableProperty]
    private ObservableCollection<ItemViewModel> _items = new();

    [ObservableProperty]
    private ItemViewModel? _selectedItem;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private bool _isLocked = true;

    [ObservableProperty]
    private bool _isFirstTimeSetup;

    [ObservableProperty]
    private string _masterPassword = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _backgroundColor = "#252526";

    [ObservableProperty]
    private bool _isEnglish = false;

    [ObservableProperty]
    private bool _isLightTheme = false;

    public ThemeVariant ActualThemeVariant => IsLightTheme ? ThemeVariant.Light : ThemeVariant.Dark;

    // Theme color properties
    public IBrush ThemeBg => Brush.Parse(IsLightTheme ? "#F5F5F7" : "#0A0A0C");
    public IBrush ThemeSidebarBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#0D0D10");
    public IBrush ThemeCardBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#16161A");
    public IBrush ThemeBorder => Brush.Parse(IsLightTheme ? "#E5E7EB" : "#252530");
    public IBrush ThemeTextPrimary => Brush.Parse(IsLightTheme ? "#1F2937" : "#F9FAFB");
    public IBrush ThemeTextSecondary => Brush.Parse(IsLightTheme ? "#6B7280" : "#9CA3AF");
    public IBrush ThemeAccent => Brush.Parse(IsLightTheme ? "#3B82F6" : "#8B5CF6");
    public IBrush ThemeInputBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#1C1C22");
    public IBrush ThemeCaretBrush => Brush.Parse(IsLightTheme ? "#1F2937" : "#F9FAFB");
    public IBrush ThemeSelectedItemBg => Brush.Parse(IsLightTheme ? "#EBF5FF" : "#1C1C24");
    public IBrush ThemeHoverBg => Brush.Parse(IsLightTheme ? "#F3F4F6" : "#2A2A35");
    public IBrush ThemeLangToggleBg => Brush.Parse(IsLightTheme ? "#F3F4F6" : "#16161A");
    public IBrush ThemeAccentGradient1 => Brush.Parse(IsLightTheme ? "#2563EB" : "#5B21B6");
    public IBrush ThemeAccentGradient2 => Brush.Parse(IsLightTheme ? "#3B82F6" : "#7C3AED");
    
    // Button theme colors
    public IBrush ThemePrimaryButtonBg => Brush.Parse(IsLightTheme ? "#3B82F6" : "#8B5CF6");
    public IBrush ThemePrimaryButtonHoverBg => Brush.Parse(IsLightTheme ? "#2563EB" : "#A78BFA");
    public IBrush ThemeSecondaryButtonBg => Brush.Parse(IsLightTheme ? "#FFFFFF" : "#1C1C22");
    public IBrush ThemeSecondaryButtonHoverBg => Brush.Parse(IsLightTheme ? "#E5E7EB" : "#252530");
    public IBrush ThemeSecondaryButtonBorder => Brush.Parse(IsLightTheme ? "#9CA3AF" : "#2D2D38");
    public IBrush ThemeSecondaryButtonText => Brush.Parse(IsLightTheme ? "#374151" : "#9CA3AF");

    partial void OnIsLightThemeChanged(bool value)
    {
        System.Diagnostics.Debug.WriteLine($"Theme changed to: {(value ? "Light" : "Dark")}");
        Console.WriteLine($"Theme changed to: {(value ? "Light" : "Dark")}");
        OnPropertyChanged(nameof(ThemeBg));
        OnPropertyChanged(nameof(ThemeSidebarBg));
        OnPropertyChanged(nameof(ThemeCardBg));
        OnPropertyChanged(nameof(ThemeBorder));
        OnPropertyChanged(nameof(ThemeTextPrimary));
        OnPropertyChanged(nameof(ThemeTextSecondary));
        OnPropertyChanged(nameof(ThemeAccent));
        OnPropertyChanged(nameof(ThemeInputBg));
        OnPropertyChanged(nameof(ThemeCaretBrush));
        OnPropertyChanged(nameof(ThemeSelectedItemBg));
        OnPropertyChanged(nameof(ThemeHoverBg));
        OnPropertyChanged(nameof(ThemeLangToggleBg));
        OnPropertyChanged(nameof(ThemeAccentGradient1));
        OnPropertyChanged(nameof(ThemeAccentGradient2));
        // Button theme colors
        OnPropertyChanged(nameof(ThemePrimaryButtonBg));
        OnPropertyChanged(nameof(ThemePrimaryButtonHoverBg));
        OnPropertyChanged(nameof(ThemeSecondaryButtonBg));
        OnPropertyChanged(nameof(ThemeSecondaryButtonHoverBg));
        OnPropertyChanged(nameof(ThemeSecondaryButtonBorder));
        OnPropertyChanged(nameof(ThemeSecondaryButtonText));
        OnPropertyChanged(nameof(ActualThemeVariant));
        OnPropertyChanged(nameof(L_ThemeShort));
    }

    // Localized strings
    public string L_AppTitle => IsEnglish ? "Credential Manager" : "凭证管理器";
    public string L_AppSubtitle => IsEnglish ? "Securely manage your login info" : "安全管理您的登录信息";
    public string L_SearchPlaceholder => IsEnglish ? "Search..." : "搜索凭证...";
    public string L_MyCredentials => IsEnglish ? "My Credentials" : "我的凭证";
    public string L_AddNew => IsEnglish ? "Add New" : "添加新凭证";
    public string L_Settings => IsEnglish ? "Settings" : "设置";
    public string L_Theme => IsEnglish ? "Theme" : "主题风格";
    public string L_DarkTheme => IsEnglish ? "Dark" : "深色";
    public string L_LightTheme => IsEnglish ? "Light" : "浅色";
    public string L_Lock => IsEnglish ? "Lock" : "锁定";
    public string L_Save => IsEnglish ? "Save" : "保存";
    public string L_Delete => IsEnglish ? "Delete" : "删除";
    public string L_Description => IsEnglish ? "Description" : "描述";
    public string L_DescPlaceholder => IsEnglish ? "Add description..." : "添加描述...";
    public string L_Attributes => IsEnglish ? "Attributes" : "属性字段";
    public string L_AttrSubtitle => IsEnglish ? "Manage credential attributes" : "管理凭证的各项属性";
    public string L_AddAttr => IsEnglish ? "Add Attribute" : "添加属性";
    public string L_SelectOne => IsEnglish ? "Select a credential" : "选择一个凭证";
    public string L_ManageDetail => IsEnglish ? "Manage credential details" : "管理此凭证的详细信息";
    public string L_CreatePwd => IsEnglish ? "Create Master Password" : "创建主密码";
    public string L_MasterPwd => IsEnglish ? "Master Password" : "主密码";
    public string L_ConfirmPwd => IsEnglish ? "Confirm Password" : "确认密码";
    public string L_PwdHint => IsEnglish ? "Password requires at least 8 characters" : "密码至少需要8个字符";

    public string L_Unlock => IsEnglish ? "Unlock" : "解锁";
    public string L_CreateUnlock => IsEnglish ? "Create and Unlock" : "创建并解锁";
    public string L_Edit => IsEnglish ? "Edit" : "编辑";
    public string L_ThemeShort => IsLightTheme ? (IsEnglish ? "Light" : "浅") : (IsEnglish ? "Dark" : "深");
    public string L_LightShort => IsEnglish ? "Light" : "浅";
    public string L_DarkShort => IsEnglish ? "Dark" : "深";

    public string UnlockButtonText => IsFirstTimeSetup ? L_CreateUnlock : L_Unlock;

    partial void OnIsEnglishChanged(bool value)
    {
        OnPropertyChanged(nameof(L_AppTitle));
        OnPropertyChanged(nameof(L_AppSubtitle));
        OnPropertyChanged(nameof(L_SearchPlaceholder));
        OnPropertyChanged(nameof(L_MyCredentials));
        OnPropertyChanged(nameof(L_AddNew));
        OnPropertyChanged(nameof(L_Settings));
        OnPropertyChanged(nameof(L_Theme));
        OnPropertyChanged(nameof(L_DarkTheme));
        OnPropertyChanged(nameof(L_LightTheme));
        OnPropertyChanged(nameof(L_Lock));
        OnPropertyChanged(nameof(L_Save));
        OnPropertyChanged(nameof(L_Delete));
        OnPropertyChanged(nameof(L_Description));
        OnPropertyChanged(nameof(L_DescPlaceholder));
        OnPropertyChanged(nameof(L_Attributes));
        OnPropertyChanged(nameof(L_AttrSubtitle));
        OnPropertyChanged(nameof(L_AddAttr));
        OnPropertyChanged(nameof(L_SelectOne));
        OnPropertyChanged(nameof(L_ManageDetail));
        OnPropertyChanged(nameof(L_CreatePwd));
        OnPropertyChanged(nameof(L_MasterPwd));
        OnPropertyChanged(nameof(L_ConfirmPwd));
        OnPropertyChanged(nameof(L_PwdHint));
        OnPropertyChanged(nameof(L_Unlock));
        OnPropertyChanged(nameof(L_CreateUnlock));
        OnPropertyChanged(nameof(L_Edit));
        OnPropertyChanged(nameof(L_ThemeShort));
        OnPropertyChanged(nameof(L_LightShort));
        OnPropertyChanged(nameof(L_DarkShort));
        OnPropertyChanged(nameof(UnlockButtonText));
        // Update all attribute view models
        foreach (var item in Items)
        {
            foreach (var attr in item.Attributes)
            {
                attr.IsEnglish = value;
            }
        }
    }

    partial void OnIsFirstTimeSetupChanged(bool value)
    {
        OnPropertyChanged(nameof(UnlockButtonText));
    }

    [RelayCommand]
    private void ToggleLanguage()
    {
        IsEnglish = !IsEnglish;
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        IsLightTheme = !IsLightTheme;
    }

    /// <summary>
    /// Callback to show add entry dialog, set by View
    /// </summary>
    public Func<AddEntryDialogViewModel, Task>? ShowAddEntryDialog { get; set; }

    /// <summary>
    /// Callback to show edit attribute dialog, set by View
    /// </summary>
    public Func<EditAttributeDialogViewModel, Task>? ShowEditAttributeDialog { get; set; }

    /// <summary>
    /// Callback to show settings dialog, set by View
    /// </summary>
    public Func<SettingsDialogViewModel, Task>? ShowSettingsDialog { get; set; }

    /// <summary>
    /// Callback to show confirmation dialog, set by View
    /// </summary>
    public Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    public MainWindowViewModel(
        IDatabaseService databaseService,
        IEncryptionService encryptionService,
        IClipboardService clipboardService)
    {
        _databaseService = databaseService;
        _encryptionService = encryptionService;
        _clipboardService = clipboardService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _databaseService.InitializeAsync();
            var settings = await _databaseService.GetSettingsAsync();
            
            IsFirstTimeSetup = string.IsNullOrEmpty(settings.PasswordHash);
            IsLocked = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
            IsLocked = true;
        }
    }

    [RelayCommand]
    private async Task UnlockAsync()
    {
        if (string.IsNullOrEmpty(MasterPassword))
        {
            ErrorMessage = IsEnglish ? "Please enter your master password" : "请输入主密码";
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var settings = await _databaseService.GetSettingsAsync();

            if (IsFirstTimeSetup)
            {
                if (MasterPassword != ConfirmPassword)
                {
                    ErrorMessage = IsEnglish ? "Passwords do not match" : "两次密码不一致";
                    return;
                }

                if (MasterPassword.Length < 8)
                {
                    ErrorMessage = IsEnglish ? "Password must be at least 8 characters" : "密码至少需要8个字符";
                    return;
                }

                // Create new password
                var salt = _encryptionService.GenerateSalt();
                var hash = _encryptionService.HashPassword(MasterPassword, salt);
                var key = _encryptionService.DeriveKey(MasterPassword, salt);

                settings.PasswordHash = hash;
                settings.PasswordSalt = Convert.ToBase64String(salt);
                await _databaseService.SaveSettingsAsync(settings);

                _encryptionService.SetSessionKey(key);
                
                // Add demo data
                await CreateDemoDataAsync();
            }
            else
            {
                // Verify existing password
                var salt = Convert.FromBase64String(settings.PasswordSalt!);
                
                if (!_encryptionService.VerifyPassword(MasterPassword, salt, settings.PasswordHash!))
                {
                    ErrorMessage = IsEnglish ? "Invalid master password" : "主密码错误";
                    return;
                }

                var key = _encryptionService.DeriveKey(MasterPassword, salt);
                _encryptionService.SetSessionKey(key);
            }

            IsLocked = false;
            IsFirstTimeSetup = false;
            MasterPassword = string.Empty;
            ConfirmPassword = string.Empty;

            await LoadItemsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Lock()
    {
        _encryptionService.ClearSessionKey();
        _clipboardService.CancelPendingClear();
        
        IsLocked = true;
        Items.Clear();
        SelectedItem = null;
        MasterPassword = string.Empty;
    }

    [RelayCommand]
    private void SelectItem(ItemViewModel? item)
    {
        SelectedItem = item;
    }

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        if (IsLocked) return;

        var entries = await _databaseService.GetAllEntriesAsync();
        
        Items.Clear();
        foreach (var entry in entries)
        {
            var itemVm = new ItemViewModel(entry, _clipboardService);
            SetupItemCallbacks(itemVm);
            Items.Add(itemVm);
        }

        if (Items.Count > 0)
        {
            SelectedItem = Items[0];
        }
    }

    private void SetupItemCallbacks(ItemViewModel itemVm)
    {
        foreach (var attr in itemVm.Attributes)
        {
            attr.IsEnglish = IsEnglish;
            attr.OnEditRequested = async (attrVm) => await EditAttributeAsync(attrVm);
            attr.OnDeleted = (attrVm) => itemVm.Attributes.Remove(attrVm);
        }
    }

    private async Task EditAttributeAsync(AttributeViewModel attrVm)
    {
        if (ShowEditAttributeDialog == null) return;

        var dialogVm = new EditAttributeDialogViewModel
        {
            Key = attrVm.Key,
            Value = attrVm.Value,
            IsEditMode = true,
            IsLightTheme = IsLightTheme,
            IsEnglish = IsEnglish
        };

        await ShowEditAttributeDialog(dialogVm);

        if (dialogVm.IsDeleted)
        {
            attrVm.OnDeleted?.Invoke(attrVm);
        }
        else if (dialogVm.IsConfirmed)
        {
            attrVm.UpdateFrom(dialogVm.Key, dialogVm.Value);
        }
    }

    [RelayCommand]
    private async Task AddAttributeAsync()
    {
        if (IsLocked || SelectedItem == null || ShowEditAttributeDialog == null) return;

        var dialogVm = new EditAttributeDialogViewModel
        {
            IsEditMode = false,
            IsLightTheme = IsLightTheme,
            IsEnglish = IsEnglish
        };

        await ShowEditAttributeDialog(dialogVm);

        if (!dialogVm.IsConfirmed) return;

        var attr = new SecretAttribute
        {
            EntryId = SelectedItem.Id,
            Type = dialogVm.DetectedType,
            Key = dialogVm.Key,
            Value = dialogVm.Value,
            IsSensitive = dialogVm.DetectedType != AttributeType.Url,
            SortOrder = SelectedItem.Attributes.Count
        };

        var attrVm = new AttributeViewModel(attr, _clipboardService)
        {
            IsEnglish = IsEnglish,
            OnEditRequested = async (vm) => await EditAttributeAsync(vm),
            OnDeleted = (vm) => SelectedItem.Attributes.Remove(vm)
        };

        SelectedItem.Attributes.Add(attrVm);
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (IsLocked) return;

        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            await LoadItemsAsync();
            return;
        }

        var entries = await _databaseService.SearchEntriesAsync(SearchQuery);
        
        Items.Clear();
        foreach (var entry in entries)
        {
            Items.Add(new ItemViewModel(entry, _clipboardService));
        }
    }

    [RelayCommand]
    private async Task AddNewItemAsync()
    {
        if (IsLocked) return;

        var dialogVm = new AddEntryDialogViewModel
        {
            IsLightTheme = IsLightTheme,
            IsEnglish = IsEnglish
        };
        
        if (ShowAddEntryDialog != null)
        {
            await ShowAddEntryDialog(dialogVm);
        }

        if (!dialogVm.IsConfirmed) return;

        var entry = new SecretEntry
        {
            Title = dialogVm.Title,
            Notes = string.IsNullOrWhiteSpace(dialogVm.Notes) ? null : dialogVm.Notes
        };

        await _databaseService.SaveEntryAsync(entry);
        
        var vm = new ItemViewModel(entry, _clipboardService);
        Items.Add(vm);
        SelectedItem = vm;
    }

    [RelayCommand]
    private async Task SaveCurrentItemAsync()
    {
        if (IsLocked || SelectedItem == null) return;

        var entry = SelectedItem.ToModel();
        await _databaseService.SaveEntryAsync(entry);
    }

    [RelayCommand]
    private async Task DeleteCurrentItemAsync()
    {
        if (IsLocked || SelectedItem == null) return;

        if (ShowConfirmDialog != null)
        {
            var title = IsEnglish ? "Confirm Delete" : "确认删除";
            var message = IsEnglish 
                ? $"Are you sure you want to delete \"{SelectedItem.Title}\"?" 
                : $"确定要删除 \"{SelectedItem.Title}\" 吗？";
            
            var confirmed = await ShowConfirmDialog(title, message);
            if (!confirmed) return;
        }

        await _databaseService.DeleteEntryAsync(SelectedItem.Id);
        Items.Remove(SelectedItem);
        SelectedItem = Items.FirstOrDefault();
    }

    [RelayCommand]
    private async Task OpenSettingsAsync()
    {
        if (ShowSettingsDialog == null) return;

        var dialogVm = new SettingsDialogViewModel(_databaseService, _encryptionService)
        {
            IsLightTheme = IsLightTheme,
            IsEnglish = IsEnglish
        };

        await ShowSettingsDialog(dialogVm);

        if (dialogVm.IsConfirmed)
        {
            IsLightTheme = dialogVm.IsLightTheme;
        }
    }

    partial void OnSearchQueryChanged(string value)
    {
        _ = SearchAsync();
    }

    private async Task CreateDemoDataAsync()
    {
        // Demo Entry 1: AWS Console
        var awsEntry = new SecretEntry
        {
            Title = "AWS Console",
            Notes = "Production AWS account",
            IconName = "Cloud"
        };
        awsEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = awsEntry.Id,
            Type = AttributeType.Url,
            Key = "Console URL",
            Value = "https://console.aws.amazon.com",
            IsSensitive = false,
            SortOrder = 0
        });
        awsEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = awsEntry.Id,
            Type = AttributeType.Username,
            Key = "Username",
            Value = "admin@company.com",
            IsSensitive = false,
            SortOrder = 1
        });
        awsEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = awsEntry.Id,
            Type = AttributeType.Password,
            Key = "Password",
            Value = "SecureP@ssw0rd!2024",
            IsSensitive = true,
            SortOrder = 2
        });
        awsEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = awsEntry.Id,
            Type = AttributeType.ApiKey,
            Key = "Access Key ID",
            Value = "AKIAIOSFODNN7EXAMPLE",
            IsSensitive = true,
            SortOrder = 3
        });
        awsEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = awsEntry.Id,
            Type = AttributeType.ApiKey,
            Key = "Secret Access Key",
            Value = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
            IsSensitive = true,
            SortOrder = 4
        });

        await _databaseService.SaveEntryAsync(awsEntry);

        // Demo Entry 2: Redis Prod
        var redisEntry = new SecretEntry
        {
            Title = "Redis Prod",
            Notes = "Production Redis cluster",
            IconName = "Database"
        };
        redisEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = redisEntry.Id,
            Type = AttributeType.Host,
            Key = "Host",
            Value = "redis-prod.internal.company.com",
            IsSensitive = false,
            SortOrder = 0
        });
        redisEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = redisEntry.Id,
            Type = AttributeType.Port,
            Key = "Port",
            Value = "6379",
            IsSensitive = false,
            SortOrder = 1
        });
        redisEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = redisEntry.Id,
            Type = AttributeType.Password,
            Key = "Auth Password",
            Value = "Redis$ecret#2024",
            IsSensitive = true,
            SortOrder = 2
        });

        await _databaseService.SaveEntryAsync(redisEntry);

        // Demo Entry 3: GitHub
        var githubEntry = new SecretEntry
        {
            Title = "GitHub",
            Notes = "Personal GitHub account",
            IconName = "Code"
        };
        githubEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = githubEntry.Id,
            Type = AttributeType.Url,
            Key = "Profile URL",
            Value = "https://github.com/developer",
            IsSensitive = false,
            SortOrder = 0
        });
        githubEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = githubEntry.Id,
            Type = AttributeType.Username,
            Key = "Username",
            Value = "developer",
            IsSensitive = false,
            SortOrder = 1
        });
        githubEntry.Attributes.Add(new SecretAttribute
        {
            EntryId = githubEntry.Id,
            Type = AttributeType.ApiKey,
            Key = "Personal Access Token",
            Value = "ghp_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
            IsSensitive = true,
            SortOrder = 2
        });

        await _databaseService.SaveEntryAsync(githubEntry);
    }
}
