using FluentValidation;

namespace Application.Features.SU.SURT04.Commands;

public class UpdateSurt04CommandValidator : AbstractValidator<UpdateSurt04Command>
{
    public UpdateSurt04CommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

        RuleFor(v => v.Password)
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
            .When(v => !string.IsNullOrEmpty(v.Password));

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");
    }
}
