using Domain.Common;
using Domain.Entities.DB;

namespace Domain.Entities.SU;

public class SuUser : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? EmailNormalized { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string SecurityStamp { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsLocked { get; set; }
    public DateTimeOffset? LockoutEndAt { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }

    // Navigation
    public DbEmployee? Employee { get; set; }
    public ICollection<SuUserOrg> UserOrgs { get; set; } = new List<SuUserOrg>();
    public ICollection<SuUserProfile> UserProfiles { get; set; } = new List<SuUserProfile>();
}
