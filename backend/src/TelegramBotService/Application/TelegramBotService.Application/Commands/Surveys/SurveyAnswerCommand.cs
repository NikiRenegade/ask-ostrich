using TelegramBotService.Application.Actions;
using TelegramBotService.Application.Interfaces;
using TelegramBotService.Application.Mappers;
using TelegramBotService.Domain.Sessions;
using TelegramBotService.Domain.Dto;

namespace TelegramBotService.Application.Commands.Surveys;

public class SurveyAnswerCommand : IUserCommand
{
    
    private readonly ISurveyApi _surveyApi;

    public SurveyAnswerCommand(ISurveyApi surveyApi)
    {
        _surveyApi = surveyApi;
    }
    
    public bool CanHandle(UserInput input, UserSession session)
        => session.SurveyState == SurveyState.InProgress
           && (input.Action?.StartsWith("survey.") == true || input.Text != null);

    public Task<AppResponse> HandleAsync(UserInput input, UserSession session)
    {
        var question = session.CurrentSurvey!.Questions
            .OrderBy(q => q.Order)
            .ElementAt(session.CurrentQuestionIndex);
        
        // ⬅️ Назад
        if (input.IsAction("survey.back"))
        {
            session.CurrentQuestionIndex--;
            RestoreMultiChoice(session);
            return Task.FromResult(SurveyRenderer.RenderQuestion(session));
        }
        
        if (input.IsAction("survey.next"))
        {
            if (session.CurrentMultiChoice == null)
            {
                return Task.FromResult(SurveyRenderer.RenderQuestion(session));
            }
            session.Answers[question.Id] = session.CurrentMultiChoice.ToList();
            session.CurrentMultiChoice.Clear();
            session.CurrentQuestionIndex++;
            return Task.FromResult(NextOrFinish(session));
        }
        
        if (question.Type == QuestionType.Text && input.Text != null)
        {
            session.Answers[question.Id] = input.Text;
            session.CurrentQuestionIndex++;
            return Task.FromResult(NextOrFinish(session));
        }
        if (input.Action.StartsWith("survey.select:"))
        {
            var value = input.Action.Split(':')[1];

            if (question.Type == QuestionType.SingleChoice)
            {
                session.Answers[question.Id] = value;
                session.CurrentQuestionIndex++;
                return Task.FromResult(NextOrFinish(session));
            }

            if (question.Type == QuestionType.MultipleChoice)
            {
                if (!session.CurrentMultiChoice.Add(value))
                    session.CurrentMultiChoice.Remove(value);

                return Task.FromResult(SurveyRenderer.RenderQuestion(session));
            }
        }

        return Task.FromResult(SurveyRenderer.RenderQuestion(session));
    }

    private AppResponse NextOrFinish(UserSession session)
    {
        if (session.CurrentQuestionIndex >= session.CurrentSurvey!.Questions.Count)
        {
            session.SurveyState = SurveyState.Completed;
            var dto = SurveyResultMapper.Map(session);
            var a = _surveyApi.SavePassedSurvey(dto);
            return new AppResponse { Text = "✅ Опрос завершён. Спасибо!", Actions = MenuActions.GetMenuActions()};
        }

        return SurveyRenderer.RenderQuestion(session);
    }

    private static void RestoreMultiChoice(UserSession session)
    {
        session.CurrentMultiChoice.Clear();

        var question = session.CurrentSurvey!.Questions
            .OrderBy(q => q.Order)
            .ElementAt(session.CurrentQuestionIndex);

        if (session.Answers.TryGetValue(question.Id, out var value)
            && value is List<string> list)
        {
            foreach (var v in list)
                session.CurrentMultiChoice.Add(v);
        }
    }
}