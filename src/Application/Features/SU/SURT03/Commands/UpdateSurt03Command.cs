using Application.Common.Abstractions;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT03.Commands;

public record PermissionDto
{
    public Guid MenuId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}

public record UpdateSurt03Command : IRequest
{
    public Guid ProfileId { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new();
}

public class UpdateSurt03CommandHandler : IRequestHandler<UpdateSurt03Command>
{
    private readonly IApplicationDbContext _context;

    public UpdateSurt03CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSurt03Command request, CancellationToken cancellationToken)
    {
        // 1. Get existing permissions
        var existingPermissions = await _context.ProfileMenus
            .Where(pm => pm.ProfileId == request.ProfileId)
            .ToListAsync(cancellationToken);

        _context.ProfileMenus.RemoveRange(existingPermissions);

        // 2. Add new permissions (only if CanView is true or any other permission is true, usually CanView is required)
        // Or blindly add all provided, relying on frontend to send correct data.
        
        var newPermissions = request.Permissions.Select(p => new SuProfileMenu
        {
            ProfileId = request.ProfileId,
            MenuId = p.MenuId,
            CanView = p.CanView,
            CanCreate = p.CanCreate,
            CanEdit = p.CanEdit,
            CanDelete = p.CanDelete
        }).ToList();

        await _context.ProfileMenus.AddRangeAsync(newPermissions);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
