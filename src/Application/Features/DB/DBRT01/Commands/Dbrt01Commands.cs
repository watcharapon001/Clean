using Application.Common.Abstractions;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DB.DBRT01.Commands;

// 1. Create
public record CreateDbrt01Command(
    string EmployeeCode,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    string? Email,
    string? Phone,
    bool IsActive
) : IRequest<string>;

public class CreateDbrt01CommandHandler : IRequestHandler<CreateDbrt01Command, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateDbrt01CommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(CreateDbrt01Command request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        if (string.IsNullOrEmpty(orgId)) throw new UnauthorizedAccessException("Org context missing");

        var entity = new DbEmployee
        {
            EmployeeId = Guid.NewGuid(),
            OrgId = Guid.Parse(orgId),
            EmployeeCode = request.EmployeeCode,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DisplayName = request.DisplayName,
            Email = request.Email,
            Phone = request.Phone,
            IsActive = request.IsActive
        };

        _context.DbEmployees.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.EmployeeId.ToString();
    }
}

// 2. Update
public record UpdateDbrt01Command(
    string EmployeeId,
    string EmployeeCode,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    string? Email,
    string? Phone,
    bool IsActive
) : IRequest;

public class UpdateDbrt01CommandHandler : IRequestHandler<UpdateDbrt01Command>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateDbrt01CommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateDbrt01Command request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        
        var entity = await _context.DbEmployees
            .FirstOrDefaultAsync(e => e.EmployeeId.ToString() == request.EmployeeId && e.OrgId.ToString() == orgId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("Employee not found");

        entity.EmployeeCode = request.EmployeeCode;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.DisplayName = request.DisplayName;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

// 3. Delete
public record DeleteDbrt01Command(string Id) : IRequest;

public class DeleteDbrt01CommandHandler : IRequestHandler<DeleteDbrt01Command>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteDbrt01CommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteDbrt01Command request, CancellationToken cancellationToken)
    {
        var orgId = _currentUserService.OrgId;
        
        var entity = await _context.DbEmployees
             .FirstOrDefaultAsync(e => e.EmployeeId.ToString() == request.Id && e.OrgId.ToString() == orgId, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("Employee not found");

        _context.DbEmployees.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
