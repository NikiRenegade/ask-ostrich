using SurveyResponseService.Domain.DTOs.Survey;
using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Application.Mappers
{
    public static class QuestionMapper
    {
        public static QuestionDto ToDto(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);

            return new QuestionDto
            {
                Id = question.Id,
                Type = question.Type,
                Title = question.Title,
                Order = question.Order,
                InnerText = question.InnerText,
                Options = question.Options.ToList()
            };
        }

        public static Question ToEntity(QuestionDto questionDto)
        {
            ArgumentNullException.ThrowIfNull(questionDto);

            var question = new Question(
                questionDto.Type,
                questionDto.Title,
                questionDto.Order,
                questionDto.InnerText
            );

            if (questionDto.Id != Guid.Empty)
            {
                question.Id = questionDto.Id;
            }

            if (questionDto.Options.Any())
            {
                question.AddOptions(questionDto.Options.ToList());
            }

            return question;
        }
    }
}
