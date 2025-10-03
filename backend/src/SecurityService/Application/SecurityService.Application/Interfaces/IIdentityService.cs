using SecurityService.Domain.Entities;

namespace SecurityService.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> CreateUserAsync(User user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> SignInAsync(User user, string password);
        Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}
