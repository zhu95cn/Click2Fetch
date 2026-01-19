using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Click2Fetch.Services;

/// <summary>
/// Implementation of encryption using AES-256-GCM and Argon2id
/// </summary>
public class EncryptionService : IEncryptionService
{
    private const int KeySize = 32; // 256 bits
    private const int NonceSize = 12; // 96 bits for GCM
    private const int TagSize = 16; // 128 bits
    private const int SaltSize = 32; // 256 bits
    
    // Argon2id parameters (OWASP recommended)
    private const int Argon2Iterations = 3;
    private const int Argon2MemorySize = 65536; // 64 MB
    private const int Argon2Parallelism = 4;
    
    private byte[]? _sessionKey;
    
    public bool HasSessionKey => _sessionKey != null;
    
    public byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(SaltSize);
    }
    
    public byte[] DeriveKey(string masterPassword, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(masterPassword))
        {
            Salt = salt,
            DegreeOfParallelism = Argon2Parallelism,
            Iterations = Argon2Iterations,
            MemorySize = Argon2MemorySize
        };
        
        return argon2.GetBytes(KeySize);
    }
    
    public string HashPassword(string masterPassword, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(masterPassword))
        {
            Salt = salt,
            DegreeOfParallelism = Argon2Parallelism,
            Iterations = Argon2Iterations,
            MemorySize = Argon2MemorySize
        };
        
        var hash = argon2.GetBytes(KeySize);
        return Convert.ToBase64String(hash);
    }
    
    public bool VerifyPassword(string masterPassword, byte[] salt, string storedHash)
    {
        var computedHash = HashPassword(masterPassword, salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computedHash),
            Convert.FromBase64String(storedHash));
    }
    
    public (byte[] ciphertext, byte[] nonce, byte[] tag) Encrypt(string plaintext, byte[] key)
    {
        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var ciphertext = new byte[plaintextBytes.Length];
        var tag = new byte[TagSize];
        
        using var aes = new AesGcm(key, TagSize);
        aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);
        
        return (ciphertext, nonce, tag);
    }
    
    public string Decrypt(byte[] ciphertext, byte[] key, byte[] nonce, byte[] tag)
    {
        var plaintext = new byte[ciphertext.Length];
        
        using var aes = new AesGcm(key, TagSize);
        aes.Decrypt(nonce, ciphertext, tag, plaintext);
        
        return Encoding.UTF8.GetString(plaintext);
    }
    
    public void SetSessionKey(byte[] key)
    {
        ClearSessionKey();
        _sessionKey = new byte[key.Length];
        Array.Copy(key, _sessionKey, key.Length);
    }
    
    public void ClearSessionKey()
    {
        if (_sessionKey != null)
        {
            CryptographicOperations.ZeroMemory(_sessionKey);
            _sessionKey = null;
        }
    }
    
    public (byte[] ciphertext, byte[] nonce, byte[] tag) EncryptWithSessionKey(string plaintext)
    {
        if (_sessionKey == null)
            throw new InvalidOperationException("Session key not set. Please unlock first.");
        
        return Encrypt(plaintext, _sessionKey);
    }
    
    public string DecryptWithSessionKey(byte[] ciphertext, byte[] nonce, byte[] tag)
    {
        if (_sessionKey == null)
            throw new InvalidOperationException("Session key not set. Please unlock first.");
        
        return Decrypt(ciphertext, _sessionKey, nonce, tag);
    }
}
