using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Interfaces.Repositories;

namespace SecurityService.Application.Services
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly IUserRepository _userRepository;

        public ExternalAuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(User user, bool isNewAccount)> AuthenticateWithGoogleAsync(string email, string userName, string firstName, string lastName)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                // Аккаунт Google не привязан
                return (User.CreateOrUpdateFromExternalProvider(email, userName, firstName, lastName), true);
            }

            // Пользователь найден, возвращаем его
            return (user, false);
        }
    }

}
