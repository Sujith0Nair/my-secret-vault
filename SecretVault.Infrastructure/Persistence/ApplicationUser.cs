using SecretVault.Domain;
using Microsoft.AspNetCore.Identity;

namespace SecretVault.Infrastructure.Persistence;

public class ApplicationUser : IdentityUser<Guid>
{
    public static User ToDomainUser(ApplicationUser user)
    {
        return User.CreateExisting(user.Id, user.Email);
    }
}