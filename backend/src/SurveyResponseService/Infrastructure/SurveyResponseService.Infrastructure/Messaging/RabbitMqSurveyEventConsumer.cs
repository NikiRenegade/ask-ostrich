using SurveyResponseService.Domain.DTOs.Survey;
using SurveyResponseService.Domain.Interfaces.Consumers;
using SurveyResponseService.Domain.Interfaces.Services;

namespace SurveyResponseService.Infrastructure.Messaging;

public class RabbitMqSurveyEventConsumer : ISurveyEventConsumer
{
    private readonly IEventConsumer _consumer;
    private readonly ISurveyService _surveyService;
    private const string ExchangeName = "survey-events";

    public RabbitMqSurveyEventConsumer(IEventConsumer consumer, ISurveyService surveyService)
    {
        _consumer = consumer;
        _surveyService = surveyService;
    }

    public async Task StartAsync()
    {
        await _consumer.SubscribeAsync<SurveyDto>("survey.created", ExchangeName, async @event =>
        {
            await _surveyService.AddAsync(@event, CancellationToken.None);
        });

        await _consumer.SubscribeAsync<UpdateSurveyDto>("survey.updated", ExchangeName, async @event =>
        {
            await _surveyService.UpdateAsync(@event, CancellationToken.None);
        });

        await _consumer.SubscribeAsync<Guid>("survey.deleted", ExchangeName, async @event =>
        {
            await _surveyService.DeleteAsync(@event, CancellationToken.None);
        });
    }
}