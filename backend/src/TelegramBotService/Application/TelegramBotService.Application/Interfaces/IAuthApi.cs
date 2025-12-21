using TelegramBotService.Domain.Dto;

namespace TelegramBotService.Application.Interfaces;

public interface IAuthApi
{
    Task<string> StartTelegramAuth(long telegramUserId);
    Task<AuthStatusDto> GetStatus(string authId);

    Task<IEnumerable<PendingAuthDto>> GetPendingAsync();
    Task ClearAsync(string authId);
}