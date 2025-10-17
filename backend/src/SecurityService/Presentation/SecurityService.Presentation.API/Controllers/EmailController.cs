using Microsoft.AspNetCore.Mvc;
using SecurityService.Application.DTOs.Auth;
using SecurityService.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailConfirmationService _emailConfirmationService;

    public EmailController(IEmailConfirmationService emailConfirmationService)
    {
        _emailConfirmationService = emailConfirmationService;
    }

    /// <summary>
    /// Подтверждение email
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userName, [FromQuery] string token)
    {
        try
        {
            var result = await _emailConfirmationService.ConfirmEmailAsync(userName, token);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Повторная отправка подтверждения
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("resend-email-confirmation")]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailDto dto)
    {
        try
        {
            var token = await _emailConfirmationService.GenerateResendTokenAsync(dto.UserName);

            var confirmLink = Url.Action(
                nameof(ConfirmEmail),
                "Email",
                new ConfirmEmailDto { UserName = dto.UserName, Token = token },
                protocol: HttpContext.Request.Scheme);

            await _emailConfirmationService.SendConfirmationLinkAsync(dto.Email, confirmLink);

            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
