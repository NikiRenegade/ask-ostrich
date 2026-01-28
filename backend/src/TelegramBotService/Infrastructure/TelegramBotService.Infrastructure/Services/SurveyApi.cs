using System.Net.Http.Json;
using TelegramBotService.Application.Interfaces;
using TelegramBotService.Domain.Dto;

namespace TelegramBotService.Infrastructure.Services;

public class SurveyApi : ISurveyApi
{
    private readonly HttpClient _http;

    public SurveyApi(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<SurveyDto?> GetSurvey(Guid surveyId)
    {
        var response = await _http.GetAsync($"/survey-response/api/Survey/{surveyId}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        return await response.Content.ReadFromJsonAsync<SurveyDto>();
    }

    public async Task<SurveyDto?> GetSurveyByShortCode(string shortCode)
    {
        var response = await _http.GetAsync($"/survey-manage/api/Survey/short/{shortCode}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        return await response.Content.ReadFromJsonAsync<SurveyDto>();
    }
    
    public async Task<List<PassedSurveyListItemDto>> GetPassedSurveys(Guid userId)
    {
        
        var response = await _http.GetAsync(
            $"/survey-response/api/SurveyResult/user-passed-surveys/{userId}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;
        
        var full = await response.Content.ReadFromJsonAsync<List<PassedSurveyDto>>();
        return full.Select(x => new PassedSurveyListItemDto
        {
            Id = x.Id,  
            SurveyId = x.SurveyId,
            Title = x.Title,
            DatePassed = x.DatePassed
        }).ToList();
    }

    public async Task SavePassedSurvey(SurveyPassDto dto)
    {
        var response = await _http.PostAsJsonAsync(
            "/survey-response/api/SurveyResult",
            dto);

        response.EnsureSuccessStatusCode();
    }
    
    public async Task<PassedSurveyDto?> GetPassedSurvey(Guid surveyId, Guid userId)
    {
        var response = await _http.GetAsync(
            $"/survey-response/api/SurveyResult/survey/{surveyId}/user/{userId}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        return await response.Content.ReadFromJsonAsync<PassedSurveyDto>();
    }
    
    public async Task<List<MySurveyDto>> GetMySurveys(Guid userId)
    {
        var response = await _http.GetAsync(
            $"/survey-manage/api/Survey/existing/{userId}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return null;

        var result = await response.Content.ReadFromJsonAsync<List<MySurveyDto>>();

        return result ?? new List<MySurveyDto>();
    }
}