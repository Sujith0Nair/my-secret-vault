using FluentValidation;
using JetBrains.Annotations;
using SecretVault.Application.DTOs;

namespace SecretVault.Application.Validators;

[UsedImplicitly]
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters.")
            .MaximumLength(50).WithMessage("Password must be less than 50 characters.");
    }
}