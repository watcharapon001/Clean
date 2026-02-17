using Application.Common.Mappings;
using Domain.Entities.SU;

namespace Application.Features.SU.SURT04;

public class OrganizeDto : IMapFrom<SuOrganize>
{
    public Guid OrgId { get; set; }
    public string OrgCode { get; set; } = string.Empty;
    public string OrgName { get; set; } = string.Empty;
}
