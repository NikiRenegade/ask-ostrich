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
        if (survey.Author != null)
        {
            _dbContext.Entry(survey.Author).State = EntityState.Unchanged;
        }
        await _dbContext.Surveys.AddAsync(survey, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Survey survey, CancellationToken cancellationToken = default)
    {
        var existingSurvey = await _dbContext.Surveys.AsNoTracking().FirstOrDefaultAsync(s => s.Id == survey.Id, cancellationToken);
        if (existingSurvey == null)
        {
            return false;
        }

        _dbContext.Surveys.Update(survey);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Surveys.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity is null)
        {
            return false;
        }
        _dbContext.Surveys.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
