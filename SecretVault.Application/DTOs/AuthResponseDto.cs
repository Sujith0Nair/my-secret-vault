namespace SecretVault.Application.DTOs;

public class AuthResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public Guid? UserId { get; set; }
}