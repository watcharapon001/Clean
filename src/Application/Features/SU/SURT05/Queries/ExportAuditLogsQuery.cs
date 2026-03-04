using Application.Common.Abstractions;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.SU.SURT05.Queries;

public record ExportAuditLogsQuery : IRequest<byte[]>;

public class ExportAuditLogsQueryHandler : IRequestHandler<ExportAuditLogsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;

    public ExportAuditLogsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> Handle(ExportAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _context.SuAuditLogs
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

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Audit Logs");

        worksheet.Cell(1, 1).Value = "No.";
        worksheet.Cell(1, 2).Value = "Timestamp";
        worksheet.Cell(1, 3).Value = "Action";
        worksheet.Cell(1, 4).Value = "Table Name";
        worksheet.Cell(1, 5).Value = "User ID";
        worksheet.Cell(1, 6).Value = "Key Values";

        var headerRange = worksheet.Range(1, 1, 1, 6);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 2;
        foreach (var log in logs)
        {
            worksheet.Cell(row, 1).Value = row - 1;
            worksheet.Cell(row, 2).Value = log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cell(row, 3).Value = log.Action;
            worksheet.Cell(row, 4).Value = log.TableName;
            worksheet.Cell(row, 5).Value = log.UserId;
            worksheet.Cell(row, 6).Value = log.KeyValues;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
