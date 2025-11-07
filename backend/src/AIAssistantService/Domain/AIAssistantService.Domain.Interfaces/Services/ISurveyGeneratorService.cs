using AIAssistantService.Domain.DTO;

namespace AIAssistantService.Domain.Interfaces.Services
{
    public interface ISurveyGeneratorService
    {
        Task<GeneratedSurveyDto> GenerateSurveyAsync(GenerateSurveyRequestDto request, CancellationToken cancellationToken = default);
    }
}
