namespace SurveyManageService.Domain.Entities;

public class Survey
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPublished { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ShortUrl { get; set; }
    public List<Question> Questions { get; set; }
}