using Application.Common.Abstractions;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT04.Commands;

public class UserOrgInput
{
    public Guid OrgId { get; set; }
    public bool IsDefault { get; set; }
}

// 1. Create
public class CreateSurt04Command : IRequest<Guid>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid? EmployeeId { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> ProfileIds { get; set; } = new();
    public List<UserOrgInput> UserOrgs { get; set; } = new();
}

public class CreateSurt04CommandHandler : IRequestHandler<CreateSurt04Command, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateSurt04CommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateSurt04Command request, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username, cancellationToken))
        {
            throw new Exception($"User with username '{request.Username}' already exists.");
        }

        var user = new SuUser
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            EmailNormalized = request.Email.ToUpperInvariant(),
            EmployeeId = request.EmployeeId,
            IsActive = request.IsActive,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        foreach (var profileId in request.ProfileIds)
        {
            user.UserProfiles.Add(new SuUserProfile
            {
                UserId = user.UserId,
                ProfileId = profileId
            });
        }

        foreach (var userOrg in request.UserOrgs)
        {
            user.UserOrgs.Add(new SuUserOrg
            {
                UserId = user.UserId,
                OrgId = userOrg.OrgId,
                IsDefault = userOrg.IsDefault
            });
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.UserId;
    }
}

// 2. Update
public class UpdateSurt04Command : IRequest
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Password { get; set; } // Optional: Only update if provided
    public string Email { get; set; } = string.Empty;
    public Guid? EmployeeId { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> ProfileIds { get; set; } = new();
    public List<UserOrgInput> UserOrgs { get; set; } = new();
}

public class UpdateSurt04CommandHandler : IRequestHandler<UpdateSurt04Command>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateSurt04CommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(UpdateSurt04Command request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserProfiles)
            .Include(u => u.UserOrgs)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

        if (user == null)
        {
             throw new Exception($"Entity \"{nameof(SuUser)}\" ({request.UserId}) was not found.");
        }

        user.Username = request.Username;
        user.Email = request.Email;
        user.EmailNormalized = request.Email.ToUpperInvariant();
        user.EmployeeId = request.EmployeeId;
        user.IsActive = request.IsActive;

        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            user.SecurityStamp = Guid.NewGuid().ToString();
        }

        // Update Profiles
        // 1. Remove profiles not in new list
        var profilesToRemove = user.UserProfiles
            .Where(up => !request.ProfileIds.Contains(up.ProfileId))
            .ToList();

        foreach (var profile in profilesToRemove)
        {
            user.UserProfiles.Remove(profile);
        }

        // 2. Add profiles in new list that are not existing
        var existingProfileIds = user.UserProfiles.Select(up => up.ProfileId).ToList();
        var profilesToAdd = request.ProfileIds
            .Where(id => !existingProfileIds.Contains(id))
            .ToList();

        foreach (var profileId in profilesToAdd)
        {
            user.UserProfiles.Add(new SuUserProfile
            {
                UserId = user.UserId,
                ProfileId = profileId
            });
        }

        // Update UserOrgs
        // 1. Remove orgs not in new list
        var orgsToRemove = user.UserOrgs
            .Where(uo => !request.UserOrgs.Select(r => r.OrgId).Contains(uo.OrgId))
            .ToList();

        foreach (var org in orgsToRemove)
        {
            user.UserOrgs.Remove(org);
        }

        // 2. Add/Update orgs
        foreach (var userOrgInput in request.UserOrgs)
        {
            var existingUserOrg = user.UserOrgs.FirstOrDefault(uo => uo.OrgId == userOrgInput.OrgId);
            if (existingUserOrg != null)
            {
                // Update IsDefault if changed
                existingUserOrg.IsDefault = userOrgInput.IsDefault;
            }
            else
            {
                // Add new
                user.UserOrgs.Add(new SuUserOrg
                {
                    UserId = user.UserId,
                    OrgId = userOrgInput.OrgId,
                    IsDefault = userOrgInput.IsDefault
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

// 3. Delete
public record DeleteSurt04Command(Guid Id) : IRequest;

public class DeleteSurt04CommandHandler : IRequestHandler<DeleteSurt04Command>
{
    private readonly IApplicationDbContext _context;

    public DeleteSurt04CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSurt04Command request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);

        if (user == null)
        {
             throw new Exception($"Entity \"{nameof(SuUser)}\" ({request.Id}) was not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
