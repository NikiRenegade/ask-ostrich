using Microsoft.EntityFrameworkCore;
using SurveyResponseService.Domain.Entities;
using SurveyResponseService.Domain.Interfaces.Repositories;
using SurveyResponseService.Infrastructure.EntityFramework;

namespace SurveyResponseService.Infrastructure.Repositories
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly SurveyResponseDbContext _dbContext;

        public SurveyRepository(SurveyResponseDbContext dbContext)
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
            var existingSurvey = await _dbContext.Surveys.FirstOrDefaultAsync(s => s.Id == survey.Id, cancellationToken);
            if (existingSurvey == null)
            {
                return false;
            }

            // Update properties manually to avoid shadow property issues
            existingSurvey.Title = survey.Title;
            existingSurvey.Description = survey.Description;
            existingSurvey.IsPublished = survey.IsPublished;
            existingSurvey.Author = survey.Author;
            existingSurvey.LastUpdateAt = DateTime.Now;
            existingSurvey.ShortUrl = survey.ShortUrl;

            // Update questions and their options
            existingSurvey.UpdateQuestions(survey.Questions.ToList());

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
}
