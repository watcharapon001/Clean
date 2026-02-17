using FluentValidation;
using Application.Features.SU.SURT04.Commands;

namespace Application.Features.SU.SURT04.Commands.UpdateUser;

public class UpdateSurt04CommandValidator : AbstractValidator<UpdateSurt04Command>
{
    public UpdateSurt04CommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");
    }
}
