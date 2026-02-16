using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.Menus.Queries.GetMenus;

public record GetMenusQuery : IRequest<List<MenuDto>>;

public class GetMenusQueryHandler : IRequestHandler<GetMenusQuery, List<MenuDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMenusQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<MenuDto>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return new List<MenuDto>();
        }

        if (!Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return new List<MenuDto>();
        }

        // Get user profiles
        var profileIds = await _context.UserProfiles
            .Where(up => up.UserId == userId)
            .Select(up => up.ProfileId)
            .ToListAsync(cancellationToken);

        if (!profileIds.Any())
        {
            return new List<MenuDto>();
        }

        // Get authorized menus
        // We select the distinct menus and merge permissions if a user has multiple profiles
        // (Assuming optimistic merging: if one profile allows, it is allowed)
        
        var authorizedMenus = await _context.ProfileMenus
            .Where(pm => profileIds.Contains(pm.ProfileId) && pm.CanView && pm.Menu.IsActive)
            .Include(pm => pm.Menu)
            .Select(pm => new 
            {
                pm.Menu,
                pm.CanCreate,
                pm.CanEdit,
                pm.CanDelete
            })
            .ToListAsync(cancellationToken);

        var menuDtos = authorizedMenus
            .GroupBy(x => x.Menu.MenuId)
            .Select(g => new MenuDto
            {
                MenuId = g.Key,
                MenuCode = g.First().Menu.MenuCode,
                MenuName = g.First().Menu.MenuName,
                Route = g.First().Menu.Route,
                Icon = g.First().Menu.Icon,
                Sequence = g.First().Menu.Sequence,
                ParentMenuId = g.First().Menu.ParentMenuId,
                
                // Merge permissions: if any profile grants permission, it's true
                CanCreate = g.Any(x => x.CanCreate),
                CanEdit = g.Any(x => x.CanEdit),
                CanDelete = g.Any(x => x.CanDelete)
            })
            .OrderBy(m => m.Sequence)
            .ToList();

        // Build hierarchy
        var rootMenus = menuDtos.Where(m => m.ParentMenuId == null).ToList();
        
        foreach (var rootMenu in rootMenus)
        {
            AddChildren(rootMenu, menuDtos);
        }

        return rootMenus;
    }

    private void AddChildren(MenuDto parent, List<MenuDto> allMenus)
    {
        parent.Children = allMenus
            .Where(m => m.ParentMenuId == parent.MenuId)
            .OrderBy(m => m.Sequence)
            .ToList();

        foreach (var child in parent.Children)
        {
            AddChildren(child, allMenus);
        }
    }
}
