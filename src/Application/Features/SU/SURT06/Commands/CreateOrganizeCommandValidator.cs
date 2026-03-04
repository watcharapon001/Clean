using FluentValidation;

namespace Application.Features.SU.SURT06.Commands;

public class CreateOrganizeCommandValidator : AbstractValidator<CreateOrganizeCommand>
{
    public CreateOrganizeCommandValidator()
    {
        RuleFor(v => v.OrgCode)
            .NotEmpty().WithMessage("OrgCode is required.")
            .MaximumLength(20).WithMessage("OrgCode must not exceed 20 characters.");

        RuleFor(v => v.OrgName)
            .NotEmpty().WithMessage("OrgName is required.")
            .MaximumLength(200).WithMessage("OrgName must not exceed 200 characters.");
    }
}
