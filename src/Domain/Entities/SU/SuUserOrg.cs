using Domain.Common;

namespace Domain.Entities.SU;

public class SuUserOrg : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid OrgId { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public SuUser User { get; set; } = null!;
    public SuOrganize Org { get; set; } = null!;
}
