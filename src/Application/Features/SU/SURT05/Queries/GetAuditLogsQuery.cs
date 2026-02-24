using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT05.Queries;

public record AuditLogDto(
    Guid AuditLogId,
    string? UserId,
    string Action,
    string TableName,
    string? KeyValues,
    string? OldValues,
    string? NewValues,
    DateTimeOffset Timestamp
);

public record GetAuditLogsQuery : IRequest<List<AuditLogDto>>;

public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, List<AuditLogDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAuditLogsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuditLogDto>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
    {
        return await _context.SuAuditLogs
            .OrderByDescending(a => a.Timestamp)
            .Take(500)
            .Select(a => new AuditLogDto(
                a.AuditLogId,
                a.UserId,
                a.Action,
                a.TableName,
                a.KeyValues,
                a.OldValues,
                a.NewValues,
                a.Timestamp
            ))
            .ToListAsync(cancellationToken);
    }
}
