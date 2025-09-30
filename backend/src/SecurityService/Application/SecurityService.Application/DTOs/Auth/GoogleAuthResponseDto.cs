namespace SecurityService.Application.DTOs.Auth
{
    public class GoogleAuthResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Suggestion { get; set; } = string.Empty;
        public AuthResponseDto? Token { get; set; }
        public UserProfileDto? UserProfile { get; set; }
        public object? GoogleProfile { get; set; }
    }

}
