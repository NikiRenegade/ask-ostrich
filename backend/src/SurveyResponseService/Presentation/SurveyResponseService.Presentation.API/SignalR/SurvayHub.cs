using Microsoft.AspNetCore.SignalR;
using SurveyResponseService.Domain.Interfaces.Services;

namespace SurveyResponseService.Presentation.API.SignalR
{
    public class SurveyHub : Hub
    {
        private readonly ISurveyResultService _surveyResultService;

        public SurveyHub(ISurveyResultService surveyResultService)
        {
            _surveyResultService = surveyResultService;
        }

        public async Task RequestSurveyResults(Guid surveyId)
        {
            var results = await _surveyResultService.GetBySurveyIdAsync(surveyId, CancellationToken.None);
            await Clients.Caller.SendAsync("SurveyResultsUpdated", results);
        }

        public async Task JoinSurveyGroup(Guid surveyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, surveyId.ToString());
        }

        public async Task LeaveSurveyGroup(Guid surveyId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, surveyId.ToString());
        }
    }

}
