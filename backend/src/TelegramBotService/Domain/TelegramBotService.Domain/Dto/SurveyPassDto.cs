namespace TelegramBotService.Domain.Dto;

public class SurveyPassDto
{
    public Guid UserId { get; set; }
    public Guid SurveyId { get; set; }
    public DateTime DatePassed { get; set; }
    public List<SurveyAnswerDto> Answers { get; set; } = [];
}