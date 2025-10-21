namespace SurveyResponseService.Domain.Entities;

public class SurveyResult : BaseEntity
{
    private readonly List<Answer> _answers = [];

    public Guid UserId { get; set; }
    public Guid SurveyId { get; set; }
    public DateTime DatePassed { get; set; }
    public IEnumerable<Answer> Answers => _answers.ToList();

    public SurveyResult()
    {
        Id = Guid.NewGuid();
        DatePassed = DateTime.Now;
    }

    public SurveyResult(Guid userId, Guid surveyId, List<Answer> answers)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        SurveyId = surveyId;
        DatePassed = DateTime.Now;

        if (answers != null && answers.Any())
        {
            _answers.AddRange(answers);
        }
    }

    public void AddAnswers(List<Answer> answers)
    {
        if (answers != null && answers.Any())
        {
            _answers.AddRange(answers);
        }
    }

    public void UpdateAnswers(List<Answer> answers)
    {
        _answers.Clear();
        if (answers != null && answers.Any())
        {
            _answers.AddRange(answers);
        }
    }
}
