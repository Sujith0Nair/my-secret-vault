using SecretVault.Domain;

namespace SecretVault.Application.Interfaces;

public interface ISharingRepository
{
    public Task<List<SharePermission>> GetPermissionsForUserAsync(Guid userId);
    public Task<SharePermission?> GetPermissionByIdAsync(Guid permissionId);
    public Task<SharePermission?> GetPermissionForSecretAndUserAsync(Guid secretId, Guid sharedUserId);
    public Task AddAsync(SharePermission sharePermission);
    public void Update(SharePermission sharePermission);
    public void Delete(SharePermission sharePermission);
}