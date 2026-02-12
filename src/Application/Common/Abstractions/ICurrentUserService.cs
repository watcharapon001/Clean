namespace Application.Common.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? OrgId { get; }
}
