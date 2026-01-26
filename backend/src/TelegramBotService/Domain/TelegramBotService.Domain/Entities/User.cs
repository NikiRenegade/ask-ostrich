namespace TelegramBotService.Domain.Entities;

public class User: BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public User()
    {
    }

    public User(Guid id, string userName, string firstName, string lastName)
    {
        Id =id;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }
}