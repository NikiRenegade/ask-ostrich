using TelegramBotService.Domain.Dto;
using TelegramBotService.Domain.Entities;

namespace TelegramBotService.Domain.Sessions;

public class UserSession
{
    public AuthState AuthState { get; set; }
    public string? AuthId { get; set; }
    public User User { get; set; }
    
    public SurveyState SurveyState { get; set; } = SurveyState.None;
    public SurveyDto? CurrentSurvey { get; set; }
    public int CurrentQuestionIndex { get; set; }
    public Dictionary<Guid, object> Answers { get; } = new();
    public HashSet<string> CurrentMultiChoice { get; } = new();
}