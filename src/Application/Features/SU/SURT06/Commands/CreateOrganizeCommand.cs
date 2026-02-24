using Application.Common.Abstractions;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT06.Commands;

public record CreateOrganizeCommand(
    string OrgCode,
    string OrgName,
    bool IsActive
) : IRequest<Guid>;

public class CreateOrganizeCommandHandler : IRequestHandler<CreateOrganizeCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateOrganizeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateOrganizeCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Organizes.AnyAsync(o => o.OrgCode == request.OrgCode, cancellationToken);
        if (exists)
            throw new InvalidOperationException($"Organization code '{request.OrgCode}' already exists.");

        var organize = new SuOrganize
        {
            OrgCode = request.OrgCode,
            OrgName = request.OrgName,
            IsActive = request.IsActive
        };

        _context.Organizes.Add(organize);
        await _context.SaveChangesAsync(cancellationToken);

        return organize.OrgId;
    }
}
