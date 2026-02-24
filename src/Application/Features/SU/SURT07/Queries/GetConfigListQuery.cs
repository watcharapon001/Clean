using Application.Common.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT07.Queries;

public record ConfigDto(
    string ConfigKey,
    string ConfigValue,
    string? Description,
    string DataType
);

public record GetConfigListQuery : IRequest<List<ConfigDto>>;

public class GetConfigListQueryHandler : IRequestHandler<GetConfigListQuery, List<ConfigDto>>
{
    private readonly IApplicationDbContext _context;

    public GetConfigListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ConfigDto>> Handle(GetConfigListQuery request, CancellationToken cancellationToken)
    {
        return await _context.SuConfigs
            .OrderBy(c => c.ConfigKey)
            .Select(c => new ConfigDto(
                c.ConfigKey,
                c.ConfigValue,
                c.Description,
                c.DataType
            ))
            .ToListAsync(cancellationToken);
    }
}
