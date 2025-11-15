namespace SurveyResponseService.Domain.Interfaces.Consumers;

public interface IUserEventConsumer
{
    Task StartAsync();
}