namespace SurveyResponseService.Domain.DTOs.SurveyResults
{
    public class SurveyResultDto
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DatePassed { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<AnswerDto> Answers { get; set; } = new();
    }
}
