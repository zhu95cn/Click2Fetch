using Click2Fetch.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Click2Fetch.Services;

/// <summary>
/// SQLite database service implementation
/// </summary>
public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    private readonly IEncryptionService _encryptionService;

    public DatabaseService(IEncryptionService encryptionService)
    {
        // Portable mode: database in same directory as exe
        var exePath = AppContext.BaseDirectory;
        var dbPath = Path.Combine(exePath, "secrets.db");
        _connectionString = $"Data Source={dbPath}";
        _encryptionService = encryptionService;
    }

    public async Task InitializeAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var createTables = @"
            CREATE TABLE IF NOT EXISTS Settings (
                Key TEXT PRIMARY KEY,
                Value TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS SecretEntries (
                Id TEXT PRIMARY KEY,
                Title TEXT NOT NULL,
                Notes TEXT,
                IconName TEXT,
                CreatedAt TEXT NOT NULL,
                LastModified TEXT NOT NULL,
                IsDeleted INTEGER NOT NULL DEFAULT 0
            );

            CREATE TABLE IF NOT EXISTS SecretAttributes (
                Id TEXT PRIMARY KEY,
                EntryId TEXT NOT NULL,
                Type INTEGER NOT NULL,
                Key TEXT NOT NULL,
                Value TEXT NOT NULL,
                Nonce TEXT NOT NULL,
                Tag TEXT NOT NULL,
                IsSensitive INTEGER NOT NULL DEFAULT 1,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY (EntryId) REFERENCES SecretEntries(Id) ON DELETE CASCADE
            );

            CREATE INDEX IF NOT EXISTS idx_attributes_entry ON SecretAttributes(EntryId);
            CREATE INDEX IF NOT EXISTS idx_entries_title ON SecretEntries(Title);
        ";

        await connection.ExecuteAsync(createTables);
    }

    public async Task<List<SecretEntry>> GetAllEntriesAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        var rows = await connection.QueryAsync<dynamic>(
            "SELECT * FROM SecretEntries WHERE IsDeleted = 0 ORDER BY Title");

        var entries = new List<SecretEntry>();
        foreach (var row in rows)
        {
            var entry = new SecretEntry
            {
                Id = Guid.Parse(row.Id),
                Title = row.Title,
                Notes = row.Notes,
                IconName = row.IconName,
                CreatedAt = DateTime.Parse(row.CreatedAt),
                LastModified = DateTime.Parse(row.LastModified),
                IsDeleted = row.IsDeleted == 1
            };
            entry.Attributes = await GetAttributesByEntryIdAsync(entry.Id);
            entries.Add(entry);
        }

        return entries;
    }

    public async Task<SecretEntry?> GetEntryByIdAsync(Guid id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT * FROM SecretEntries WHERE Id = @Id", new { Id = id.ToString() });

        if (row == null) return null;

        var entry = new SecretEntry
        {
            Id = Guid.Parse(row.Id),
            Title = row.Title,
            Notes = row.Notes,
            IconName = row.IconName,
            CreatedAt = DateTime.Parse(row.CreatedAt),
            LastModified = DateTime.Parse(row.LastModified),
            IsDeleted = row.IsDeleted == 1
        };
        
        entry.Attributes = await GetAttributesByEntryIdAsync(entry.Id);
        return entry;
    }

    public async Task SaveEntryAsync(SecretEntry entry)
    {
        await using var connection = new SqliteConnection(_connectionString);
        
        entry.LastModified = DateTime.UtcNow;

        var sql = @"
            INSERT INTO SecretEntries (Id, Title, Notes, IconName, CreatedAt, LastModified, IsDeleted)
            VALUES (@Id, @Title, @Notes, @IconName, @CreatedAt, @LastModified, @IsDeleted)
            ON CONFLICT(Id) DO UPDATE SET
                Title = excluded.Title,
                Notes = excluded.Notes,
                IconName = excluded.IconName,
                LastModified = excluded.LastModified,
                IsDeleted = excluded.IsDeleted";

        await connection.ExecuteAsync(sql, new
        {
            Id = entry.Id.ToString(),
            entry.Title,
            entry.Notes,
            entry.IconName,
            CreatedAt = entry.CreatedAt.ToString("O"),
            LastModified = entry.LastModified.ToString("O"),
            IsDeleted = entry.IsDeleted ? 1 : 0
        });

        // Save attributes
        foreach (var attr in entry.Attributes)
        {
            attr.EntryId = entry.Id;
            await SaveAttributeAsync(attr);
        }
    }

    public async Task DeleteEntryAsync(Guid id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        
        // Soft delete
        await connection.ExecuteAsync(
            "UPDATE SecretEntries SET IsDeleted = 1, LastModified = @LastModified WHERE Id = @Id",
            new { Id = id.ToString(), LastModified = DateTime.UtcNow.ToString("O") });
    }

    public async Task<List<SecretAttribute>> GetAttributesByEntryIdAsync(Guid entryId)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var rawAttributes = await connection.QueryAsync<dynamic>(
            "SELECT * FROM SecretAttributes WHERE EntryId = @EntryId ORDER BY SortOrder",
            new { EntryId = entryId.ToString() });

        var attributes = new List<SecretAttribute>();

        foreach (var raw in rawAttributes)
        {
            try
            {
                var decryptedValue = _encryptionService.DecryptWithSessionKey(
                    Convert.FromBase64String(raw.Value),
                    Convert.FromBase64String(raw.Nonce),
                    Convert.FromBase64String(raw.Tag));

                attributes.Add(new SecretAttribute
                {
                    Id = Guid.Parse(raw.Id),
                    EntryId = Guid.Parse(raw.EntryId),
                    Type = (AttributeType)raw.Type,
                    Key = raw.Key,
                    Value = decryptedValue,
                    IsSensitive = raw.IsSensitive == 1,
                    SortOrder = raw.SortOrder
                });
            }
            catch
            {
                // Skip attributes that can't be decrypted
            }
        }

        return attributes;
    }

    public async Task SaveAttributeAsync(SecretAttribute attribute)
    {
        await using var connection = new SqliteConnection(_connectionString);

        var (ciphertext, nonce, tag) = _encryptionService.EncryptWithSessionKey(attribute.Value);

        var sql = @"
            INSERT INTO SecretAttributes (Id, EntryId, Type, Key, Value, Nonce, Tag, IsSensitive, SortOrder)
            VALUES (@Id, @EntryId, @Type, @Key, @Value, @Nonce, @Tag, @IsSensitive, @SortOrder)
            ON CONFLICT(Id) DO UPDATE SET
                Type = excluded.Type,
                Key = excluded.Key,
                Value = excluded.Value,
                Nonce = excluded.Nonce,
                Tag = excluded.Tag,
                IsSensitive = excluded.IsSensitive,
                SortOrder = excluded.SortOrder";

        await connection.ExecuteAsync(sql, new
        {
            Id = attribute.Id.ToString(),
            EntryId = attribute.EntryId.ToString(),
            Type = (int)attribute.Type,
            attribute.Key,
            Value = Convert.ToBase64String(ciphertext),
            Nonce = Convert.ToBase64String(nonce),
            Tag = Convert.ToBase64String(tag),
            IsSensitive = attribute.IsSensitive ? 1 : 0,
            attribute.SortOrder
        });
    }

    public async Task DeleteAttributeAsync(Guid id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM SecretAttributes WHERE Id = @Id",
            new { Id = id.ToString() });
    }

    public async Task<AppSettings> GetSettingsAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        var settings = new AppSettings();
        var rows = await connection.QueryAsync<dynamic>(
            "SELECT Key, Value FROM Settings");

        foreach (var row in rows)
        {
            string key = row.Key;
            string value = row.Value;
            switch (key)
            {
                case "PasswordHash": settings.PasswordHash = value; break;
                case "PasswordSalt": settings.PasswordSalt = value; break;
                case "ClipboardClearSeconds": settings.ClipboardClearSeconds = int.TryParse(value, out var s) ? s : 30; break;
                case "AutoLockEnabled": settings.AutoLockEnabled = value == "1"; break;
                case "AutoLockMinutes": settings.AutoLockMinutes = int.TryParse(value, out var m) ? m : 5; break;
                case "Theme": settings.Theme = value; break;
            }
        }

        return settings;
    }

    public async Task SaveSettingsAsync(AppSettings settings)
    {
        await using var connection = new SqliteConnection(_connectionString);

        var upsertSql = @"
            INSERT INTO Settings (Key, Value) VALUES (@Key, @Value)
            ON CONFLICT(Key) DO UPDATE SET Value = excluded.Value";

        var settingsDict = new Dictionary<string, string>
        {
            ["PasswordHash"] = settings.PasswordHash ?? "",
            ["PasswordSalt"] = settings.PasswordSalt ?? "",
            ["ClipboardClearSeconds"] = settings.ClipboardClearSeconds.ToString(),
            ["AutoLockEnabled"] = settings.AutoLockEnabled ? "1" : "0",
            ["AutoLockMinutes"] = settings.AutoLockMinutes.ToString(),
            ["Theme"] = settings.Theme
        };

        foreach (var kvp in settingsDict)
        {
            await connection.ExecuteAsync(upsertSql, new { Key = kvp.Key, Value = kvp.Value });
        }
    }

    public async Task<List<SecretEntry>> SearchEntriesAsync(string query)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var rows = await connection.QueryAsync<dynamic>(
            "SELECT * FROM SecretEntries WHERE IsDeleted = 0 AND Title LIKE @Query ORDER BY Title",
            new { Query = $"%{query}%" });

        var entries = new List<SecretEntry>();
        foreach (var row in rows)
        {
            var entry = new SecretEntry
            {
                Id = Guid.Parse(row.Id),
                Title = row.Title,
                Notes = row.Notes,
                IconName = row.IconName,
                CreatedAt = DateTime.Parse(row.CreatedAt),
                LastModified = DateTime.Parse(row.LastModified),
                IsDeleted = row.IsDeleted == 1
            };
            entry.Attributes = await GetAttributesByEntryIdAsync(entry.Id);
            entries.Add(entry);
        }

        return entries;
    }
}
