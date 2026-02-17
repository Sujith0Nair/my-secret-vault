using SecretVault.Domain;

namespace SecretVault.Application.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(Guid userId);
    public Task<User?> GetByEmailAsync(string email);
    public Task AddAsync(Secret secret);
}