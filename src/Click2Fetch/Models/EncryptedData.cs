namespace Click2Fetch.Models;

/// <summary>
/// Encrypted data wrapper for storage
/// </summary>
public class EncryptedData
{
    /// <summary>
    /// Base64 encoded ciphertext
    /// </summary>
    public required string Ciphertext { get; set; }
    
    /// <summary>
    /// Base64 encoded nonce/IV
    /// </summary>
    public required string Nonce { get; set; }
    
    /// <summary>
    /// Base64 encoded authentication tag
    /// </summary>
    public required string Tag { get; set; }
}

/// <summary>
/// Application settings stored locally
/// </summary>
public class AppSettings
{
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public int ClipboardClearSeconds { get; set; } = 30;
    public bool AutoLockEnabled { get; set; } = true;
    public int AutoLockMinutes { get; set; } = 5;
    public string Theme { get; set; } = "Dark";
    
    // Email backup settings
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; } = 587;
    public string? SmtpEmail { get; set; }
    public string? SmtpPassword { get; set; }
    public string? BackupTargetEmail { get; set; }
}
