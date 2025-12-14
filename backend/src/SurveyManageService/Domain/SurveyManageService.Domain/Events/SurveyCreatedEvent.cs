using SurveyManageService.Domain.DTO;

namespace SurveyManageService.Domain.Events;

public class SurveyCreatedEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsPublished { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdateAt { get; set; }
    public Guid? ShortUrlId { get; set; }
    public IEnumerable<QuestionDto> Questions { get; set; } = null!;
}