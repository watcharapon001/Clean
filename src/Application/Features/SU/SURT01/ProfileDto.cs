using Application.Common.Mappings;
using Domain.Entities.SU;

namespace Application.Features.SU.SURT01;

public class ProfileDto : IMapFrom<SuProfile>
{
    public Guid ProfileId { get; set; }
    public string ProfileCode { get; set; } = string.Empty;
    public string ProfileName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
