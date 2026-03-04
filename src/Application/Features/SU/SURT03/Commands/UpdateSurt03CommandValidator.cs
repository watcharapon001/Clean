using FluentValidation;

namespace Application.Features.SU.SURT03.Commands;

public class UpdateSurt03CommandValidator : AbstractValidator<UpdateSurt03Command>
{
    public UpdateSurt03CommandValidator()
    {
        RuleFor(v => v.ProfileId)
            .NotEmpty().WithMessage("ProfileId is required.");

        RuleFor(v => v.Permissions)
            .NotNull().WithMessage("Permissions list cannot be null.");
    }
}
