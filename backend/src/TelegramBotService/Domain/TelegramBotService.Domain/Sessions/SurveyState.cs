namespace TelegramBotService.Domain.Sessions;

public enum SurveyState
{
    None,
    WaitingForSurveyGuid,
    InProgress,
    Completed
}