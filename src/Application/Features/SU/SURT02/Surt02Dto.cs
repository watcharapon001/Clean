using Application.Common.Mappings;
using Domain.Entities.SU;

namespace Application.Features.SU.SURT02;

public class Surt02Dto : IMapFrom<SuMenu>
{
    public Guid MenuId { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
    public string? ParentMenuName { get; set; }
    public bool IsActive { get; set; }
}
