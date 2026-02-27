using System.Security.Cryptography;
using System.Text;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _repository;

    public RefreshTokenService(IRefreshTokenRepository repository)
    {
        _repository = repository;
    }
    
    public string GenerateToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
    
    public string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<string> CreateAsync(Guid userId)
    {
        var rawToken = GenerateToken();
        var hash = HashToken(rawToken);

        var refreshToken = new RefreshToken
        {
            UserId = userId,
            TokenHash = hash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(3),
            IsRevoked = false
        };

        await _repository.AddAsync(refreshToken);
        await _repository.SaveChangesAsync();

        return rawToken;
    }

    public async Task<RefreshToken?> ValidateAsync(string rawToken)
    {
        var hash = HashToken(rawToken);

        var token = await _repository.GetByHashAsync(hash);
        if (token == null)
            return null;

        if (token.IsRevoked)
            return null;

        if (token.ExpiresAt <= DateTime.UtcNow)
            return null;

        return token;
    }

    public async Task RevokeAsync(string rawToken)
    {
        var hash = HashToken(rawToken);

        var token = await _repository.GetByHashAsync(hash);
        if (token == null)
            return;

        token.IsRevoked = true;

        await _repository.SaveChangesAsync();
    }
}