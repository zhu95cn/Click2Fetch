using System.Collections.ObjectModel;
using Click2Fetch.Models;
using Click2Fetch.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Click2Fetch.ViewModels;

/// <summary>
/// ViewModel for a single secret entry item
/// </summary>
public partial class ItemViewModel : ViewModelBase
{
    private readonly SecretEntry _entry;
    private readonly IClipboardService _clipboardService;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string? _notes;

    [ObservableProperty]
    private string? _iconName;

    [ObservableProperty]
    private DateTime _lastModified;

    [ObservableProperty]
    private ObservableCollection<AttributeViewModel> _attributes = new();

    public Guid Id => _entry.Id;

    public ItemViewModel(SecretEntry entry, IClipboardService clipboardService)
    {
        _entry = entry;
        _clipboardService = clipboardService;

        Title = entry.Title;
        Notes = entry.Notes;
        IconName = entry.IconName;
        LastModified = entry.LastModified;

        foreach (var attr in entry.Attributes)
        {
            Attributes.Add(new AttributeViewModel(attr, clipboardService));
        }
    }

    public void AddAttribute(AttributeType type, string key, string value, bool isSensitive = true)
    {
        var attr = new SecretAttribute
        {
            EntryId = _entry.Id,
            Type = type,
            Key = key,
            Value = value,
            IsSensitive = isSensitive,
            SortOrder = Attributes.Count
        };

        Attributes.Add(new AttributeViewModel(attr, _clipboardService));
    }

    public SecretEntry ToModel()
    {
        return new SecretEntry
        {
            Id = _entry.Id,
            Title = Title,
            Notes = Notes,
            IconName = IconName,
            CreatedAt = _entry.CreatedAt,
            LastModified = DateTime.UtcNow,
            IsDeleted = _entry.IsDeleted,
            Attributes = Attributes.Select(a => a.ToModel()).ToList()
        };
    }
}
