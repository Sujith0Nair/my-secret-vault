using SecretVault.Application.DTOs;

namespace SecretVault.Application.Interfaces;

public interface ISharingService
{
    public Task<PermissionDto> ShareSecretAsync(Guid secretId, Guid ownerUserId, ShareSecretDto shareSecretDto);
    public Task UpdateSharePermissionAsync(Guid secretId, Guid ownerUserId, Guid sharedWithUserId, UpdatePermissionDto updatePermissionDto);
    public Task RevokeSharePermissionAsync(Guid secretId, Guid ownerUserId, Guid sharedWithUserId);
    public Task<IEnumerable<PermissionDto>> GetPermissionsForSecretAsync(Guid secretId, Guid ownerUserId);
}