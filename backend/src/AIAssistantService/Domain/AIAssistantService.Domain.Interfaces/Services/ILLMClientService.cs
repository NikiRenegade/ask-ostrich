using AIAssistantService.Domain.DTO;

namespace AIAssistantService.Domain.Interfaces.Services
{
    public interface ILLMClientService
    {
        Task<GeneratedSurveyDto> GenerateSurveyAsync(string prompt, string currentSurveyJson, CancellationToken cancellationToken = default);
        Task<string> AskLLMAsync(string prompt, string currentSurveyJson, CancellationToken cancellationToken = default);
    }
}

