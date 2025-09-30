using Microsoft.AspNetCore.Identity;
using SecurityService.Application.DTOs.Auth;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Interfaces.Repositories;

namespace SecurityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Регистрация нового пользователя
        public async Task<string> RegisterUserAsync(RegisterUserDto dto)
        {
            // Проверка существующего пользователя
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception($"Пользователь с email '{dto.Email}' уже существует.");

            // Создание нового пользователя
            var user = new User(dto.Email, dto.UserName, dto.FirstName, dto.LastName);

            // Добавление пользователя в Identity
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Не удалось создать пользователя: {errorMessages}");
            }

            // Возврат токена подтверждения email
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        // Вход пользователя
        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            // Получение пользователя по email
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            // Если пользователь не найден или хэш пароля отсутствует, вход не успешен
            if (user == null || user.PasswordHash == null)
                return false;

            // Попытка входа с паролем
            var result = await _signInManager.PasswordSignInAsync(
                user,
                loginDto.Password,
                isPersistent: false,  // Не сохранять cookie между сессиями
                lockoutOnFailure: false // Не блокировать аккаунт при ошибке входа
            );

            return result.Succeeded;
        }

        // Изменение пароля пользователя
        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            // Получение пользователя по email
            var user = await _userRepository.GetUserByEmailAsync(changePasswordDto.Email);

            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }

            // Попытка изменения пароля
            var result = await _userManager.ChangePasswordAsync(user,
                                                                changePasswordDto.CurrentPassword,
                                                                changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                // Объединение всех ошибок в одно сообщение
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Не удалось изменить пароль: {errors}");
            }
        }

        // Получение профиля пользователя
        public async Task<UserProfileDto> GetUserProfileAsync(string email)
        {
            // Получение пользователя по email
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден.");
            }

            // Преобразование пользователя в DTO
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
