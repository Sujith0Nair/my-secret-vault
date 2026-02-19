using Microsoft.AspNetCore.Mvc;
using SecretVault.Application.DTOs;
using SecretVault.Application.Interfaces;

namespace SecretVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthControllers(IIdentityService identityService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var result = await identityService.RegisterAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await identityService.LoginAsync(dto);

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }
        
        return Ok(result);
    }
}