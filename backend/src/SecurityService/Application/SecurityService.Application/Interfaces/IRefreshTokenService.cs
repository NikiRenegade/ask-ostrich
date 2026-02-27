public interface IRefreshTokenService
{
    string GenerateToken();
    string HashToken(string token);
    Task<string> CreateAsync(Guid userId);
    Task<RefreshToken?> ValidateAsync(string rawToken);
    Task RevokeAsync(string rawToken);
}