using System.Diagnostics;
using Click2Fetch.Models;
using Click2Fetch.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Click2Fetch.ViewModels;

/// <summary>
/// ViewModel for a single attribute card
/// </summary>
public partial class AttributeViewModel : ViewModelBase
{
    private readonly SecretAttribute _attribute;
    private readonly IClipboardService _clipboardService;

    [ObservableProperty]
    private string _key = string.Empty;

    [ObservableProperty]
    private string _value = string.Empty;

    [ObservableProperty]
    private string _displayValue = string.Empty;

    [ObservableProperty]
    private bool _isSensitive;

    [ObservableProperty]
    private bool _isValueVisible;

    [ObservableProperty]
    private AttributeType _type;

    [ObservableProperty]
    private string _iconKind = "Key";

    [ObservableProperty]
    private bool _isCopied;

    [ObservableProperty]
    private bool _isEnglish;

    /// <summary>
    /// Action hint text based on type and copied state
    /// </summary>
    public string ActionHint => Type switch
    {
        AttributeType.Url => IsEnglish ? "Click to open" : "点击前往",
        AttributeType.Application => IsEnglish ? "Click to open" : "点击打开",
        _ => IsCopied ? (IsEnglish ? "✓ Copied" : "✓ 已复制") : (IsEnglish ? "Click to copy" : "点击复制")
    };

    partial void OnIsCopiedChanged(bool value)
    {
        OnPropertyChanged(nameof(ActionHint));
    }

    partial void OnIsEnglishChanged(bool value)
    {
        OnPropertyChanged(nameof(ActionHint));
    }

    public Guid Id => _attribute.Id;
    public Guid EntryId => _attribute.EntryId;

    /// <summary>
    /// Callback to show edit dialog, set by parent
    /// </summary>
    public Func<AttributeViewModel, Task>? OnEditRequested { get; set; }

    /// <summary>
    /// Callback when attribute is deleted
    /// </summary>
    public Action<AttributeViewModel>? OnDeleted { get; set; }

    public AttributeViewModel(SecretAttribute attribute, IClipboardService clipboardService)
    {
        _attribute = attribute;
        _clipboardService = clipboardService;

        Key = attribute.Key;
        Value = attribute.Value;
        IsSensitive = attribute.IsSensitive;
        Type = attribute.Type;
        
        UpdateDisplayValue();
        UpdateIcon();
    }

    private void UpdateDisplayValue()
    {
        if (IsSensitive && !IsValueVisible)
        {
            DisplayValue = "••••••••••••";
        }
        else
        {
            DisplayValue = Value;
        }
    }

    private void UpdateIcon()
    {
        IconKind = Type switch
        {
            AttributeType.Username => "Person",
            AttributeType.Password => "Lock",
            AttributeType.Url => "Link",
            AttributeType.ApiKey => "Key",
            AttributeType.Email => "Mail",
            AttributeType.Port => "Server",
            AttributeType.Host => "Desktop",
            _ => "Document"
        };
    }

    [RelayCommand]
    private void ToggleVisibility()
    {
        IsValueVisible = !IsValueVisible;
        UpdateDisplayValue();
    }

    [RelayCommand]
    private async Task CopyOrOpenAsync()
    {
        if (Type == AttributeType.Url && Uri.TryCreate(Value, UriKind.Absolute, out var uri))
        {
            // Open URL in browser
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = uri.ToString(),
                    UseShellExecute = true
                });
            }
            catch
            {
                await CopyToClipboardAsync();
            }
        }
        else if (Type == AttributeType.Application && !string.IsNullOrEmpty(Value))
        {
            // Open application
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Value,
                    UseShellExecute = true
                });
            }
            catch
            {
                await CopyToClipboardAsync();
            }
        }
        else
        {
            await CopyToClipboardAsync();
        }
    }

    private async Task CopyToClipboardAsync()
    {
        await _clipboardService.CopyToClipboardAsync(Value);
        
        IsCopied = true;
        await Task.Delay(1500);
        IsCopied = false;
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        if (OnEditRequested != null)
        {
            await OnEditRequested(this);
        }
    }

    [RelayCommand]
    private void Delete()
    {
        OnDeleted?.Invoke(this);
    }

    public void UpdateFrom(string key, string value)
    {
        Key = key;
        Value = value;
        UpdateDisplayValue();
    }

    public SecretAttribute ToModel()
    {
        return new SecretAttribute
        {
            Id = _attribute.Id,
            EntryId = _attribute.EntryId,
            Type = Type,
            Key = Key,
            Value = Value,
            IsSensitive = IsSensitive,
            SortOrder = _attribute.SortOrder
        };
    }
}
