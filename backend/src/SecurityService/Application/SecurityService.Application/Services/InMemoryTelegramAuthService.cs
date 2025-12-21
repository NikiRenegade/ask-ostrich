using System.Collections.Concurrent;
using SecurityService.Application.Interfaces;

namespace SecurityService.Application.Services.TelegramAuth;

public class InMemoryTelegramAuthService : ITelegramAuthService
{
	private readonly ConcurrentDictionary<string, (long TelegramId, Guid? UserId)> _store = new();

	public Task<string> StartAsync(long telegramUserId)
	{
		var authId = Guid.NewGuid().ToString();
		_store[authId] = (telegramUserId, null);
		return Task.FromResult(authId);
	}

	public Task<(bool Completed, Guid? UserId)> GetStatusAsync(string authId)
	{
		if (!_store.TryGetValue(authId, out var entry))
			return Task.FromResult((false, (Guid?)null));

		return Task.FromResult((entry.UserId != null, entry.UserId));
	}

	public Task CompleteAsync(string authId, Guid userId)
	{
		if (_store.TryGetValue(authId, out var entry))
			_store[authId] = (entry.TelegramId, userId);

		return Task.CompletedTask;
	}
}