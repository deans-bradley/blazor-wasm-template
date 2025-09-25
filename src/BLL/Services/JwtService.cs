using Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL.Services.Interfaces;

namespace BLL.Services
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly string? _secretKey = configuration["JwtSettings:SecretKey"];

        public string GenerateJwtToken(User user)
        {
            Claim[] claims =
            [
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim("exp", DateTime.Now.AddDays(365).ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secretKey ?? throw new NullReferenceException()));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                claims: claims,
                expires: DateTime.Now.AddDays(365),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}