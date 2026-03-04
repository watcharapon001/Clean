using FluentValidation;

namespace Application.Features.DB.DBRT01.Commands;

public class CreateDbrt01CommandValidator : AbstractValidator<CreateDbrt01Command>
{
    public CreateDbrt01CommandValidator()
    {
        RuleFor(v => v.EmployeeCode)
            .NotEmpty().WithMessage("EmployeeCode is required.")
            .MaximumLength(50).WithMessage("EmployeeCode must not exceed 50 characters.");

        RuleFor(v => v.FirstName)
            .MaximumLength(100).WithMessage("FirstName must not exceed 100 characters.");

        RuleFor(v => v.LastName)
            .MaximumLength(100).WithMessage("LastName must not exceed 100 characters.");

        RuleFor(v => v.DisplayName)
            .MaximumLength(150).WithMessage("DisplayName must not exceed 150 characters.");

        RuleFor(v => v.Email)
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(v => !string.IsNullOrEmpty(v.Email));

        RuleFor(v => v.Phone)
            .MaximumLength(50).WithMessage("Phone must not exceed 50 characters.");
    }
}
