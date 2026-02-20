using SecretVault.Domain;
using Microsoft.EntityFrameworkCore;
using SecretVault.Application.Interfaces;
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
}