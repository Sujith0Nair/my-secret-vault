using SecretVault.Domain;

namespace SecretVault.Application.Interfaces;

public interface ISharingRepository
{
    public Task<List<SharePermission>> GetPermissionsForUserAsync(Guid userId);
}