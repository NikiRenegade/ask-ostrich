using SurveyResponseService.Application.Mappers;
using SurveyResponseService.Domain.DTOs.Users;
using SurveyResponseService.Domain.Interfaces.Repositories;
using SurveyResponseService.Domain.Interfaces.Services;

namespace SurveyResponseService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await _repository.GetAllAsync(cancellationToken);
            return users.Select(UserMapper.ToDto).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);
            return user != null ? UserMapper.ToDto(user) : null;
        }

        public async Task<UserCreatedDto> AddAsync(CreateUserDto request, CancellationToken cancellationToken = default)
        {
            var user = UserMapper.ToEntity(request);
            await _repository.AddAsync(user, cancellationToken);

            return new UserCreatedDto { Id = user.Id };
        }

        public async Task<bool> UpdateAsync(UpdateUserDto request, CancellationToken cancellationToken = default)
        {
            var updatedUser = UserMapper.ToEntity(request);
            return await _repository.UpdateAsync(updatedUser, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _repository.DeleteAsync(id, cancellationToken);
        }
    }
}
