using TelegramBotService.Application.Interfaces;
namespace TelegramBotService.Application.Handlers
{
    public class HandleUserInputUseCase
    {
        private readonly IUserSessionStore _sessions;
        private readonly IEnumerable<IUserCommand> _commands;

        public HandleUserInputUseCase(
            IUserSessionStore sessions,
            IEnumerable<IUserCommand> commands)
        {
            _sessions = sessions;
            _commands = commands;
        }

        public async Task<AppResponse> HandleAsync(
            long userId,
            string? text,
            string? action)
        {
            var session = _sessions.Get(userId);
            var input = new UserInput { Text = text, Action = action, ChatId = userId };

            var command = _commands.FirstOrDefault(c => c.CanHandle(input, session));

            if (command == null)
                return new AppResponse
                {
                    Text = "Неизвестная команда",
                    Actions = [ new AppAction { Id = "menu.startSurvey", Label = "📝 Пройти опрос" },
                                new AppAction { Id = "menu.mySurveys", Label = "📋 Мои опросы" }]
                };

            return await command.HandleAsync(input, session);
        }
    }
}