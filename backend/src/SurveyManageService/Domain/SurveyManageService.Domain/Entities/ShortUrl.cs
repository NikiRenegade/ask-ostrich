namespace SurveyManageService.Domain.Entities;

public class ShortUrl : BaseEntity
{
    private const string __codeChars = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    private const int __codeLength = 6;

    public string Code { get; set; } = string.Empty;

    public string OriginUrl { get; set; } = string.Empty;

    public ShortUrl()
    {
        Id = Guid.NewGuid();
    }

    public static string GenerateCode()
    {
        var random = new Random();
        var chars = new char[__codeLength];
        for (int i = 0; i < __codeLength; i++)
        {
            chars[i] = __codeChars[random.Next(__codeChars.Length)];
        }
        return new string(chars);
    }
}