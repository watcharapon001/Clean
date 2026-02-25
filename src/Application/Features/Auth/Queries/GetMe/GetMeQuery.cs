using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Queries.GetMe;

public record GetMeQuery : IRequest<UserDto>;

public record UserDto(
    string UserId,
    string Username,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    string? Email,
    string? OrgId,
    string? OrgName,
    string? EmployeeId,
    List<string> Roles
);

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMeQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UserDto> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var tokenOrgId = _currentUserService.OrgId;

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User context is missing");

        var user = await _context.Users
            .Include(u => u.Employee)
            .ThenInclude(e => e!.Org) // Nullable
            .Include(u => u.UserOrgs)
            .ThenInclude(uo => uo.Org)
            .FirstOrDefaultAsync(u => u.UserId.ToString() == userId, cancellationToken);
        
        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        // Determine Context Org (Prioritize Token > Default > First)
        string? currentOrgId = tokenOrgId;
        string? currentOrgName = null;

        if (!string.IsNullOrEmpty(currentOrgId))
        {
            var org = user.UserOrgs.FirstOrDefault(uo => uo.OrgId.ToString() == currentOrgId)?.Org;
            currentOrgName = org?.OrgName;
        }
        else
        {
            // Fallback
            var def = user.UserOrgs.FirstOrDefault(uo => uo.IsDefault) ?? user.UserOrgs.FirstOrDefault();
            currentOrgId = def?.OrgId.ToString();
            currentOrgName = def?.Org?.OrgName;
        }

        // Roles (Placeholder for now)
        var roles = new List<string> { "User" };

        return new UserDto(
            user.UserId.ToString(),
            user.Username ?? "",
            user.Employee?.FirstName,
            user.Employee?.LastName,
            user.Employee?.DisplayName,
            user.Email,
            currentOrgId,
            currentOrgName,
            user.EmployeeId?.ToString(),
            roles
        );
    }
}
