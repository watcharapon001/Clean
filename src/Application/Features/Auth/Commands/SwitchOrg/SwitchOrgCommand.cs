using Application.Common.Abstractions;
using Application.Features.Auth.Commands.Login; // Reuse LoginResponse
using Application.Features.Auth.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.SwitchOrg;

public record SwitchOrgCommand(string OrgId) : IRequest<LoginResponse>;

public class SwitchOrgCommandHandler : IRequestHandler<SwitchOrgCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ITokenService _tokenService;

    public SwitchOrgCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ITokenService tokenService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(SwitchOrgCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User context is missing");

        var user = await _context.Users
            .Include(u => u.UserOrgs)
            .ThenInclude(uo => uo.Org)
            .FirstOrDefaultAsync(u => u.UserId.ToString() == userId && u.IsActive, cancellationToken);

        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        var targetOrg = user.UserOrgs.FirstOrDefault(uo => uo.OrgId.ToString() == request.OrgId && uo.IsActive && uo.Org.IsActive);

        if (targetOrg == null)
            throw new UnauthorizedAccessException("User does not have access to this organization");

        // Generate Token for new Org
        var role = "User"; // TODO: Determine role per org if needed
        var token = _tokenService.GenerateAccessToken(user.UserId.ToString(), user.Username!, role, targetOrg.OrgId.ToString());

        // Prepare response with fresh list of orgs (in case status changed, though typically static during session)
        var orgDtos = user.UserOrgs.Where(uo => uo.IsActive && uo.Org.IsActive).Select(uo => new OrgDto(
            uo.OrgId.ToString(),
            uo.Org.OrgCode,
            uo.Org.OrgName,
            uo.IsDefault
        )).ToList();

        return new LoginResponse(token, 3600, orgDtos, targetOrg.OrgId.ToString());
    }
}
