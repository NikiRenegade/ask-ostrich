using SecurityService.Domain.Entities;

namespace SecurityService.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
    }

}
