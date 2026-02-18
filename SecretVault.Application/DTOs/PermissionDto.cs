using SecretVault.Domain;

namespace SecretVault.Application.DTOs;

public class PermissionDto
{
    public Guid PermissionId { get; set; }
    public Guid SecretId { get; set; }
    public Guid SharedWithUserId { get; set; }
    public string? SharedWithEmail { get; set; }
    public AccessLevel AccessLevel { get; set; }

    public static PermissionDto ToPermissionDto(SharePermission sharePermission, User? sharedWithUser)
    {
        return new PermissionDto
        {
            PermissionId = sharePermission.Id,
            SecretId = sharePermission.SecretId,
            SharedWithUserId = sharePermission.SharedWithUserId,
            SharedWithEmail = sharedWithUser?.Email,
            AccessLevel = sharePermission.AccessLevel
        };
    }
}