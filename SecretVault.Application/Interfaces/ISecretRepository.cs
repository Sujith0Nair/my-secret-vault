using SecretVault.Domain;

namespace SecretVault.Application.Interfaces;

public interface ISecretRepository
{
    public Task<Secret?> GetByIdAsync(Guid secretId);
    public Task<List<Secret>> GetByOwnerIdAsync(Guid ownerId);
    public Task AddAsync(Secret secret);
    void Update(Secret secret);
    void Delete(Secret secret);
}