namespace TelegramBotService.Domain.Dto;

public record PassedSurveyListItemDto
{
    public Guid Id { get; set; } 
    public Guid SurveyId { get; set; } 
    public string Title { get; set; } = "";
    public DateTime DatePassed { get; set; }
}