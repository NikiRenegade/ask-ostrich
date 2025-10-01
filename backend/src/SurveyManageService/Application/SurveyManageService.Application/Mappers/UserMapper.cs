using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Application.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public static User ToEntity(CreateUserDto createUserDto)
    {
        if (createUserDto == null)
            throw new ArgumentNullException(nameof(createUserDto));

        return new User(
            createUserDto.UserName,
            createUserDto.Email,
            createUserDto.FirstName,
            createUserDto.LastName
        );
    }

    public static User ToEntity(UpdateUserDto updateUserDto)
    {
        if (updateUserDto == null)
            throw new ArgumentNullException(nameof(updateUserDto));

        var user = new User(
            updateUserDto.UserName,
            updateUserDto.Email,
            updateUserDto.FirstName,
            updateUserDto.LastName
        );

        user.Id = updateUserDto.Id;

        return user;
    }
}
