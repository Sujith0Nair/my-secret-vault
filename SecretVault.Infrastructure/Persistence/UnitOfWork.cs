using SecretVault.Application.Interfaces;
using SecretVault.Infrastructure.Repositories;

namespace SecretVault.Infrastructure.Persistence;

public class UnitOfWork(SecretVaultDbContext dbContext) : IUnitOfWork
{
    private ISecretRepository? _secretRepository;
    private IUserRepository? _userRepository;
    private ISharingRepository? _sharingRepository;
    
    public ISecretRepository SecretRepository => _secretRepository ??= new SecretRepository(dbContext);
    public IUserRepository UserRepository =>  _userRepository ??= new UserRepository(dbContext);
    public ISharingRepository SharingRepository =>  _sharingRepository ??= new SharingRepository(dbContext);
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        dbContext.Dispose();
    }
}