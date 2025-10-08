namespace SurveyResponseService.Domain.DTOs.SurveyResults
{
    public class AnswerDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public List<string> Values { get; set; }
    }
}
