namespace SurveyResponseService.Domain.DTOs.SurveyResults;

public class PassedSurveyDto
{
    public Guid SurveyId { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime DatePassed { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
}

