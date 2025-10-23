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

        public async Task<GeneratedSurveyDto> GenerateSurveyAsync(string userPrompt, CancellationToken cancellationToken = default)
        {
            string systemMessage = """
            Transform the user’s request into a survey or quiz structure in JSON format.

            The survey must include the following fields:

            title: The name of the survey.
            description: A brief description of the survey.
            questions: An array of questions. Each question must have:
            type: 0 for Text input, 1 for SingleChoice, 2 for MultipleChoice
            title: The main question text.
            order: The order number of the question.
            innerText: Additional description or help text.
            options: For SingleChoice or MultipleChoice, a list of answer options. Each option must include:
            title: Option text
            value: Unique (machine-readable) value for the option
            order: Option order number
            isCorrect: true if the answer must be chosen as correct one, may be not set if the question doesn’t have any particular right answer.
            Return ONLY valid JSON according to this schema, with no additional comments or explanations.
            Your answer must be in the same language as the user’s request.

            If the user’s request doesn’t specify details, make reasonable assumptions.

            Your task: Given any user prompt about creating a survey, output the survey in valid JSON according to the above schema and rules. 
            User prompt:
            """;

            var response = await _chatService.GetResponse(systemMessage + userPrompt, cancellationToken);

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
