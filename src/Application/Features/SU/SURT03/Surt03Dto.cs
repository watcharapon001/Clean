using Application.Common.Mappings;
using Domain.Entities.SU;

namespace Application.Features.SU.SURT03;

public class Surt03Dto
{
    public Guid MenuId { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public Guid? ParentMenuId { get; set; }
    public string? ParentMenuName { get; set; }
    public int Sequence { get; set; }

    // Permissions
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
