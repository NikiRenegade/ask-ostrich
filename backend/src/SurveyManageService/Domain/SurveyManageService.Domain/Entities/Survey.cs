namespace SurveyManageService.Domain.Entities;

public class Survey : BaseEntity
{
    private readonly List<Question> _questions = [];

    public string Title { get; set; } 
    public string Description { get; set; } 
    public bool IsPublished { get; set; }
    public User? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdateAt { get; set; }
    public string ShortUrl { get; set; } 
    public IEnumerable<Question> Questions => _questions.ToList();

    public Survey(string title, string desctiption, User author)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = desctiption;
        ShortUrl = string.Empty;
        Author = author;
        IsPublished = false;
        CreatedAt = DateTime.Now;
        LastUpdateAt = DateTime.Now;
    }

    public void AddQuestions(List<Question> questions)
    {
        if (questions != null && questions.Any())
        {
            _questions.AddRange(questions);
        }
    }
}