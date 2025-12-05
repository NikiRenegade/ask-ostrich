namespace SurveyManageService.Domain.Entities;

public class Survey : BaseEntity
{
    private readonly List<Question> _questions = [];

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdateAt { get; set; }
    public string ShortUrl { get; set; } = string.Empty; 
    public IEnumerable<Question> Questions => _questions.ToList();

    public virtual User? Author { get; set; }

    // Parameterless constructor for Entity Framework
    public Survey()
    {
        Id = Guid.NewGuid();
        ShortUrl = string.Empty;
        IsPublished = false;
        CreatedAt = DateTime.Now;
        LastUpdateAt = DateTime.Now;
    }

    public Survey(string title, string description, Guid authorId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        ShortUrl = string.Empty;
        AuthorId = authorId;
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

    public void UpdateQuestions(List<Question> questions)
    {
        _questions.Clear();
        if (questions != null && questions.Any())
        {
            _questions.AddRange(questions);
        }
    }
}