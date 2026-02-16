using Application.Common.Mappings;
using Domain.Entities.SU;

namespace Application.Features.SU.Menus.Queries.GetMenus;

public class MenuDto : IMapFrom<SuMenu>
{
    public Guid MenuId { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    
    public List<MenuDto> Children { get; set; } = new();
}
