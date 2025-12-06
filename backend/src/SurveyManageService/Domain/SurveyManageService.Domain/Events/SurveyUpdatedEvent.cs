using SurveyManageService.Domain.DTO;

namespace SurveyManageService.Domain.Events;

public record SurveyUpdatedEvent
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
    public Dictionary<string, object> Changes { get; set; } = null!;
}