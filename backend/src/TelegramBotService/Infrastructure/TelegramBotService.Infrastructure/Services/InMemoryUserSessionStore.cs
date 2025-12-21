using TelegramBotService.Application.Bot;
using TelegramBotService.Application.Interfaces;
using System.Collections.Concurrent;

namespace TelegramBotService.Infrastructure.Services;

public class InMemoryUserSessionStore : IUserSessionStore
{
    private readonly ConcurrentDictionary<long, UserSession> _sessions = new();

    public UserSession Get(long userId)
        => _sessions.GetOrAdd(userId, _ => new UserSession());
}