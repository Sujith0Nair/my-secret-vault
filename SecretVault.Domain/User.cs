using SecretVault.Domain.Extensions;

namespace SecretVault.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }

    private User() { }

    private User(Guid id, string? email)
    {
        Guard.AgainstEmptyGuid(id, nameof(id));
        Guard.AgainstNullOrWhiteSpace(email, nameof(email));
        
        Id = id;
        Email = email!;
    }
    
    public User(string? email)
    {
        Guard.AgainstNullOrWhiteSpace(email, nameof(email));
        Id = Guid.NewGuid();
        Email = email!;
    }

    public void UpdateEmail(string? email)
    {
        Guard.AgainstNullOrWhiteSpace(email, nameof(email));
        Email = email!;
    }

    public static User CreateExisting(Guid id, string? email)
    {
        return new User(id, email);
    }
}