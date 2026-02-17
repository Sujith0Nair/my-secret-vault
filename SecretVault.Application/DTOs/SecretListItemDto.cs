using SecretVault.Domain;

namespace SecretVault.Application.DTOs;

public class SecretListItemDto
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public bool IsOwner { get; set; }
    public AccessLevel GrantedAccessLevel { get; set; }
    
    public static SecretListItemDto ToSecretListItemDto(Secret secret, bool owner, AccessLevel accessLevel)
    {
        return new SecretListItemDto
        {
            Id = secret.Id,
            OwnerUserId = secret.OwnerUserId,
            Title = secret.Title,
            CreatedAt = secret.CreatedAt,
            LastModifiedAt = secret.LastModifiedAt,
            IsOwner = owner,
            GrantedAccessLevel = accessLevel,
        };
    }
}