using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT03.Queries;

public record GetSurt03Query(Guid ProfileId) : IRequest<List<Surt03Dto>>;

public class GetSurt03QueryHandler : IRequestHandler<GetSurt03Query, List<Surt03Dto>>
{
    private readonly IApplicationDbContext _context;

    public GetSurt03QueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Surt03Dto>> Handle(GetSurt03Query request, CancellationToken cancellationToken)
    {
        // 1. Get all active menus
        var menus = await _context.Menus
            .AsNoTracking()
            .Include(m => m.ParentMenu)
            .Where(m => m.IsActive)
            .OrderBy(m => m.Sequence)
            .ToListAsync(cancellationToken);

        // 2. Get existing permissions for checking profile
        var permissions = await _context.ProfileMenus
            .AsNoTracking()
            .Where(pm => pm.ProfileId == request.ProfileId)
            .ToDictionaryAsync(pm => pm.MenuId, cancellationToken);

        // 3. Merge
        var result = new List<Surt03Dto>();
        foreach (var menu in menus)
        {
            var dto = new Surt03Dto
            {
                MenuId = menu.MenuId,
                MenuCode = menu.MenuCode,
                MenuName = menu.MenuName,
                ParentMenuId = menu.ParentMenuId,
                ParentMenuName = menu.ParentMenu?.MenuName,
                Sequence = menu.Sequence,
                
                // Set permissions if exists, otherwise false
                CanView = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanView,
                CanCreate = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanCreate,
                CanEdit = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanEdit,
                CanDelete = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanDelete
            };
            result.Add(dto);
        }

        return result;
    }
}
