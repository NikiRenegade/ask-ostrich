namespace AIAssistantService.Application.Helpers
{
    public class AskPromptGenerator : BasePromptGenerator
    {
        protected override void AddPreparationInfo()
        {
            Prompt += 
                """
                You are an expert survey and quiz designer. Your task is to answer the user about the existing survey or relevant subjects. Ensure that the survey maintains clarity, relevance, and engagement for respondents.
                """;
        }

        protected override void AddMainTask()
        {
            Prompt += 
                """
                Your main task is to provide a clear and concise answer to the user's question regarding the existing survey or related topic. Ensure that your response is informative and directly addresses the user's inquiry.                
                """;
        }
    }
}
