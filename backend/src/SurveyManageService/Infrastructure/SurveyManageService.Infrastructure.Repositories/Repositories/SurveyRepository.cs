using Microsoft.EntityFrameworkCore;
using SurveyManageService.Domain.Entities;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Infrastructure.EntityFramework;

namespace SurveyManageService.Infrastructure.Repositories;

public class SurveyRepository: ISurveyRepository
{
    private readonly SurveyManageDbContext _dbContext;

    public SurveyRepository(SurveyManageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Survey>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Surveys.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Survey?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Surveys.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(Survey survey, CancellationToken cancellationToken = default)
    {
        await _dbContext.Surveys.AddAsync(survey, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Survey survey, CancellationToken cancellationToken = default)
    {
        _dbContext.Surveys.Update(survey);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Surveys.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity is null)
        {
            return;
        }
        _dbContext.Surveys.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
