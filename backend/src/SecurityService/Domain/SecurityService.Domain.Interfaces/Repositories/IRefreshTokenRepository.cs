public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByHashAsync(string hash);
    Task SaveChangesAsync();
}