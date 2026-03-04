using FluentValidation;

namespace Application.Features.SU.SURT02.Commands;

public class CreateSurt02CommandValidator : AbstractValidator<CreateSurt02Command>
{
    public CreateSurt02CommandValidator()
    {
        RuleFor(v => v.MenuCode)
            .NotEmpty().WithMessage("MenuCode is required.")
            .MaximumLength(20).WithMessage("MenuCode must not exceed 20 characters.");

        RuleFor(v => v.MenuName)
            .NotEmpty().WithMessage("MenuName is required.")
            .MaximumLength(200).WithMessage("MenuName must not exceed 200 characters.");

        RuleFor(v => v.Route)
            .MaximumLength(200).WithMessage("Route must not exceed 200 characters.");

        RuleFor(v => v.Icon)
            .MaximumLength(50).WithMessage("Icon must not exceed 50 characters.");
    }
}
