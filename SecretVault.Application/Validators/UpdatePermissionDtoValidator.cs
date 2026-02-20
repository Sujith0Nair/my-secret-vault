using FluentValidation;
using SecretVault.Domain;
using JetBrains.Annotations;
using SecretVault.Application.DTOs;

namespace SecretVault.Application.Validators;

[UsedImplicitly]
public class UpdatePermissionDtoValidator : AbstractValidator<UpdatePermissionDto>
{
    public UpdatePermissionDtoValidator()
    {
        RuleFor(x => x.AccessLevel)
            .IsInEnum()
            .Must(x => x is > AccessLevel.None and < AccessLevel.Owner)
            .WithMessage("Access level for permissions must be View, Edit, or Delete.");
    }
}
