using Application.Common.Abstractions;
using Application.Features.Auth.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Queries.GetOrgs;

public record GetOrgsQuery : IRequest<List<OrgDto>>;

public class GetOrgsQueryHandler : IRequestHandler<GetOrgsQuery, List<OrgDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetOrgsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<OrgDto>> Handle(GetOrgsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId)) return new List<OrgDto>();

        return await _context.Users
            .Where(u => u.UserId.ToString() == userId)
            .SelectMany(u => u.UserOrgs)
            .Where(uo => uo.IsActive && uo.Org.IsActive)
            .Select(uo => new OrgDto(
                uo.OrgId.ToString(),
                uo.Org.OrgCode,
                uo.Org.OrgName,
                uo.IsDefault))
            .ToListAsync(cancellationToken);
    }
}
