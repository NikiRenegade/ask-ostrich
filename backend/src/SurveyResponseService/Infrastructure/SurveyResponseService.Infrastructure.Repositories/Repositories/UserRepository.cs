using Microsoft.EntityFrameworkCore;
using SurveyResponseService.Domain.Entities;
using SurveyResponseService.Domain.Interfaces.Repositories;
using SurveyResponseService.Infrastructure.EntityFramework;

namespace SurveyResponseService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SurveyResponseDbContext _dbContext;

        public UserRepository(SurveyResponseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var existingUser = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            if (existingUser == null)
            {
                return false;
            }

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (entity is null)
            {
                return false;
            }
            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}