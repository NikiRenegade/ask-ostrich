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
                Author = survey.Author != null ? UserMapper.ToDto(survey.Author) : null,
                CreatedAt = survey.CreatedAt,
                LastUpdateAt = survey.LastUpdateAt,
                Questions = survey.Questions.Select(QuestionMapper.ToDto).ToList()
            };
        }

        public static Survey ToEntity(CreateSurveyDto createSurveyDto, User author)
        {
            if (createSurveyDto == null)
                throw new ArgumentNullException(nameof(createSurveyDto));
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            var survey = new Survey(createSurveyDto.Title, createSurveyDto.Description, author);

            if (createSurveyDto.Questions.Any())
            {
                var questions = createSurveyDto.Questions.Select(QuestionMapper.ToEntity).ToList();
                survey.AddQuestions(questions);
            }

            return survey;
        }

        public static Survey ToEntity(UpdateSurveyDto updateSurveyDto, User author)
        {
            if (updateSurveyDto == null)
                throw new ArgumentNullException(nameof(updateSurveyDto));
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            var survey = new Survey(updateSurveyDto.Title, updateSurveyDto.Description, author);
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
