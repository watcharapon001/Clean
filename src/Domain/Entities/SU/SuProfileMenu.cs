using Domain.Common;

namespace Domain.Entities.SU;

public class SuProfileMenu : BaseAuditableEntity
{
    public Guid ProfileId { get; set; }
    public Guid MenuId { get; set; }
    
    public bool CanView { get; set; } = true;
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }

    // Navigation
    public SuProfile Profile { get; set; } = null!;
    public SuMenu Menu { get; set; } = null!;
}
