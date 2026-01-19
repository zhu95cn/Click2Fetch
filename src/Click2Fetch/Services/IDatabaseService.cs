using Click2Fetch.Models;

namespace Click2Fetch.Services;

/// <summary>
/// Interface for database operations
/// </summary>
public interface IDatabaseService
{
    Task InitializeAsync();
    
    // Secret Entry operations
    Task<List<SecretEntry>> GetAllEntriesAsync();
    Task<SecretEntry?> GetEntryByIdAsync(Guid id);
    Task SaveEntryAsync(SecretEntry entry);
    Task DeleteEntryAsync(Guid id);
    
    // Secret Attribute operations
    Task<List<SecretAttribute>> GetAttributesByEntryIdAsync(Guid entryId);
    Task SaveAttributeAsync(SecretAttribute attribute);
    Task DeleteAttributeAsync(Guid id);
    
    // Settings operations
    Task<AppSettings> GetSettingsAsync();
    Task SaveSettingsAsync(AppSettings settings);
    
    // Search
    Task<List<SecretEntry>> SearchEntriesAsync(string query);
}
