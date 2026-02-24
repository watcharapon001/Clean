using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.DASH01.Queries;

public record DashboardMetricsDto(
    int TotalUsers,
    int TotalOrganizes,
    int TotalProfiles,
    int TotalMenus,
    List<RecentAuditDto> RecentAudits
);

public record RecentAuditDto(
    string Action,
    string TableName,
    DateTimeOffset Timestamp
);

public record GetDashboardMetricsQuery : IRequest<DashboardMetricsDto>;

public class GetDashboardMetricsQueryHandler : IRequestHandler<GetDashboardMetricsQuery, DashboardMetricsDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardMetricsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardMetricsDto> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
    {
        var totalUsers = await _context.Users.CountAsync(cancellationToken);
        var totalOrganizes = await _context.Organizes.CountAsync(cancellationToken);
        var totalProfiles = await _context.Profiles.CountAsync(cancellationToken);
        var totalMenus = await _context.Menus.CountAsync(cancellationToken);

        var recentAudits = await _context.SuAuditLogs
            .OrderByDescending(a => a.Timestamp)
            .Take(5)
            .Select(a => new RecentAuditDto(a.Action, a.TableName, a.Timestamp))
            .ToListAsync(cancellationToken);

        return new DashboardMetricsDto(
            totalUsers,
            totalOrganizes,
            totalProfiles,
            totalMenus,
            recentAudits
        );
    }
}
