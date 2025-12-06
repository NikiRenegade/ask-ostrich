using SurveyResponseService.Domain.DTOs.Users;

namespace SurveyResponseService.Domain.DTOs.Survey;

public class SurveyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsPublished { get; set; }
    public UserDto Author { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdateAt { get; set; }
    public string ShortUrl { get; set; } = null!;
    public IEnumerable<QuestionDto> Questions { get; set; } = null!;
}