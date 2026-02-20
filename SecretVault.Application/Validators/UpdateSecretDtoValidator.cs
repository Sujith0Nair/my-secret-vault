using FluentValidation;
using JetBrains.Annotations;
using SecretVault.Application.DTOs;

namespace SecretVault.Application.Validators;

[UsedImplicitly]
public class UpdateSecretDtoValidator : AbstractValidator<UpdateSecretDto>
{
    public UpdateSecretDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
            
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Secret content cannot be empty.");
    }
}