using TelegramBotService.Domain.Dto;

namespace TelegramBotService.Application.Interfaces;

public interface ISurveyApi
{
    Task<SurveyDto?> GetSurvey(Guid surveyId);
    Task<List<PassedSurveyListItemDto>> GetPassedSurveys(Guid userId);
    Task SavePassedSurvey(SurveyPassDto surveyPassDto);
    Task<PassedSurveyDto?> GetPassedSurvey(Guid surveyId, Guid userId);
    Task<List<MySurveyDto>> GetMySurveys(Guid userId);
}