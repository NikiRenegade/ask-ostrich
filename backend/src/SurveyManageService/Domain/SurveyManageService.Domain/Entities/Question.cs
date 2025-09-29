namespace SurveyManageService.Domain.Entities;

public class Question
{
    private readonly List<Option> _options = [];

    public QuestionType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; } = 0;
    public string InnerText { get; set; } = string.Empty;
    public IEnumerable<Option> Options => _options.ToList();

    // Parameterless constructor for Entity Framework
    public Question()
    {
    }

    public Question(QuestionType type, string title, int order, string innerText)
    {        
        Type = type;
        Title = title;
        Order = order;
        InnerText = innerText;
    }

    public void AddOptions(List<Option> options)
    {
        if (options != null && options.Any())
        {
            _options.AddRange(options);
        }
    }
}

public enum QuestionType
{
    Text = 0,
    SingleChoice,
    MultipleChoice
}