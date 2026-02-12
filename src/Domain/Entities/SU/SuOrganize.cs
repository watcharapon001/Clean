using Domain.Common;
using Domain.Entities.DB;

namespace Domain.Entities.SU;

public class SuOrganize : BaseAuditableEntity
{
    public Guid OrgId { get; set; }
    public string OrgCode { get; set; } = string.Empty;
    public string OrgName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<DbEmployee> Employees { get; set; } = new List<DbEmployee>();
    public ICollection<SuUserOrg> UserOrgs { get; set; } = new List<SuUserOrg>();
}
