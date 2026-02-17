namespace SecretVault.Domain;

public enum AccessLevel
{
    None = 0,
    View  = 1,
    Edit = 2,
    Delete = 3,
    Owner = 4
}