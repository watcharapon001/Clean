using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.SU;

[Table("su_audit_log")]
public class SuAuditLog
{
    [Key]
    public Guid AuditLogId { get; set; } = Guid.NewGuid();
    
    [MaxLength(255)]
    public string? UserId { get; set; }
    
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string TableName { get; set; } = string.Empty;
    
    public string? KeyValues { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
