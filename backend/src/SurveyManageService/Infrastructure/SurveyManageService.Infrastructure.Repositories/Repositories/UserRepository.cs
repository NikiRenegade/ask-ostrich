using Microsoft.EntityFrameworkCore;
using SurveyManageService.Domain.Entities;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Infrastructure.EntityFramework;

namespace SurveyManageService.Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly SurveyManageDbContext _dbContext;

    public UserRepository(SurveyManageDbContext dbContext)
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

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }
        _dbContext.Users.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
