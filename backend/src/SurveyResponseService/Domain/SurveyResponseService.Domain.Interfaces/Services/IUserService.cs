using SurveyResponseService.Domain.DTOs.Users;

namespace SurveyResponseService.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<IList<UserDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<UserCreatedDto> AddAsync(CreateUserDto request, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(UpdateUserDto request, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
