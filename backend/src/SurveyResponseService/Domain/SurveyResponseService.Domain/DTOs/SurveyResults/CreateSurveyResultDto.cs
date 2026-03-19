namespace SurveyResponseService.Domain.DTOs.SurveyResults
{
    public class CreateSurveyResultDto
    {
        public Guid? UserId { get; set; }
        public Guid SurveyId { get; set; }
        
        public Guid? GuestId { get; set; }

        public string? DisplayName { get; set; } = null!;
        public DateTime DatePassed { get; set; }
        public IList<AnswerDto>? Answers { get; set; }
    }
}
