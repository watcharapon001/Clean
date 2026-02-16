namespace Application.Features.DB.Employees.Queries;

public record EmployeeDto(
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
