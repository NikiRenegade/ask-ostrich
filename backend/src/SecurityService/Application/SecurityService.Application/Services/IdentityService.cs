using Microsoft.AspNetCore.Identity;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;

namespace SecurityService.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Создание пользователя с паролем.
        /// </summary>
        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        /// <summary>
        /// Генерация токена подтверждения email.
        /// </summary>
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        /// <summary>
        /// Генерация токена подтверждения email по имени пользователя.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<string> GenerateEmailConfirmationTokenByUserNameAsync(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName)
                ?? throw new ArgumentException("Пользователь не существует");

            return await GenerateEmailConfirmationTokenAsync(user);
        }

        /// <summary>
        /// Подтверждение email
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmEmailAsync(string userName, string token)
        {
            User? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        /// <summary>
        /// Проверка пароля пользователя.
        /// </summary>
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Попытка входа пользователя.
        /// </summary>
        public async Task<bool> SignInAsync(User user, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(
                user,
                password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            return result.Succeeded;
        }

        /// <summary>
        /// Смена пароля пользователя.
        /// </summary>
        public async Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }
    }
}
