namespace AIAssistantService.Application.Helpers
{
    public class UpdateSurveyPromptGenerator : BasePromptGenerator
    {
        protected override void AddPreparationInfo()
        {
            Prompt += 
                $"""
                You are an expert survey and quiz designer. Your task is to update and improve the existing survey based on the user's request. Ensure that the updated survey maintains clarity, relevance, and engagement for respondents.
                """;
        }

        protected override void AddMainTask()
        {
            Prompt += 
                $"""
                Your main task is to modify the existing survey according to the user's instructions. This may involve adding new questions, removing irrelevant ones, rephrasing questions for better clarity, or adjusting the survey structure to enhance the flow and respondent experience.
                Convert all the question types to their respective numeric representations as described earlier.
                Return ONLY valid JSON according to this schema, with no additional comments or explanations.                .
                """;
        }
    }
}