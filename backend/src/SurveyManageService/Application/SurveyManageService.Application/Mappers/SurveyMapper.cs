using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Entities;
using SurveyManageService.Domain.Events;

namespace SurveyManageService.Application.Mappers;

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
            Questions = survey.Questions.Select(QuestionMapper.ToDto).ToList()
        };
    }

    public static Survey ToEntity(CreateSurveyDto createSurveyDto, User author)
    {
        ArgumentNullException.ThrowIfNull(createSurveyDto);

        var survey = new Survey(createSurveyDto.Title, createSurveyDto.Description, author.Id);

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

        var survey = new Survey(updateSurveyDto.Title, updateSurveyDto.Description, author.Id);
        survey.Id = updateSurveyDto.Id;
        survey.IsPublished = updateSurveyDto.IsPublished;
        survey.ShortUrl = updateSurveyDto.ShortUrl;
        
        if (updateSurveyDto.Questions.Any())
        {
            var questions = updateSurveyDto.Questions.Select(QuestionMapper.ToEntity).ToList();
            survey.AddQuestions(questions);
        }

        return survey;
    }

    public static SurveyCreatedEvent ToSurveyCreatedEvent(this Survey source) => new()
    {
        Id = source.Id,
        Title = source.Title,
        Description = source.Description,
        AuthorId = source.AuthorId,
        CreatedAt = source.CreatedAt,
        LastUpdateAt = source.LastUpdateAt,
        IsPublished = source.IsPublished,
        ShortUrl = source.ShortUrl,
        Questions = source.Questions.Select(QuestionMapper.ToDto).ToList()
    };

    public static SurveyUpdatedEvent ToSurveyUpdatedEvent(this Survey source, Survey old) => new()
    {
        Id = source.Id,
        Title = source.Title,
        Description = source.Description,
        AuthorId = source.AuthorId,
        CreatedAt = source.CreatedAt,
        LastUpdateAt = source.LastUpdateAt,
        IsPublished = source.IsPublished,
        ShortUrl = source.ShortUrl,
        Questions = source.Questions.Select(QuestionMapper.ToDto).ToList(),
        Changes = new()
        {
            { nameof(source.Title), old.Title },
            { nameof(source.Description), old.Description },
            { nameof(source.IsPublished), old.IsPublished },
            { nameof(source.Questions), old.Questions }
        }
    };
}
