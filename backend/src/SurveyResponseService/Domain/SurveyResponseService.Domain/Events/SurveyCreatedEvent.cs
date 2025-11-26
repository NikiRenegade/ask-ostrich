using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Domain.Events;

public class SurveyCreatedEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsPublished { get; set; }
    public User? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ShortUrl { get; set; } = null!;
}