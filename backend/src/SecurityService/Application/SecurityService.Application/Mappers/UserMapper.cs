using SecurityService.Domain.Entities;
using SecurityService.Domain.Events;

namespace SecurityService.Application.Mappers;

public static class UserMapper
{
    public static UserCreatedEvent ToUserCreatedEvent(this User source) => new UserCreatedEvent
    {
        Id = source.Id,
        Email = source.Email!,
        UserName = source.UserName!,
        FirstName = source.FirstName,
        LastName = source.LastName,
    };
}