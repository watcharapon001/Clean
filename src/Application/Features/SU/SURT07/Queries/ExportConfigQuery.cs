using Application.Common.Abstractions;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT07.Queries;

public record ExportConfigQuery : IRequest<byte[]>
{
    public string? Keyword { get; init; }
}

public class ExportConfigQueryHandler : IRequestHandler<ExportConfigQuery, byte[]>
{
    private readonly IApplicationDbContext _context;

    public ExportConfigQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> Handle(ExportConfigQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SuConfigs.AsNoTracking();

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(c => c.ConfigKey.Contains(request.Keyword) ||
                                     c.Description!.Contains(request.Keyword));
        }

        var configs = await query.OrderBy(c => c.ConfigKey).ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Configurations");

        // Headers
        worksheet.Cell(1, 1).Value = "No.";
        worksheet.Cell(1, 2).Value = "Configuration Key";
        worksheet.Cell(1, 3).Value = "Value";
        worksheet.Cell(1, 4).Value = "Description";

        // Formatting headers
        var headerRange = worksheet.Range(1, 1, 1, 4);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        // Data
        int row = 2;
        foreach (var config in configs)
        {
            worksheet.Cell(row, 1).Value = row - 1;
            worksheet.Cell(row, 2).Value = config.ConfigKey;
            worksheet.Cell(row, 3).Value = config.ConfigValue;
            worksheet.Cell(row, 4).Value = config.Description;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
