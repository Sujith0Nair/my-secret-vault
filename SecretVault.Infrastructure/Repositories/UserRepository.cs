using SecretVault.Domain;
using Microsoft.EntityFrameworkCore;
using SecretVault.Application.Interfaces;
using SecretVault.Infrastructure.Persistence;

namespace SecretVault.Infrastructure.Repositories;

public class UserRepository(SecretVaultDbContext dbContext) : IUserRepository
{   
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var appUser = await dbContext.Users.FindAsync(userId);
        return appUser == null ? null : ApplicationUser.ToDomainUser(appUser);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var appUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        return appUser == null ? null : ApplicationUser.ToDomainUser(appUser);
    }

    public async Task AddAsync(User user)
    {
        var appUser = new ApplicationUser
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.Email
        };
        
        await dbContext.Users.AddAsync(appUser);
    }
}