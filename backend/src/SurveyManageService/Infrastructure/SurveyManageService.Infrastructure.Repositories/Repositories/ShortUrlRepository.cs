using Microsoft.EntityFrameworkCore;
using SurveyManageService.Domain.Entities;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Infrastructure.EntityFramework.Context;

namespace SurveyManageService.Infrastructure.Repositories.Repositories;

public class ShortUrlRepository: IShortUrlRepository
{
    private readonly SurveyManageDbContext _dbContext;

    public ShortUrlRepository(SurveyManageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<ShortUrl>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ShortUrls.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<ShortUrl?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ShortUrls.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(ShortUrl url, CancellationToken cancellationToken = default)
    {
        await _dbContext.ShortUrls.AddAsync(url, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> UpdateAsync(ShortUrl url, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Update operation for ShortUrls is not supported!");
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ShortUrls.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity is null)
        {
            return false;
        }
        _dbContext.ShortUrls.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ShortUrl?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ShortUrls.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Code.Equals(code), cancellationToken);
    }
}
