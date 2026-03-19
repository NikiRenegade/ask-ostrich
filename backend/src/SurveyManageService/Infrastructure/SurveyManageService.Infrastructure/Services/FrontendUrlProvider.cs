using SurveyManageService.Domain.Services;

namespace SurveyManageService.Infrastructure.Services;

public class FrontendUrlProvider(string baseUrl) 
    : IFrontendUrlProvider
{
    public string GetSurveyShortUrl(string shortCode) =>
        $"{baseUrl}/{shortCode}";
}