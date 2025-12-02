namespace SurveyResponseService.Domain.DTOs.Survey
{
    public class UpdateSurveyDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public Guid AuthorGuid { get; set; }
        public List<CreateQuestionDto> Questions { get; set; } = [];
    }
}