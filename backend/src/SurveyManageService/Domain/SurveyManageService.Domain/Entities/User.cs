namespace SurveyManageService.Domain.Entities;

public class User: BaseEntity
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public User(string userName, string email, string firstName, string lastName)
    {
        UserName = userName;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}