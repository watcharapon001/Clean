using FluentValidation;
using Application.Features.DB.DBRT01.Commands;

namespace Application.Features.DB.DBRT01.Commands.UpdateEmployee;

public class UpdateDbrt01CommandValidator : AbstractValidator<UpdateDbrt01Command>
{
    public UpdateDbrt01CommandValidator()
    {
        RuleFor(v => v.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId is required.");

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
