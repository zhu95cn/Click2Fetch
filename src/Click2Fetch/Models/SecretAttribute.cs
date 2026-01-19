namespace Click2Fetch.Models;

/// <summary>
/// Types of secret attributes
/// </summary>
public enum AttributeType
{
    Username,
    Password,
    Url,
    ApiKey,
    Text,
    Email,
    Port,
    Host,
    Application,
    Custom
}

/// <summary>
/// Represents a single attribute of a secret entry
/// </summary>
public class SecretAttribute
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid EntryId { get; set; }
    
    public AttributeType Type { get; set; }
    
    /// <summary>
    /// Display label (e.g., "Redis Port", "Admin Username")
    /// </summary>
    public string Key { get; set; } = string.Empty;
    
    /// <summary>
    /// The actual value (stored encrypted in database)
    /// </summary>
    public string Value { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the value should be masked by default
    /// </summary>
    public bool IsSensitive { get; set; } = true;
    
    public int SortOrder { get; set; }
}
