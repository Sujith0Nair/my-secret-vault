using SecretVault.Domain;

namespace SecretVault.Application.DTOs;

public class UpdatePermissionDto
{
    public AccessLevel AccessLevel { get; set; }
}