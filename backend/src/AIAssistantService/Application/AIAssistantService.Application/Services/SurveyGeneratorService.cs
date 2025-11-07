using AIAssistantService.Application.Helpers;
using AIAssistantService.Domain.DTO;
using AIAssistantService.Domain.Interfaces.Services;
using System.Runtime.Serialization;
using System.Text.Json;

namespace AIAssistantService.Application.Services
{
    public class SurveyGeneratorService: ISurveyGeneratorService
    {
        private readonly ILLMChatApiService _chatService;

        public SurveyGeneratorService(ILLMChatApiService chatService)
        {
            _chatService = chatService;
        }

        public async Task<GeneratedSurveyDto> GenerateSurveyAsync(GenerateSurveyRequestDto request, CancellationToken cancellationToken = default)
        {
            string prompt = PromptGenerationHelper.GeneratePrompt(request.Prompt, request.CurrentSurveyJson, request.Type);
            var response = await _chatService.GetResponse(prompt, cancellationToken);

            response = response.Replace("```json", "").Replace("`","");

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new InvalidOperationException("LLM returned an empty response.");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<GeneratedSurveyDto>(response, options)
                         ?? throw new InvalidOperationException("Failed to deserialize survey from LLM response.");

            return result;
        }
    }
}
