using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Interfaces.Repositories;

namespace SecurityService.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public JwtTokenService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateJwtToken(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) throw new Exception("User not found");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("userId", user.Id.ToString())
            };

            var jwtKey = _configuration["JWT:PRIVATE_KEY"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT private key not configured");

            var rsa = RSA.Create();
            rsa.ImportFromPem(jwtKey.Replace("\\n", "\n"));
            

            var key = new RsaSecurityKey(rsa);
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
