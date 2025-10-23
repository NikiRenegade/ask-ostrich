using AIAssistantService.Domain.DTO;

namespace AIAssistantService.Domain.Interfaces.Services
{
    public interface ISurveyGeneratorService
    {
        Task<GeneratedSurveyDto> GenerateSurveyAsync(string userPrompt, CancellationToken cancellationToken = default);
    }
}
