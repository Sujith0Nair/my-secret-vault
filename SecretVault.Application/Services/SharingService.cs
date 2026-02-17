using SecretVault.Domain;
using SecretVault.Application.DTOs;
using SecretVault.Domain.Extensions;
using SecretVault.Application.Interfaces;

namespace SecretVault.Application.Services;

public class SharingService(IUnitOfWork unitOfWork) : ISharingService
{   
    public async Task<PermissionDto> ShareSecretAsync(Guid secretId, Guid ownerUserId, ShareSecretDto shareSecretDto)
    {
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEmptyGuid(ownerUserId, nameof(ownerUserId));
        Guard.AgainstNullObject(shareSecretDto, nameof(shareSecretDto));
        Guard.AgainstNullOrWhiteSpace(shareSecretDto.ShareWithEmail, nameof(shareSecretDto.ShareWithEmail));

        if (shareSecretDto.AccessLevel is >= AccessLevel.Owner or AccessLevel.None)
        {
            throw new ArgumentException($"Access level for sharing must be View, Edit or Delete", nameof(shareSecretDto.AccessLevel));
        }
        
        var userToShareWith = await unitOfWork.UserRepository.GetByEmailAsync(shareSecretDto.ShareWithEmail);
        if (userToShareWith is null)
        {
            throw new InvalidOperationException($"User with email {shareSecretDto.ShareWithEmail} not found");
        }

        if (userToShareWith.Id == ownerUserId)
        {
            throw new InvalidOperationException("Owner cannot share a secret with themselves");
        }
        
        var secret = await unitOfWork.SecretRepository.GetByIdAsync(secretId);
        if (secret is null)
        {
            throw new InvalidOperationException($"Secret with id {secretId} not found");
        }

        if (secret.OwnerUserId != ownerUserId)
        {
            throw new UnauthorizedAccessException(
                $"User '{ownerUserId}' is not the owner of the secret '{secretId}' and cannot share it");
        }
        
        secret.AddSharePermission(userToShareWith.Id, shareSecretDto.AccessLevel);
        unitOfWork.SecretRepository.Update(secret);

        await unitOfWork.SaveChangesAsync();

        var newPermission = secret.SharePermissions.First(p => p.SharedWithUserId == ownerUserId);
        return PermissionDto.ToPermissionDto(newPermission, userToShareWith);
    }

    public async Task UpdateSharePermissionAsync(Guid secretId, Guid ownerUserId, Guid sharedWithUserId,
        UpdatePermissionDto updatePermissionDto)
    {
        throw new NotImplementedException();
    }

    public async Task RevokeSharePermissionAsync(Guid secretId, Guid ownerUserId, Guid sharedWithUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsForSecretAsync(Guid secretId, Guid ownerUserId)
    {
        throw new NotImplementedException();
    }
}