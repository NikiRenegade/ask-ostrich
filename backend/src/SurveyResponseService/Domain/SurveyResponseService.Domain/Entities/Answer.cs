namespace SurveyResponseService.Domain.Entities
{
    public class Answer
    {
        public Guid QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public List<string> Values { get; set; }
    }
}