using AIAssistantService.Domain.DTO;

namespace AIAssistantService.Application.Helpers
{
    internal class PromptGenerationHelper
    {
        public static string GeneratePrompt(string userPrompt, string currentSurveyJson, PromptType promptType)
        {
            BasePromptGenerator generator = promptType switch
            {
                PromptType.UpdateSurvey => new UpdateSurveyPromptGenerator(),
                PromptType.Ask => new AskPromptGenerator(),
                _ => throw new ArgumentOutOfRangeException(nameof(promptType), "Unsupported prompt type")
            };

            return generator.GenerateSurveyPrompt(userPrompt, currentSurveyJson);
        }
    }
}
