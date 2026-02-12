using Domain.Common;

namespace Domain.Entities.SU;

public class SuProfile : BaseAuditableEntity
{
    public Guid ProfileId { get; set; }
    public string ProfileCode { get; set; } = string.Empty;
    public string ProfileName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<SuUserProfile> UserProfiles { get; set; } = new List<SuUserProfile>();
}
