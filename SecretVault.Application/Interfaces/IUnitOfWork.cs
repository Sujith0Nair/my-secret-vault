namespace SecretVault.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public ISecretRepository SecretRepository { get; }
    public IUserRepository UserRepository { get; }
    public ISharingRepository SharingRepository { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}