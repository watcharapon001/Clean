using Application.Common.Abstractions;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT03.Queries;

public record ExportSurt03Query(Guid ProfileId) : IRequest<byte[]>;

public class ExportSurt03QueryHandler : IRequestHandler<ExportSurt03Query, byte[]>
{
    private readonly IApplicationDbContext _context;

    public ExportSurt03QueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> Handle(ExportSurt03Query request, CancellationToken cancellationToken)
    {
        var profile = await _context.Profiles.FindAsync(new object[] { request.ProfileId }, cancellationToken);
        if (profile == null) throw new Exception("Profile not found.");

        var menus = await _context.Menus
            .AsNoTracking()
            .Include(m => m.ParentMenu)
            .Where(m => m.IsActive)
            .OrderBy(m => m.Sequence)
            .ToListAsync(cancellationToken);

        var permissions = await _context.ProfileMenus
            .AsNoTracking()
            .Where(pm => pm.ProfileId == request.ProfileId)
            .ToDictionaryAsync(pm => pm.MenuId, cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Permissions");

        worksheet.Cell(1, 1).Value = $"Permissions for Profile: {profile.ProfileName}";
        worksheet.Range(1, 1, 1, 7).Merge().Style.Font.Bold = true;

        // Headers
        worksheet.Cell(3, 1).Value = "Menu Code";
        worksheet.Cell(3, 2).Value = "Menu Name";
        worksheet.Cell(3, 3).Value = "Parent Menu";
        worksheet.Cell(3, 4).Value = "View";
        worksheet.Cell(3, 5).Value = "Create";
        worksheet.Cell(3, 6).Value = "Edit";
        worksheet.Cell(3, 7).Value = "Delete";

        // Formatting headers
        var headerRange = worksheet.Range(3, 1, 3, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        // Data
        int row = 4;
        foreach (var menu in menus)
        {
            worksheet.Cell(row, 1).Value = menu.MenuCode;
            worksheet.Cell(row, 2).Value = menu.MenuName;
            worksheet.Cell(row, 3).Value = menu.ParentMenu?.MenuName ?? "-";

            bool canView = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanView;
            bool canCreate = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanCreate;
            bool canEdit = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanEdit;
            bool canDelete = permissions.ContainsKey(menu.MenuId) && permissions[menu.MenuId].CanDelete;

            worksheet.Cell(row, 4).Value = canView ? "Yes" : "No";
            worksheet.Cell(row, 5).Value = canCreate ? "Yes" : "No";
            worksheet.Cell(row, 6).Value = canEdit ? "Yes" : "No";
            worksheet.Cell(row, 7).Value = canDelete ? "Yes" : "No";
            
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
