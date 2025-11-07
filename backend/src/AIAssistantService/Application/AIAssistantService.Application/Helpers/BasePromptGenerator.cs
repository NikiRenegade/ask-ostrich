namespace AIAssistantService.Application.Helpers
{
    public abstract class BasePromptGenerator
    {
        protected string Prompt { get; set; } = string.Empty;

        public string GenerateSurveyPrompt(string userPrompt, string currentSurveyJson)
        {
            Prompt = string.Empty;

            AddPreparationInfo();
            AddCurrentSurveyState(currentSurveyJson);
            AddStructureExplanation();
            AddMainTask();
            AddUserPrompt(userPrompt);

            return Prompt;
        }

        protected abstract void AddPreparationInfo();

        protected void AddCurrentSurveyState(string currentSurveyJson)
        {
            Prompt += $"""
                You are provided with an existing survey in JSON format:
                {currentSurveyJson}                                           
            """;
        }

        protected virtual void AddStructureExplanation()
        {
            // TODO: use reflection
            Prompt += $"""
                The structure of the survey includes the following fields:
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
            """;
        }

        protected abstract void AddMainTask();

        protected virtual void AddUserPrompt(string userPrompt)
        {
            Prompt += $"""
                If the user’s request doesn’t specify details, make reasonable assumptions.
                User request:
                {userPrompt}
            """;
        }
    }
}
