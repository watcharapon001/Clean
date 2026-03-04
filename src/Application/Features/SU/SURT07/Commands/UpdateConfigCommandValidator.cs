using FluentValidation;

namespace Application.Features.SU.SURT07.Commands;

public class UpdateConfigCommandValidator : AbstractValidator<UpdateConfigCommand>
{
    public UpdateConfigCommandValidator()
    {
        RuleFor(v => v.ConfigKey)
            .NotEmpty().WithMessage("ConfigKey is required.")
            .MaximumLength(50).WithMessage("ConfigKey must not exceed 50 characters.");

        RuleFor(v => v.ConfigValue)
            .NotEmpty().WithMessage("ConfigValue is required.")
            .MaximumLength(500).WithMessage("ConfigValue must not exceed 500 characters.");
    }
}
