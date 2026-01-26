public class PassedSurveyDto
{
    public Guid Id { get; set; }

    public Guid SurveyId { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DatePassed { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<PassedSurveyAnswerDto> Answers { get; set; } = new();
}