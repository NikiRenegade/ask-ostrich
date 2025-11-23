using SecurityService.Application.DTOs.Auth;
using SecurityService.Application.Interfaces;
using SecurityService.Application.Mappers;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Interfaces.Repositories;
using SecurityService.Domain.Interfaces.Publishers;

namespace SecurityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;
        private readonly IUserEventPublisher _userEventPublisher;

        public AuthService(IUserRepository userRepository, IIdentityService identityService, IUserEventPublisher userEventPublisher)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _userEventPublisher = userEventPublisher;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException($"Пользователь с email '{dto.Email}' уже существует.");

            var user = new User(dto.Email, dto.UserName, dto.FirstName, dto.LastName);

            var created = await _identityService.CreateUserAsync(user, dto.Password);
            if (!created)
                throw new InvalidOperationException("Не удалось создать пользователя.");
            _userEventPublisher.PublishUserCreated(user.ToUserCreatedEvent());
            return await _identityService.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<bool> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return false;

            return await _identityService.SignInAsync(user, dto.Password);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден.");

            var changed = await _identityService.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!changed)
                throw new InvalidOperationException("Не удалось изменить пароль.");
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден.");

            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty
            };
        }
    }
}
