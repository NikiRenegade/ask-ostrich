namespace SecurityService.Application.Interfaces
{
    public interface IEmailConfirmationService
    {
        Task SendConfirmationLinkAsync(string email, string confirmationLink);
        Task<string> GenerateResendTokenAsync(string userName);
        Task<bool> ConfirmEmailAsync(string userName, string token);
    }
}
