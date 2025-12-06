namespace SurveyResponseService.Domain.Interfaces.Consumers;

public interface ISurveyEventConsumer
{
    Task StartAsync();
}