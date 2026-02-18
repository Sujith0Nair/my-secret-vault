using SecretVault.Application.DTOs;

namespace SecretVault.Application.Interfaces;

public interface IIdentityService
{
    public Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    public Task<AuthResponseDto> LoginAsync(LoginDto dto);
}