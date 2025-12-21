namespace TelegramBotService.Domain.Dto;

public class SurveyResultCreateDto
{
	public Guid UserId { get; set; }
	public Guid SurveyId { get; set; }
	public DateTime DatePassed { get; set; }
	public IList<AnswerDto>? Answers { get; set; }
}

public class AnswerDto
{
	public Guid QuestionId { get; set; }
	public string QuestionTitle { get; set; } = null!;
	public List<string> Values { get; set; } = new();
	public bool IsCorrect { get; set; }
}