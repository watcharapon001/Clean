using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Application.Features.Auth.Common;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;

public record LoginResponse(string AccessToken, int ExpiresInSeconds, List<OrgDto> Orgs, string DefaultOrgId);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }


    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Find User (Fetch by username first to check specific status)
        var user = await _context.Users
            .Include(u => u.UserOrgs)
            .ThenInclude(uo => uo.Org)
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // Check Status
        if (!user.IsActive)
            throw new UnauthorizedAccessException("User is inactive"); // Or Forbidden, but usually Auth fails

        if (user.IsLocked) {
             if (user.LockoutEndAt.HasValue && user.LockoutEndAt > DateTimeOffset.UtcNow)
                throw new UnauthorizedAccessException("User is locked");
             // If lock expired, we might unlock, but for simplicity just throw if IsLocked is true
        }

        // 2. Verify Password
        if (!_passwordHasher.VerifyPassword(user, user.PasswordHash!, request.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        // 3. Determine Org (Default first, otherwise first available)
        var userOrgs = user.UserOrgs.Where(uo => uo.IsActive && uo.Org.IsActive).ToList();
        var defaultOrg = userOrgs.FirstOrDefault(uo => uo.IsDefault) ?? userOrgs.FirstOrDefault();
        
        var orgId = defaultOrg?.OrgId.ToString() ?? "";

        // 4. Generate Token
        // TODO: Role management (for now verify user has UserProfile or similar if needed)
        // For simplicity, we just pass "User" or check UserProfiles if loaded. 
        // Let's assume generic "User" role for now, or empty.
        var role = "User"; 

        var token = _tokenService.GenerateAccessToken(user.UserId.ToString(), user.Username!, role, orgId);

        // 5. Build Response
        var orgDtos = userOrgs.Select(uo => new OrgDto(
            uo.OrgId.ToString(),
            uo.Org.OrgCode,
            uo.Org.OrgName,
            uo.IsDefault
        )).ToList();

        return new LoginResponse(token, 3600, orgDtos, orgId); // 1 hour hardcoded matches default settings
    }
}

// Local OrgDto removed, using Application.Features.Auth.Common.OrgDto instead

