namespace SurveyManageService.Domain.Events;

public record SurveyUpdatedEvent
{
    public Guid Id { get; set; }
    public Dictionary<string, object> Changes { get; set; }
}