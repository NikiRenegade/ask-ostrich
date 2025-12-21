using SecurityService.Domain.Entities;

namespace SecurityService.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
    }
}
