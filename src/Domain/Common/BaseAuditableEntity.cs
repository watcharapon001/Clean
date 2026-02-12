namespace Domain.Common;

public abstract class BaseAuditableEntity
{
    public string? CreateBy { get; set; }
    public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;
    public string? UpdateBy { get; set; }
    public DateTimeOffset? UpdateDate { get; set; }
}
