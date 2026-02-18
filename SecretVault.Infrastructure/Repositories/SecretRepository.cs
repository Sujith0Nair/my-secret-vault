using SecretVault.Domain;
using Microsoft.EntityFrameworkCore;
using SecretVault.Application.Interfaces;
using SecretVault.Infrastructure.Persistence;

namespace SecretVault.Infrastructure.Repositories;

public class SecretRepository(SecretVaultDbContext dbContext) : ISecretRepository
{   
    public async Task<Secret?> GetByIdAsync(Guid secretId)
    {
        return await dbContext.Secrets
            .Include(s => s.SharePermissions)
            .FirstOrDefaultAsync(s => s.Id == secretId);
    }

    public async Task<List<Secret>> GetByOwnerIdAsync(Guid ownerId)
    {
        return await dbContext.Secrets
            .Include(s => s.SharePermissions)
            .Where(s => s.OwnerUserId == ownerId)
            .ToListAsync();
    }

    public async Task AddAsync(Secret secret)
    {
        await dbContext.Secrets.AddAsync(secret);
    }

    public void Update(Secret secret)
    {
        dbContext.Secrets.Update(secret);
    }

    public void Delete(Secret secret)
    {
        dbContext.Secrets.Remove(secret);
    }
}