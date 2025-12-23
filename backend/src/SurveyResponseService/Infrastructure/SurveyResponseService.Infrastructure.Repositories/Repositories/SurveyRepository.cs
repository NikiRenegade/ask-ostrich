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
            var survey = await _dbContext.Surveys.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (survey != null)
            {
                survey.Author = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == survey.AuthorId, cancellationToken);
            }
            return survey;
        }

        public async Task AddAsync(Survey survey, CancellationToken cancellationToken = default)
        {
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

            existingSurvey.Title = survey.Title;
            existingSurvey.Description = survey.Description;
            existingSurvey.IsPublished = survey.IsPublished;
            existingSurvey.AuthorId = survey.AuthorId;
            existingSurvey.LastUpdateAt = survey.LastUpdateAt;
            existingSurvey.ShortUrlId = survey.ShortUrlId;

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
