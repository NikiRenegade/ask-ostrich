using TelegramBotService.Application.Bot;

namespace TelegramBotService.Application.Actions;

public static class MenuActions
{
    public static IReadOnlyList<AppAction> GetMenuActions()
    {
        return new List<AppAction>
        {
            new AppAction { Id = "menu.startSurvey", Label = "📝 Пройти опрос" },
            new AppAction { Id = "menu.mySurveys", Label = "📋 Мои опросы" },
            new AppAction { Id = "menu.completedSurveys", Label = "🏁 Пройденные опросы" },
            new AppAction { Id = "menu.profile", Label = "👤 Мой профиль" }
        };
    }
}