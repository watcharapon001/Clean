using FluentValidation;

namespace Application.Features.SU.SURT06.Commands;

public class UpdateOrganizeCommandValidator : AbstractValidator<UpdateOrganizeCommand>
{
    public UpdateOrganizeCommandValidator()
    {
        RuleFor(v => v.OrgId)
            .NotEmpty().WithMessage("OrgId is required.");

        RuleFor(v => v.OrgName)
            .NotEmpty().WithMessage("OrgName is required.")
            .MaximumLength(200).WithMessage("OrgName must not exceed 200 characters.");
    }
}
