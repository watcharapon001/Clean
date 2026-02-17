using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities.SU;

namespace Application.Features.SU.SURT04;

public class Surt04UserOrgDto : IMapFrom<SuUserOrg>
{
    public Guid OrgId { get; set; }
    public string OrgCode { get; set; } = string.Empty;
    public string OrgName { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SuUserOrg, Surt04UserOrgDto>()
            .ForMember(d => d.OrgCode, opt => opt.MapFrom(s => s.Org.OrgCode))
            .ForMember(d => d.OrgName, opt => opt.MapFrom(s => s.Org.OrgName));
    }
}
