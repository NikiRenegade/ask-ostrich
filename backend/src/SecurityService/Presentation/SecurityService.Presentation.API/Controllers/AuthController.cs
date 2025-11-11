using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityService.Application.DTOs.Auth;
using SecurityService.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IExternalAuthService _externalAuthService;

    public AuthController(
        IAuthService authService,
        IEmailConfirmationService emailConfirmationService,
        IJwtTokenService jwtTokenService,
        IExternalAuthService externalAuthService)
    {
        _authService = authService;
        _emailConfirmationService = emailConfirmationService;
        _jwtTokenService = jwtTokenService;
        _externalAuthService = externalAuthService;
    }

    // Регистрация
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserDto dto)
    {
        string token = await _authService.RegisterUserAsync(dto);

        var confirmLink = Url.Action(
            nameof(EmailController.ConfirmEmail),
            "Email",
            new ConfirmEmailDto { UserName = dto.UserName, Token = token },
            protocol: HttpContext.Request.Scheme);

        await _emailConfirmationService.SendConfirmationLinkAsync(dto.Email, confirmLink);
        
        UserProfileDto user = await _authService.GetUserProfileAsync(dto.Email);

        return CreatedAtAction(nameof(Register), new { email = dto.Email }, new AuthResponseDto { AccessToken = token, UserProfile = user });
    }

    // Вход
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var loginSucceeded = await _authService.LoginAsync(dto);

        if (!loginSucceeded)
            return Unauthorized(new { Message = "Неверный email или пароль." });

        // Генерация JWT только если вход успешен
        var accessToken = await _jwtTokenService.GenerateJwtToken(dto.Email);
        
        UserProfileDto user = await _authService.GetUserProfileAsync(dto.Email);

        return Ok(new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpiresIn = 900,
            UserProfile = user
        });
    }

    // Смена пароля
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        await _authService.ChangePasswordAsync(dto);
        return NoContent();
    }

    // Проверка токена
    [Authorize]
    [HttpGet("validate-token")]
    public IActionResult ValidateToken()
    {
        var username = User.Identity?.Name ?? "Unknown";
        return Ok(new { message = "Токен валиден.", user = username });
    }

    // Вход через Google OAuth
    [HttpGet("login-google")]
    public IActionResult LoginWithGoogle()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleResponse)) };
        return Challenge(properties, "Google");
    }

    // Ответ Google OAuth
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal?.Identities.FirstOrDefault()?.Claims;

        if (claims == null)
            return BadRequest(new GoogleAuthResponseDto { Message = "Ошибка авторизации через Google" });

        // Получаем email
        var email = claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value;
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new GoogleAuthResponseDto { Message = "Email не найден в данных Google-аккаунта" });

        var username = User.Identity?.Name ?? "Unknown";

        // Получаем имя
        var fullName = claims.FirstOrDefault(c => c.Type == "name")?.Value ?? email;
        var firstName = fullName.Split(' ').FirstOrDefault() ?? "";
        var lastName = string.Join(" ", fullName.Split(' ').Skip(1));

        var (user, isNewAccount) = await _externalAuthService.AuthenticateWithGoogleAsync(email, username, firstName, lastName);

        if (isNewAccount)
        {
            return NotFound(new GoogleAuthResponseDto
            {
                Message = "Ваш Google-аккаунт не привязан к аккаунту AskOstrich.",
                Suggestion = "Хотите создать новый аккаунт?",
                GoogleProfile = new { Email = email, FirstName = firstName, LastName = lastName }
            });
        }

        var token = await _jwtTokenService.GenerateJwtToken(email);

        return Ok(new GoogleAuthResponseDto
        {
            Message = "Успешный вход через Google",
            Token = new AuthResponseDto { AccessToken = token },
            UserProfile = new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        });
    }

}
