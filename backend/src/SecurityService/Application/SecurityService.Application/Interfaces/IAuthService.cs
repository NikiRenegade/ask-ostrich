using SecurityService.Application.DTOs.Auth;

namespace SecurityService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<bool> LoginAsync(LoginDto loginDto);
        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<UserProfileDto> GetUserProfileAsync(string email);
    }
}
