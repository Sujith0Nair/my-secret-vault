using SecretVault.Domain;

namespace SecretVault.Application.DTOs;

public class ShareSecretDto
{
    public string ShareWithEmail { get; set; }
    public AccessLevel AccessLevel { get; set; }
}