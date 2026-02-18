using Microsoft.EntityFrameworkCore;
using SecretVault.Application.Interfaces;
using SecretVault.Domain;
using SecretVault.Infrastructure.Persistence;

namespace SecretVault.Infrastructure.Repositories;

public class SharingRepository(SecretVaultDbContext dbContext) : ISharingRepository
{   
    public async Task<List<SharePermission>> GetPermissionsForUserAsync(Guid userId)
    {
        return await dbContext.SharePermissions
            .Where(s => s.SharedWithUserId == userId)
            .ToListAsync();
    }

    public async Task<SharePermission?> GetPermissionByIdAsync(Guid permissionId)
    {
        return await dbContext.SharePermissions.FirstOrDefaultAsync(s => s.Id == permissionId);
    }

    public async Task<SharePermission?> GetPermissionForSecretAndUserAsync(Guid secretId, Guid sharedUserId)
    {
        return await dbContext.SharePermissions.FirstOrDefaultAsync(s => s.SecretId == secretId && s.SharedWithUserId == sharedUserId);
    }

    public async Task AddAsync(SharePermission sharePermission)
    {
        await dbContext.SharePermissions.AddAsync(sharePermission);
    }

    public void Update(SharePermission sharePermission)
    {
        dbContext.SharePermissions.Update(sharePermission);
    }

    public void Delete(SharePermission sharePermission)
    {
        dbContext.SharePermissions.Remove(sharePermission);
    }
}