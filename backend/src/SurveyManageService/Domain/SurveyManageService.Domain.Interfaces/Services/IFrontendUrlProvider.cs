namespace SurveyManageService.Domain.Services;

public interface IFrontendUrlProvider
{
    string GetSurveyShortUrl(string shortCode);
}