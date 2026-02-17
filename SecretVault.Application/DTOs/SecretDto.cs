using SecretVault.Domain;

namespace SecretVault.Application.DTOs;

public class SecretDto
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }

    public static SecretDto ToSecretDto(Secret secret, string decryptedContent)
    {
        return new SecretDto
        {
            Id = secret.Id,
            OwnerUserId = secret.OwnerUserId,
            Title = secret.Title,
            Content = decryptedContent,
            CreatedAt = secret.CreatedAt,
            LastModifiedAt = secret.LastModifiedAt
        };
    }
}