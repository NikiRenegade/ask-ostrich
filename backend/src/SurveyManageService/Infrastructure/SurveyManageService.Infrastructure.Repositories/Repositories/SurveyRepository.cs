using Microsoft.EntityFrameworkCore;
using SurveyManageService.Domain.Entities;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Infrastructure.EntityFramework.Context;
using System;

namespace SurveyManageService.Infrastructure.Repositories.Repositories;

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
        var existingSurvey = await _dbContext.Surveys
            .FirstOrDefaultAsync(s => s.Id == survey.Id, cancellationToken);
        if (existingSurvey == null)
            return false;

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
        var entity = await _dbContext.Surveys
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (entity is null)
            return false;

        _dbContext.Surveys.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IList<Survey>> GetExistingByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Surveys
            .AsNoTracking()
            .Where(x => x.AuthorId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddWithShortUrlAsync(Survey survey, ShortUrl shortUrl, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.ShortUrls.AddAsync(shortUrl, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _dbContext.Surveys.AddAsync(survey, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await TryRemoveShortUrlIfExists(shortUrl.Id, cancellationToken);

            DetachEntity(survey);
            DetachEntity(shortUrl);
            throw;
        }
    }

    private async Task TryRemoveShortUrlIfExists(Guid shortUrlId, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.ShortUrls
            .FirstOrDefaultAsync(s => s.Id == shortUrlId, cancellationToken);

        if (existing != null)
        {
            _dbContext.ShortUrls.Remove(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private void DetachEntity(object entity)
    {
        var entry = _dbContext.Entry(entity);
        if (entry.State != EntityState.Detached)
            entry.State = EntityState.Detached;
    }
}
