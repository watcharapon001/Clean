using Domain.Common;

namespace Domain.Entities.SU;

public class SuUserProfile : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid ProfileId { get; set; }

    // Navigation
    public SuUser User { get; set; } = null!;
    public SuProfile Profile { get; set; } = null!;
}
