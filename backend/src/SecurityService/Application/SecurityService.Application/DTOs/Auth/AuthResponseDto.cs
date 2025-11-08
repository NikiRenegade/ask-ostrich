namespace SecurityService.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; } = 900; // 15 минут
        
        public UserProfileDto UserProfile { get; set; }
    }
}
