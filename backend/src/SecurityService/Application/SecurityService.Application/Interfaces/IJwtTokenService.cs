namespace SecurityService.Application.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJwtToken(string email);
    }
}
