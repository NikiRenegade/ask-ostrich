public class PassedSurveyAnswerDto
{
    public Guid QuestionId { get; set; }

    public string QuestionTitle { get; set; } = string.Empty;
    
    public List<string> Values { get; set; } = new();

    public bool IsCorrect { get; set; }
}