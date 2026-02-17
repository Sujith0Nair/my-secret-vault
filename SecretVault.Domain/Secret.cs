using SecretVault.Domain.Extensions;

namespace SecretVault.Domain;

public class Secret
{
    public Guid Id { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public string Title { get; private set; }
    public string EncryptedContent { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastModifiedAt { get; private set; }

    private readonly List<SharePermission> _sharePermissions = [];
    public IReadOnlyCollection<SharePermission> SharePermissions => _sharePermissions;
    
    private Secret() { }

    public Secret(Guid ownerUserId, string title, string encryptedContent)
    {
        Guard.AgainstEmptyGuid(ownerUserId, nameof(ownerUserId));
        Guard.AgainstNullOrWhiteSpace(title, nameof(title));
        Guard.AgainstNullOrWhiteSpace(encryptedContent, nameof(encryptedContent));
        
        Id = Guid.NewGuid();
        OwnerUserId = ownerUserId;
        Title = title;
        EncryptedContent = encryptedContent;
        CreatedAt = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void UpdateSecret(string newTitle, string newEncryptedContent)
    {
        Guard.AgainstNullOrWhiteSpace(newTitle, nameof(newTitle));
        Guard.AgainstNullOrWhiteSpace(newEncryptedContent, nameof(newEncryptedContent));
        
        Title = newTitle;
        EncryptedContent = newEncryptedContent;
    }

    public void AddSharePermission(Guid sharedWithUserId, AccessLevel accessLevel)
    {
        Guard.AgainstEmptyGuid(sharedWithUserId, nameof(sharedWithUserId));
        Guard.AgainstEnumValue(accessLevel, AccessLevel.None, nameof(accessLevel));

        if (_sharePermissions.Exists(p => p.SharedWithUserId == sharedWithUserId))
        {
            throw new ArgumentException($"Share permission with user id '{sharedWithUserId}' already exists", nameof(sharedWithUserId));
        }
        
        _sharePermissions.Add(new SharePermission(Id, sharedWithUserId, accessLevel));
    }

    public void UpdateSharePermission(Guid sharedWithUserId, AccessLevel accessLevel)
    {
        Guard.AgainstEmptyGuid(sharedWithUserId, nameof(sharedWithUserId));
        Guard.AgainstEnumValue(accessLevel, AccessLevel.None, nameof(accessLevel));
        
        var existingPermission = _sharePermissions.FirstOrDefault(p => p.SharedWithUserId == sharedWithUserId);

        if (existingPermission == null)
        {
            throw new InvalidOperationException($"No shared user with the id '{sharedWithUserId}' was found");
        }
        
        existingPermission.UpdateAccessLevel(accessLevel);
    }

    public void RemoveSharePermission(Guid sharedWithUserId)
    {
        Guard.AgainstEmptyGuid(sharedWithUserId, nameof(sharedWithUserId));
        
        var removedCount = _sharePermissions.RemoveAll(p => p.SharedWithUserId == sharedWithUserId);
        if (removedCount == 0)
        {
            throw new InvalidOperationException($"Share permission with user id '{sharedWithUserId}' was not found");
        }
    }
}