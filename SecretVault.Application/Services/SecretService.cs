using SecretVault.Domain;
using SecretVault.Application.DTOs;
using SecretVault.Domain.Extensions;
using SecretVault.Application.Interfaces;

namespace SecretVault.Application.Services;

public class SecretService(IUnitOfWork unitOfWork, IEncryptionService encryptionService)
    : ISecretService
{
    public async Task<SecretDto> CreateSecretAsync(Guid ownerUserId, CreateSecretDto createSecretDto)
    {
        Guard.AgainstEmptyGuid(ownerUserId, nameof(ownerUserId));
        Guard.AgainstNullObject(createSecretDto, nameof(createSecretDto));
        
        var encryptedContent = encryptionService.Encrypt(createSecretDto.Content);
        var newSecret = new Secret(ownerUserId, createSecretDto.Title, encryptedContent);
        await unitOfWork.SecretRepository.AddAsync(newSecret);
        await unitOfWork.SaveChangesAsync();
        return SecretDto.ToSecretDto(newSecret, createSecretDto.Content);
    }

    public async Task<SecretDto> GetSecretAsync(Guid secretId, Guid requesterUserId)
    {
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEmptyGuid(requesterUserId, nameof(requesterUserId));
        
        var secret = await unitOfWork.SecretRepository.GetByIdAsync(secretId);
        if (secret == null)
        {
            throw new InvalidOperationException($"Secret with id {secretId} not found");
        }
        
        var accessLevel = GetAccessLevel(secret, requesterUserId);
        if (accessLevel < AccessLevel.View)
        {
            throw new UnauthorizedAccessException(
                $"User '{requesterUserId}' does not have permission to view the secret '{secretId}'");
        }

        var decryptedContent = encryptionService.Decrypt(secret.EncryptedContent);
        return SecretDto.ToSecretDto(secret, decryptedContent);
    }

    public async Task<IEnumerable<SecretListItemDto>> GetAllSecretsAsync(Guid userId)
    {
        Guard.AgainstEmptyGuid(userId, nameof(userId));
        var secretsDictionary = new Dictionary<Guid, SecretListItemDto>();
        
        var ownedSecrets = await unitOfWork.SecretRepository.GetByOwnerIdAsync(userId);
        foreach (var secret in ownedSecrets)
        {
            secretsDictionary[secret.Id] = SecretListItemDto.ToSecretListItemDto(secret, true, AccessLevel.Owner);
        }

        var sharedPermissions = await unitOfWork.SharingRepository.GetPermissionsForUserAsync(userId);
        foreach (var permission in sharedPermissions)
        {
            if (secretsDictionary.ContainsKey(permission.SecretId))
                continue;
            
            var secret = await unitOfWork.SecretRepository.GetByIdAsync(permission.SecretId);
            if (secret == null) continue;
            
            secretsDictionary[permission.SecretId] = SecretListItemDto.ToSecretListItemDto(secret, false, permission.AccessLevel);
        }
        
        return secretsDictionary.Values.OrderByDescending(s => s.LastModifiedAt);
    }

    public async Task UpdateSecretAsync(Guid secretId, Guid requesterUserId, UpdateSecretDto updateSecretDto)
    {
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEmptyGuid(requesterUserId, nameof(requesterUserId));
        Guard.AgainstNullObject(updateSecretDto, nameof(updateSecretDto));
        
        var secret = await unitOfWork.SecretRepository.GetByIdAsync(secretId);
        if (secret == null)
        {
            throw new InvalidOperationException($"Secret with id {secretId} not found");
        }
        
        var accessLevel = GetAccessLevel(secret, requesterUserId);
        if (accessLevel < AccessLevel.Edit)
        {
            throw new UnauthorizedAccessException(
                $"User '{requesterUserId}' does not have permission to update the secret '{secretId}'");
        }
        
        var newEncryptedContent = encryptionService.Encrypt(updateSecretDto.Content);
        secret.UpdateSecret(updateSecretDto.Title, newEncryptedContent);
        unitOfWork.SecretRepository.Update(secret);
        
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteSecretAsync(Guid secretId, Guid requesterUserId)
    {
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEmptyGuid(requesterUserId, nameof(requesterUserId));
        
        var secret = await unitOfWork.SecretRepository.GetByIdAsync(secretId);
        if (secret == null)
        {
            throw new InvalidOperationException($"Secret with id {secretId} not found");
        }
        
        var accessLevel = GetAccessLevel(secret, requesterUserId);
        if (accessLevel < AccessLevel.Delete)
        {
            throw new UnauthorizedAccessException($"User '{requesterUserId}' does not have permission to delete the secret '{secretId}'");
        }
        
        unitOfWork.SecretRepository.Delete(secret);
        await unitOfWork.SaveChangesAsync();
    }

    private static AccessLevel GetAccessLevel(Secret secret, Guid requesterUserId)
    {
        if (secret.OwnerUserId == requesterUserId)
        {
            return AccessLevel.Owner;
        }
        
        var permission = secret.SharePermissions.FirstOrDefault(p => p.SharedWithUserId == requesterUserId);
        return permission?.AccessLevel ?? AccessLevel.None;
    }
}