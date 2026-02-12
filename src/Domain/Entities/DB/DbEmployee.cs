using Domain.Common;
using Domain.Entities.SU;

namespace Domain.Entities.DB;

public class DbEmployee : BaseAuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Guid OrgId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public SuOrganize Org { get; set; } = null!;
    public SuUser? User { get; set; }
}
