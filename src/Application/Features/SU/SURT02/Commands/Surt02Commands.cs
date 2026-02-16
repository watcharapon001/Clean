using Application.Common.Abstractions;
using Domain.Entities.SU;
using MediatR;

namespace Application.Features.SU.SURT02.Commands;

// 1. Create
public record CreateSurt02Command : IRequest<Guid>
{
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSurt02CommandHandler : IRequestHandler<CreateSurt02Command, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSurt02CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSurt02Command request, CancellationToken cancellationToken)
    {
        var entity = new SuMenu
        {
            MenuId = Guid.NewGuid(),
            MenuCode = request.MenuCode,
            MenuName = request.MenuName,
            Route = request.Route,
            Icon = request.Icon,
            Sequence = request.Sequence,
            ParentMenuId = request.ParentMenuId,
            IsActive = request.IsActive
        };

        _context.Menus.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.MenuId;
    }
}

// 2. Update
public record UpdateSurt02Command : IRequest
{
    public Guid MenuId { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? Route { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateSurt02CommandHandler : IRequestHandler<UpdateSurt02Command>
{
    private readonly IApplicationDbContext _context;

    public UpdateSurt02CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSurt02Command request, CancellationToken cancellationToken)
    {
        var entity = await _context.Menus.FindAsync(new object[] { request.MenuId }, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity \"{nameof(SuMenu)}\" ({request.MenuId}) was not found.");
        }

        entity.MenuCode = request.MenuCode;
        entity.MenuName = request.MenuName;
        entity.Route = request.Route;
        entity.Icon = request.Icon;
        entity.Sequence = request.Sequence;
        entity.ParentMenuId = request.ParentMenuId;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

// 3. Delete
public record DeleteSurt02Command(Guid Id) : IRequest;

public class DeleteSurt02CommandHandler : IRequestHandler<DeleteSurt02Command>
{
    private readonly IApplicationDbContext _context;

    public DeleteSurt02CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSurt02Command request, CancellationToken cancellationToken)
    {
        var entity = await _context.Menus.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
             throw new Exception($"Entity \"{nameof(SuMenu)}\" ({request.Id}) was not found.");
        }

        _context.Menus.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
