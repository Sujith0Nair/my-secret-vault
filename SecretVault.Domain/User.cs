namespace SecretVault.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }

    private User() { }
    
    public User(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("User email id cannot be empty", nameof(email));
        Id = Guid.NewGuid();
        Email = email;
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("User email id cannot be empty", nameof(email));
        Email = email;
    }
}