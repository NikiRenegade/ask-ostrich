using SurveyManageService.Application.Mappers;
using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Entities;
using SurveyManageService.Domain.Interfaces.Publishers;
using SurveyManageService.Domain.Interfaces.Repositories;
using SurveyManageService.Domain.Interfaces.Services;

namespace SurveyManageService.Application.Services;

public class ShortUrlService : IShortUrlService
{
    private readonly IShortUrlRepository _repository;
    private readonly ISurveyRepository _surveyRepository;
    private const int __generateCodeAttempts = 10;

    public ShortUrlService(
        IShortUrlRepository shortUrlRepository,
        ISurveyRepository surveyRepository)
    {
        _repository = shortUrlRepository;
        _surveyRepository = surveyRepository;
    }

    public async Task<ShortUrlDto?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        var shortUrl = await _repository.GetByCodeAsync(code, cancellationToken);
        return shortUrl != null ? ShortUrlMapper.ToDto(shortUrl) : null;
    }

    public async Task<ShortUrlDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var shortUrl = await _repository.GetByIdAsync(id, cancellationToken);
        return shortUrl != null ? ShortUrlMapper.ToDto(shortUrl) : null;
    }

    public async Task<ShortUrlCreatedDto> AddAsync(CreateShortUrlDto request, CancellationToken cancellationToken)
    {
        var survey = await _surveyRepository.GetByIdAsync(request.SurveyId, cancellationToken)
            ?? throw new ArgumentException("Survey not found", nameof(request.SurveyId));

        string code = await GenerateShortCode(cancellationToken);

        var shortUrl = ShortUrlMapper.ToEntity(request, code);
        await _repository.AddAsync(shortUrl, cancellationToken);

        return new ShortUrlCreatedDto 
        { 
            Id = shortUrl.Id,
            Code = shortUrl.Code
        };
    }

    public async Task<string> GenerateShortCode(CancellationToken cancellationToken)
    {
        string code = string.Empty;

        for (int i = 0; i <= __generateCodeAttempts; i++)
        {
            if (i == __generateCodeAttempts)
                throw new InvalidOperationException("Failed to generate short url");

            code = ShortUrl.GenerateCode();
            var existsCode = await _repository.GetByCodeAsync(code, cancellationToken);

            if (existsCode == null)
                break;
        }
        return code;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
