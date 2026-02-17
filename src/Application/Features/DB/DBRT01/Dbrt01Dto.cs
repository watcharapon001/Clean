namespace Application.Features.DB.DBRT01;

public record Dbrt01Dto(
    string EmployeeId,
    string OrgId,
    string EmployeeCode,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    string? Email,
    string? Phone,
    bool IsActive
);
