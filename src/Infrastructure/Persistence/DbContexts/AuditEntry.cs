using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Domain.Entities.SU;

namespace Infrastructure.Persistence.DbContexts;

public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    public EntityEntry Entry { get; }
    public string UserId { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    
    public Dictionary<string, object?> KeyValues { get; } = new();
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();

    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public SuAuditLog ToAudit()
    {
        return new SuAuditLog
        {
            AuditLogId = Guid.NewGuid(),
            UserId = UserId,
            Action = Action,
            TableName = TableName,
            Timestamp = DateTimeOffset.UtcNow,
            KeyValues = KeyValues.Count == 0 ? null : JsonSerializer.Serialize(KeyValues),
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues)
        };
    }
}
