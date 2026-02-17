using FluentValidation;
using Application.Features.DB.DBRT01.Commands;

namespace Application.Features.DB.DBRT01.Commands.CreateEmployee;

public class CreateDbrt01CommandValidator : AbstractValidator<CreateDbrt01Command>
{
    public CreateDbrt01CommandValidator()
    {
        RuleFor(v => v.EmployeeCode)
            .NotEmpty().WithMessage("Employee Code is required.")
            .MaximumLength(20).WithMessage("Employee Code must not exceed 20 characters.");

        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(100).WithMessage("First Name must not exceed 100 characters.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(100).WithMessage("Last Name must not exceed 100 characters.");

        RuleFor(v => v.Email)
             .EmailAddress().WithMessage("A valid email address is required.")
             .When(v => !string.IsNullOrEmpty(v.Email));
    }
}
