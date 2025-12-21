using SurveyResponseService.Domain.DTOs.Survey;
using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Application.Mappers
{
    public static class SurveyMapper
    {
        public static SurveyDto ToDto(Survey survey)
        {
            ArgumentNullException.ThrowIfNull(survey);

            return new SurveyDto
            {
                Id = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                IsPublished = survey.IsPublished,
                Author = survey.Author != null
                    ? UserMapper.ToDto(survey.Author)
                    : throw new ArgumentNullException(nameof(survey.Author), "Author must exist!"),
                CreatedAt = survey.CreatedAt,
                LastUpdateAt = survey.LastUpdateAt,
                ShortUrlId = survey.ShortUrlId,
                Questions = survey.Questions.Select(QuestionMapper.ToDto).ToList()
            };
        }
        
        public static Survey ToEntity(CreateSurveyDto createSurveyDto, User author)
        {
            ArgumentNullException.ThrowIfNull(createSurveyDto);

            var survey = new Survey(createSurveyDto.Title, createSurveyDto.Description, author.Id)
            {
                Id = createSurveyDto.Id,
                CreatedAt = createSurveyDto.CreatedAt,
                IsPublished = createSurveyDto.IsPublished,
                LastUpdateAt = createSurveyDto.LastUpdateAt,
                ShortUrlId = createSurveyDto.ShortUrlId
            };

            if (createSurveyDto.Questions.Any())
            {
                var questions = createSurveyDto.Questions.Select(QuestionMapper.ToEntity).ToList();
                survey.AddQuestions(questions);
            }

            return survey;
        }

        public static Survey ToEntity(UpdateSurveyDto updateSurveyDto, User author)
        {
            ArgumentNullException.ThrowIfNull(updateSurveyDto);

            var survey = new Survey(updateSurveyDto.Title, updateSurveyDto.Description, author.Id)
            {
                Id = updateSurveyDto.Id,
                IsPublished = updateSurveyDto.IsPublished,
                CreatedAt = updateSurveyDto.CreatedAt,
                LastUpdateAt = updateSurveyDto.LastUpdateAt,
                ShortUrlId = updateSurveyDto.ShortUrlId
            };

            if (updateSurveyDto.Questions.Any())
            {
                var questions = updateSurveyDto.Questions.Select(QuestionMapper.ToEntity).ToList();
                survey.AddQuestions(questions);
            }

            return survey;
        }
    }
}
