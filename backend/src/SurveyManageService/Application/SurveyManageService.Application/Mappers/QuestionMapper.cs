using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Application.Mappers;

public static class QuestionMapper
{
    public static QuestionDto ToDto(Question question)
    {
        if (question == null)
            throw new ArgumentNullException(nameof(question));

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

    public static Question ToEntity(CreateQuestionDto questionDto)
    {
        if (questionDto == null)
            throw new ArgumentNullException(nameof(questionDto));

        var question = new Question(
            questionDto.Type,
            questionDto.Title,
            questionDto.Order,
            questionDto.InnerText
        );

        if (questionDto.Options.Any())
        {
            question.AddOptions(questionDto.Options.ToList());
        }

        return question;
    }

    public static Question ToEntity(QuestionDto questionDto)
    {
        if (questionDto == null)
            throw new ArgumentNullException(nameof(questionDto));

        var question = new Question(
            questionDto.Type,
            questionDto.Title,
            questionDto.Order,
            questionDto.InnerText
        );

        question.Id = questionDto.Id;

        if (questionDto.Options.Any())
        {
            question.AddOptions(questionDto.Options.ToList());
        }

        return question;
    }
}
