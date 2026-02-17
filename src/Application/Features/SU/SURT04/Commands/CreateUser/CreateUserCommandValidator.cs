using FluentValidation;
using Application.Features.SU.SURT04.Commands;

namespace Application.Features.SU.SURT04.Commands.CreateUser;

public class CreateSurt04CommandValidator : AbstractValidator<CreateSurt04Command>
{
    public CreateSurt04CommandValidator()
    {
        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
