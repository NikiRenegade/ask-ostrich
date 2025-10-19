using SecurityService.Application.Interfaces;

namespace SecurityService.Application.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IEmailService _emailService;
        private readonly IIdentityService _identityService;

        public EmailConfirmationService(IEmailService emailService, IIdentityService identityService)
        {
            _emailService = emailService;
            _identityService = identityService;
        }

        public async Task SendConfirmationLinkAsync(string email, string confirmationLink)
        {
            var subject = "Подтверждение email адреса";
            var message =
                $"""
                <h2>Подтверждение email</h2>
                <p>Для завершения регистрации подтвердите ваш email адрес, перейдя по ссылке ниже:</p>
                <p><a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Подтвердить Email</a></p>
                <p>Если вы не регистрировались в нашем сервисе, проигнорируйте это письмо.</p>
                """;

            await _emailService.SendEmailAsync(email, subject, message);
        }

        public async Task<string> GenerateResendTokenAsync(string userName)
        {
            return await _identityService.GenerateEmailConfirmationTokenByUserNameAsync(userName);
        }

        public async Task<bool> ConfirmEmailAsync(string userName, string token)
        {
            var result = await _identityService.ConfirmEmailAsync(userName, token);

            if (!result)
                throw new InvalidOperationException("Не удалось подтвердить Email");

            return true;
        }
    }
}
