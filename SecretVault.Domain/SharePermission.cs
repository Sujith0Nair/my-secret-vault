using SecretVault.Domain.Extensions;

namespace SecretVault.Domain;

public class SharePermission
{
    public Guid Id { get; private set; }
    public Guid SecretId { get; private set; }
    public Guid SharedWithUserId { get; private set; }
    public AccessLevel AccessLevel { get; private set; }
 
    private SharePermission() { }

    public SharePermission(Guid secretId, Guid sharedWithUserId, AccessLevel accessLevel)
    {
        Guard.AgainstEmptyGuid(sharedWithUserId, nameof(sharedWithUserId));
        Guard.AgainstEmptyGuid(secretId, nameof(secretId));
        Guard.AgainstEnumValue(accessLevel, AccessLevel.None, nameof(accessLevel));
        
        Id = Guid.NewGuid();
        SecretId = secretId;
        SharedWithUserId = sharedWithUserId;
        AccessLevel = accessLevel;
    }
    
    public void UpdateAccessLevel(AccessLevel accessLevel)
    {
        Guard.AgainstEnumValue(accessLevel, AccessLevel.None, nameof(accessLevel));
        AccessLevel = accessLevel;
    }
}