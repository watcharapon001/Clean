using Domain.Common;

namespace Domain.Entities.SU;

public class SuMenu : BaseAuditableEntity
{
    public Guid MenuId { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public SuMenu? ParentMenu { get; set; }
    public ICollection<SuMenu> ChildMenus { get; set; } = new List<SuMenu>();
    public ICollection<SuProfileMenu> ProfileMenus { get; set; } = new List<SuProfileMenu>();
}
