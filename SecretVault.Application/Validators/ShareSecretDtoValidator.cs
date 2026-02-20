using FluentValidation;
using SecretVault.Domain;
using JetBrains.Annotations;
using SecretVault.Application.DTOs;

namespace SecretVault.Application.Validators;

[UsedImplicitly]
public class ShareSecretDtoValidator : AbstractValidator<ShareSecretDto>
{
    public ShareSecretDtoValidator()
    {
        RuleFor(x => x.ShareWithEmail)
            .NotEmpty().WithMessage("ShareWithEmail cannot be empty.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.AccessLevel)
            .IsInEnum()
            .Must(x => x is > AccessLevel.None and < AccessLevel.Owner)
            .WithMessage("Access level for sharing must be View, Edit, or Delete.");
    }
}