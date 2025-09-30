using SecurityService.Domain.Entities;

namespace SecurityService.Application.Interfaces
{
    public interface IExternalAuthService
    {
        Task<(User user, bool isNewAccount)> AuthenticateWithGoogleAsync(string email, string userName, string firstName, string lastName);
    }
}
