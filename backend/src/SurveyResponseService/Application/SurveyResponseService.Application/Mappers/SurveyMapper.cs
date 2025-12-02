using SurveyResponseService.Domain.DTOs.Survey;
using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Application.Mappers
{
    public static class SurveyMapper
    {
        public static SurveyDto ToDto(Survey survey)
        {
            if (survey == null)
                throw new ArgumentNullException(nameof(survey));

            return new SurveyDto
            {
                Id = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                IsPublished = survey.IsPublished,
                AuthorId = survey.AuthorId,
                CreatedAt = survey.CreatedAt,
                LastUpdateAt = survey.LastUpdateAt,
                Questions = survey.Questions.Select(QuestionMapper.ToDto).ToList()
            };
        }

        public static Survey ToEntity(CreateSurveyDto createSurveyDto)
        {
            if (createSurveyDto == null)
                throw new ArgumentNullException(nameof(createSurveyDto));

            var survey = new Survey(createSurveyDto.Title, createSurveyDto.Description, createSurveyDto.AuthorGuid);

            if (createSurveyDto.Questions.Any())
            {
                var questions = createSurveyDto.Questions.Select(QuestionMapper.ToEntity).ToList();
                survey.AddQuestions(questions);
            }

            return survey;
        }
        
        public static Survey ToEntity(SurveyDto surveyDto)
        {
            if (surveyDto == null)
                throw new ArgumentNullException(nameof(surveyDto));

            var survey = new Survey()
            {
                Id = surveyDto.Id,
                Description = surveyDto.Description,
                AuthorId = surveyDto.AuthorId,
                CreatedAt = surveyDto.CreatedAt,
                IsPublished = surveyDto.IsPublished,
                LastUpdateAt = surveyDto.LastUpdateAt,
                ShortUrl = surveyDto.ShortUrl,
                Title = surveyDto.Title,
            };

            if (surveyDto.Questions.Any())
            {
                var questions = surveyDto.Questions.Select(QuestionMapper.ToEntity).ToList();
                survey.AddQuestions(questions);
            }

            return survey;
        }

        public static Survey ToEntity(UpdateSurveyDto updateSurveyDto)
        {
            if (updateSurveyDto == null)
                throw new ArgumentNullException(nameof(updateSurveyDto));

            var survey = new Survey(updateSurveyDto.Title, updateSurveyDto.Description, updateSurveyDto.AuthorGuid);
            survey.Id = updateSurveyDto.Id;
            survey.IsPublished = updateSurveyDto.IsPublished;

            if (updateSurveyDto.Questions.Any())
            {
                var questions = updateSurveyDto.Questions.Select(QuestionMapper.ToEntity).ToList();
                survey.AddQuestions(questions);
            }

            return survey;
        }
    }
}
