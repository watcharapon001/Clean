using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities.SU;

namespace Application.Features.SU.SURT04;

public class Surt04Dto : IMapFrom<SuUser>
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid? EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public List<Guid> ProfileIds { get; set; } = new();
    public List<string> ProfileNames { get; set; } = new();
    public List<Surt04UserOrgDto> UserOrgs { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SuUser, Surt04Dto>()
            .ForMember(d => d.EmployeeName, opt => opt.MapFrom(s => s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : string.Empty))
            .ForMember(d => d.ProfileIds, opt => opt.MapFrom(s => s.UserProfiles.Select(up => up.ProfileId).ToList()))
            .ForMember(d => d.ProfileNames, opt => opt.MapFrom(s => s.UserProfiles.Select(up => up.Profile.ProfileName).ToList()))
            .ForMember(d => d.UserOrgs, opt => opt.MapFrom(s => s.UserOrgs));
    }
}
