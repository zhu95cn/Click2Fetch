namespace Click2Fetch.Models;

/// <summary>
/// Represents a secret entry containing multiple attributes
/// </summary>
public class SecretEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Title { get; set; } = string.Empty;
    
    public string? Notes { get; set; }
    
    public string? IconName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    
    public bool IsDeleted { get; set; }
    
    public List<SecretAttribute> Attributes { get; set; } = new();
}
