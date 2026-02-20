using FluentValidation;
using JetBrains.Annotations;
using SecretVault.Application.DTOs;

namespace SecretVault.Application.Validators;

[UsedImplicitly]
public class CreateSecretDtoValidator : AbstractValidator<CreateSecretDto>
{
    public CreateSecretDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Secret content cannot be empty.");
    }
}