using Microsoft.EntityFrameworkCore;
using SurveyResponseService.Domain.Entities;
using SurveyResponseService.Domain.Interfaces.Repositories;
using SurveyResponseService.Infrastructure.EntityFramework;

namespace SurveyResponseService.Infrastructure.Repositories
{
    public class SurveyResultRepository : ISurveyResultRepository
    {
        private readonly SurveyResponseDbContext _dbContext;

        public SurveyResultRepository(SurveyResponseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<SurveyResult>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SurveyResults
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<SurveyResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SurveyResults
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task AddAsync(SurveyResult surveyResult, CancellationToken cancellationToken = default)
        {
            await _dbContext.SurveyResults.AddAsync(surveyResult, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UpdateAsync(SurveyResult surveyResult, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.SurveyResults
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == surveyResult.Id, cancellationToken);

            if (existing == null)
                return false;

            _dbContext.SurveyResults.Update(surveyResult);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.SurveyResults
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity is null)
                return false;

            _dbContext.SurveyResults.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IList<SurveyResult>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SurveyResults
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<SurveyResult>> GetBySurveyIdAsync(Guid surveyId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SurveyResults
                .AsNoTracking()
                .Where(r => r.SurveyId == surveyId)
                .ToListAsync(cancellationToken);
        }
    }
}
