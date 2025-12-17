using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Application.Mappers;

public static class ShortUrlMapper
{
    public static ShortUrl ToEntity(CreateShortUrlDto request, string code)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new ShortUrl()
        {
            Code = code,
            OriginUrl = request.OriginUrl
        };
    }
    
    public static ShortUrl ToEntity(string code, string originUrl)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(originUrl);

        return new ShortUrl()
        {
            Code = code,
            OriginUrl = originUrl
        };
    }

    public static ShortUrlDto? ToDto(ShortUrl shortUrl)
    {
        ArgumentNullException.ThrowIfNull(shortUrl);
        return new ShortUrlDto
        {
            Id = shortUrl.Id,
            Code = shortUrl.Code,
            OriginUrl = shortUrl.OriginUrl
        };
    }
}
