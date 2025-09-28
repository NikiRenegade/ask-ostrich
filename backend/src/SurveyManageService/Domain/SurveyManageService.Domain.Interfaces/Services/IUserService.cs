using SurveyManageService.Domain.DTO;

namespace SurveyManageService.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IList<UserDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<UserCreatedDto> AddAsync(CreateUserDto request, CancellationToken cancellationToken);

    Task UpdateAsync(UpdateUserDto request, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
