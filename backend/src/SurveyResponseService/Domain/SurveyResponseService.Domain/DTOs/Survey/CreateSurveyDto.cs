namespace SurveyResponseService.Domain.DTOs.Survey;

public class CreateSurveyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsPublished { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdateAt { get; set; }
    public string ShortUrl { get; set; } = null!;
    public IEnumerable<QuestionDto> Questions { get; set; } = null!;
}