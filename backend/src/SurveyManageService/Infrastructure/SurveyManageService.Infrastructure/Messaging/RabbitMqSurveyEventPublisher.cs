using SurveyManageService.Domain.Events;
using SurveyManageService.Domain.Interfaces.Publishers;

namespace SurveyManageService.Infrastructure.Messaging;

public class RabbitMqSurveyEventPublisher : ISurveyEventPublisher
{
    private readonly IEventPublisher _eventPublisher;
    private const string ExchangeName = "survey-events";


    public RabbitMqSurveyEventPublisher(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task PublishSurveyCreated(SurveyCreatedEvent surveyCreatedEvent)
    {
        return _eventPublisher.PublishAsync(
            surveyCreatedEvent,
            routingKey: "survey.created",
            exchangeName: ExchangeName
        );
    }

    public Task PublishSurveyUpdated(SurveyUpdatedEvent surveyUpdatedEvent)
    {
        return _eventPublisher.PublishAsync(
            surveyUpdatedEvent,
            routingKey: "survey.updated",
            exchangeName: ExchangeName
        );
    }

    public Task PublishSurveyDeleted(Guid surveyId)
    {
        return _eventPublisher.PublishAsync(
            surveyId,
            routingKey: "survey.deleted",
            exchangeName: ExchangeName
        );
    }
}