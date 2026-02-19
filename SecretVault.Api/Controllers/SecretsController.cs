using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SecretVault.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using SecretVault.Application.Interfaces;

namespace SecretVault.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SecretsController(ISecretService secretService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SecretListItemDto>>> GetSecrets()
    {
        var userId = GetUserId();
        var secrets = await secretService.GetAllSecretsAsync(userId);
        return Ok(secrets);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SecretDto>> GetSecret(Guid id)
    {
        var userId = GetUserId();
        try
        {
            var secret = await secretService.GetSecretAsync(id, userId);
            return Ok(secret);
        }
        catch (UnauthorizedAccessException accessException)
        {
            return Forbid(accessException.Message);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            return NotFound(invalidOperationException.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<SecretDto>> CreateSecret([FromBody] CreateSecretDto createSecretDto)
    {
        var userId = GetUserId();
        var secret = await secretService.CreateSecretAsync(userId, createSecretDto);
        return CreatedAtAction(nameof(GetSecret), new { id = secret.Id }, secret);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateSecret(Guid id, [FromBody] UpdateSecretDto updateSecretDto)
    {
        var userId = GetUserId();
        try
        {
            await secretService.UpdateSecretAsync(id, userId, updateSecretDto);
            return NoContent();
        }
        catch (UnauthorizedAccessException accessException)
        {
            return Forbid(accessException.Message);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            return NotFound(invalidOperationException.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteSecret(Guid id)
    {
        var userId = GetUserId();
        try
        {
            await secretService.DeleteSecretAsync(id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException accessException)
        {
            return Forbid(accessException.Message);
        }
        catch (InvalidOperationException invalidOperationException)
        {
            return NotFound(invalidOperationException.Message);
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User Id not found in the token");
        }
        return userId;
    }
}