using AIAssistantService.Application.Helpers;
using AIAssistantService.Domain.DTO;
using AIAssistantService.Domain.Interfaces.Services;
using System.Runtime.Serialization;
using System.Text.Json;

namespace AIAssistantService.Application.Services
{
    public class LLMClientService: ILLMClientService
    {
        private readonly ILLMChatApiService _chatService;

        public LLMClientService(ILLMChatApiService chatService)
        {
            _chatService = chatService;
        }

        public async Task<GeneratedSurveyDto> GenerateSurveyAsync(string prompt, string currentSurveyJson, CancellationToken cancellationToken = default)
        {
            string generatedPrompt = PromptGenerationHelper.GeneratePrompt(prompt, currentSurveyJson, PromptType.UpdateSurvey);
            var response = await _chatService.GetResponse(generatedPrompt, cancellationToken);

            response = response.Replace("```json", "").Replace("`","");

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new InvalidOperationException("LLM returned an empty response.");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            GeneratedSurveyDto? result = new();
            try
            {
                result = JsonSerializer.Deserialize<GeneratedSurveyDto>(response, options);
                    
            } catch (Exception ex) 
            {
                throw new SerializationException("Failed to deserialize survey from LLM response.", ex);
            }

            return result!;
        }

        public async Task<string> AskLLMAsync(string prompt, string currentSurveyJson, CancellationToken cancellationToken = default)
        {
            string generatedPrompt = PromptGenerationHelper.GeneratePrompt(prompt, currentSurveyJson, PromptType.Ask);
            var response = await _chatService.GetResponse(generatedPrompt, cancellationToken);

            if (string.IsNullOrWhiteSpace(response))
            {
                throw new InvalidOperationException("LLM returned an empty response.");
            }

            return response;
        }

        public async IAsyncEnumerable<string> AskLLMStreamAsync(string prompt, string currentSurveyJson, CancellationToken cancellationToken = default)
        {
            string generatedPrompt = PromptGenerationHelper.GeneratePrompt(prompt, currentSurveyJson, PromptType.Ask);
            
            await foreach (var responseChunk in _chatService.GetResponseStream(generatedPrompt, cancellationToken))
            {
                if (!string.IsNullOrWhiteSpace(responseChunk))
                {
                    yield return responseChunk;
                }
            }
        }
    }
}

