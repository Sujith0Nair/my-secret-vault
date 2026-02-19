using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SecretVault.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using SecretVault.Application.Interfaces;

namespace SecretVault.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/{secretId:guid}")]
public class SharingController(ISharingService sharingService) : ControllerBase
{
    [HttpPost("share")]
    public async Task<ActionResult<PermissionDto>> ShareSecret(Guid secretId, [FromBody] ShareSecretDto shareSecretDto)
    {
        var userId = GetUserId();
        try
        {
            var result = await sharingService.ShareSecretAsync(secretId, userId, shareSecretDto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException accessException)
        {
            return Forbid(accessException.Message);
        }
        catch (InvalidOperationException validationException)
        {
            return BadRequest(validationException.Message);
        }
    }

    [HttpPut("permissions/{sharedWithUserId:guid}")]
    public async Task<ActionResult> UpdatePermission(Guid secretId, Guid sharedWithUserId,
        [FromBody] UpdatePermissionDto updatePermissionDto)
    {
        var userId = GetUserId();
        try
        {
            await sharingService.UpdateSharePermissionAsync(secretId, userId, sharedWithUserId, updatePermissionDto);
            return NoContent();
        }
        catch (UnauthorizedAccessException accessException)
        {
            return Forbid(accessException.Message);
        }
        catch (InvalidOperationException validationException)
        {
            return NotFound(validationException.Message);
        }
    }

    [HttpGet("permissions")]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions(Guid secretId)
    {
        var userId = GetUserId();
        try
        {
            var result = await sharingService.GetPermissionsForSecretAsync(secretId, userId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException accessException)
        {
            return Forbid(accessException.Message);
        }
        catch (InvalidOperationException validationException)
        {
            return NotFound(validationException.Message);
        }
    }

    [HttpDelete("permissions/{sharedWithUserId:guid}")]
    public async Task<ActionResult> DeletePermission(Guid secretId, Guid sharedWithUserId)
    {
        var userId = GetUserId();
        try
        {
            await sharingService.RevokeSharePermissionAsync(secretId, userId, sharedWithUserId);
            return NoContent();
        }
        catch (UnauthorizedAccessException accessException)
        {
            return Forbid(accessException.Message);
        }
        catch (InvalidOperationException validationException)
        {
            return NotFound(validationException.Message);
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