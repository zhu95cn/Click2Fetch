namespace Click2Fetch.Services;

/// <summary>
/// Interface for encryption operations using AES-256-GCM and Argon2id
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Derives a cryptographic key from the master password using Argon2id
    /// </summary>
    byte[] DeriveKey(string masterPassword, byte[] salt);
    
    /// <summary>
    /// Generates a random salt for key derivation
    /// </summary>
    byte[] GenerateSalt();
    
    /// <summary>
    /// Hashes the master password for verification using Argon2id
    /// </summary>
    string HashPassword(string masterPassword, byte[] salt);
    
    /// <summary>
    /// Verifies the master password against the stored hash
    /// </summary>
    bool VerifyPassword(string masterPassword, byte[] salt, string storedHash);
    
    /// <summary>
    /// Encrypts plaintext using AES-256-GCM
    /// </summary>
    (byte[] ciphertext, byte[] nonce, byte[] tag) Encrypt(string plaintext, byte[] key);
    
    /// <summary>
    /// Decrypts ciphertext using AES-256-GCM
    /// </summary>
    string Decrypt(byte[] ciphertext, byte[] key, byte[] nonce, byte[] tag);
    
    /// <summary>
    /// Sets the current session key (derived from master password)
    /// </summary>
    void SetSessionKey(byte[] key);
    
    /// <summary>
    /// Clears the session key (on lock/logout)
    /// </summary>
    void ClearSessionKey();
    
    /// <summary>
    /// Encrypts using the current session key
    /// </summary>
    (byte[] ciphertext, byte[] nonce, byte[] tag) EncryptWithSessionKey(string plaintext);
    
    /// <summary>
    /// Decrypts using the current session key
    /// </summary>
    string DecryptWithSessionKey(byte[] ciphertext, byte[] nonce, byte[] tag);
    
    /// <summary>
    /// Whether a session key is currently set
    /// </summary>
    bool HasSessionKey { get; }
}
