namespace SurveyManageService.Domain.Interfaces.Consumers;

public interface IUserEventConsumer
{
    Task StartAsync();
}