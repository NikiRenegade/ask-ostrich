using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
[ApiController]
[Route("api/[controller]")]
public class TelegramAuthController : ControllerBase
{
	private readonly ITelegramAuthService _service;
	private readonly IUserRepository _userRepository;

	public TelegramAuthController(ITelegramAuthService service, IUserRepository userRepository)
	{
		_service = service;
		_userRepository = userRepository;
	}

	[HttpPost("start")]
	public async Task<ActionResult<object>> Start([FromBody] StartRequest req)
	{
		var authId = await _service.StartAsync(req.TelegramUserId);
		return Ok(new { AuthId = authId });
	}

	[HttpGet("status")]
	public async Task<ActionResult<object>> Status([FromQuery] string authId)
	{
		var (completed, userId) = await _service.GetStatusAsync(authId);

		if (userId == null)
		{
			return BadRequest();
		}
		var user = await _userRepository.GetByIdAsync(userId.Value);
		if (user != null && completed)
		{
			return Ok(new { Completed = true, UserId = userId, UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName });
		}

		return BadRequest();
	}

	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[HttpPost("complete")]
	public async Task<IActionResult> Complete([FromBody] CompleteRequest req)
	{
		var claim = User.FindFirst("userId")
			?? User.FindFirst(ClaimTypes.NameIdentifier)
			?? User.FindFirst(JwtRegisteredClaimNames.Sub);

		if (claim == null)
		{
			return Unauthorized("User claim not found");
		}

		if (!Guid.TryParse(claim.Value, out var userId))
		{
			return BadRequest("Invalid user id claim format");
		}

		if (req == null || string.IsNullOrWhiteSpace(req.AuthId))
		{
			return BadRequest("authId is required");
		}

		try
		{
			await _service.CompleteAsync(req.AuthId, userId);
			return NoContent();
		}
		catch (Exception ex)
		{
			return Problem("Internal server error");
		}
	}

	public record StartRequest(long TelegramUserId);
	public record CompleteRequest(string AuthId);
}