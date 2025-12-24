namespace SecurityService.Application.Interfaces;

public interface ITelegramAuthService
{
	Task<string> StartAsync(long telegramUserId);
	Task<(bool Completed, Guid? UserId)> GetStatusAsync(string authId);
	Task CompleteAsync(string authId, Guid userId);
}
