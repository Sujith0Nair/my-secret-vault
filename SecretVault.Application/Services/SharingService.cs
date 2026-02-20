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
        await unitOfWork.SaveChangesAsync();

        var newPermission = secret.SharePermissions.First(p => p.SharedWithUserId == userToShareWith.Id);
        return PermissionDto.ToPermissionDto(newPermission, userToShareWith);
    }

    public async Task UpdateSharePermissionAsync(Guid secretId, Guid ownerUserId, Guid sharedWithUserId,
        UpdatePermissionDto updatePermissionDto)
    {
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEmptyGuid(ownerUserId, nameof(ownerUserId));
        Guard.AgainstEmptyGuid(sharedWithUserId, nameof(sharedWithUserId));
        Guard.AgainstNullObject(updatePermissionDto, nameof(updatePermissionDto));

        if (updatePermissionDto.AccessLevel is >= AccessLevel.Owner or AccessLevel.None)
        {
            throw new ArgumentException("New access level for updating permission must be View, Edit or Delete", nameof(updatePermissionDto.AccessLevel));
        }
        
        var secret = await unitOfWork.SecretRepository.GetByIdAsync(secretId);
        if (secret is null)
        {
            throw new InvalidOperationException($"Secret with id {secretId} not found");
        }

        if (secret.OwnerUserId != ownerUserId)
        {
            throw new UnauthorizedAccessException($"User '{ownerUserId}' is not the owner of the secret '{secretId}'");
        }
        
        secret.UpdateSharePermission(sharedWithUserId, updatePermissionDto.AccessLevel);
        
        await unitOfWork.SaveChangesAsync();
    }

    public async Task RevokeSharePermissionAsync(Guid secretId, Guid ownerUserId, Guid sharedWithUserId)
    {
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEmptyGuid(ownerUserId, nameof(ownerUserId));
        Guard.AgainstEmptyGuid(sharedWithUserId, nameof(sharedWithUserId));
        
        var secret = await unitOfWork.SecretRepository.GetByIdAsync(secretId);
        if (secret is null)
        {
            throw new InvalidOperationException($"Secret with id {secretId} not found");
        }

        if (secret.OwnerUserId != ownerUserId)
        {
            throw new UnauthorizedAccessException($"User '{ownerUserId}' is not the owner of the secret '{secretId}'");
        }
        
        secret.RemoveSharePermission(sharedWithUserId);
        
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsForSecretAsync(Guid secretId, Guid ownerUserId)
    {
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEmptyGuid(ownerUserId, nameof(ownerUserId));
        
        var secret = await unitOfWork.SecretRepository.GetByIdAsync(secretId);
        if (secret is null)
        {
            throw new InvalidOperationException($"Secret with id {secretId} not found");
        }

        if (secret.OwnerUserId != ownerUserId)
        {
            throw new UnauthorizedAccessException(
                $"User '{ownerUserId}' is not the owner of the secret '{secretId}' and cannot view its share permissions.");
        }

        var permissionDtos = new List<PermissionDto>();
        foreach (var permission in secret.SharePermissions)
        {
            var sharedWithUser = await unitOfWork.UserRepository.GetByIdAsync(permission.SharedWithUserId);
            permissionDtos.Add(PermissionDto.ToPermissionDto(permission, sharedWithUser));
        }

        return permissionDtos;
    }
}