using SecretVault.Application.DTOs;

namespace SecretVault.Application.Interfaces;

public interface ISecretService
{
    public Task<SecretDto> CreateSecretAsync(Guid ownerUserId, CreateSecretDto createSecretDto);
    public Task<SecretDto> GetSecretAsync(Guid secretId, Guid requesterUserId);
    public Task<IEnumerable<SecretListItemDto>> GetAllSecretsAsync(Guid userId);
    public Task UpdateSecretAsync(Guid secretId, Guid requesterUserId, UpdateSecretDto updateSecretDto);
    public Task DeleteSecretAsync(Guid secretId, Guid requesterUserId);
}