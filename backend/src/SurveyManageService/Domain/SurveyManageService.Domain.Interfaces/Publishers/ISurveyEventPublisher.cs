using SurveyManageService.Domain.Events;

namespace SurveyManageService.Domain.Interfaces.Publishers;

public interface ISurveyEventPublisher
{
    public Task PublishSurveyCreated(SurveyCreatedEvent surveyCreatedEvent);
    public Task PublishSurveyUpdated(SurveyUpdatedEvent surveyUpdatedEvent);
    public Task PublishSurveyDeleted(Guid surveyId);
}