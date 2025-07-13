using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApp.Identity.Context;
using WebApp.Identity.Interfaces;

namespace WebApp.Identity.Services;

public class JwtDbService(IConfiguration _configuration, TokenIdentityContext _dbContext) : TokenService
{
    public string GenerateAccessToken(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var ttl = jwtSettings.GetValue<int>("accessTokenLifetime");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("id", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(ttl),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(Guid userId)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        byte[] tokenBytes = new byte[32];
        RandomNumberGenerator.Fill(tokenBytes);
        string token = Convert.ToBase64String(tokenBytes);
        await _dbContext.AddRefreshToken(token, userId, jwtSettings.GetValue<int>("refreshTokenLifetime"));

        return token;
    }

    public Task<(bool isValid, Guid userId)> VerifyRefreshToken(string refreshToken)
    {
        return _dbContext.VerifyRefreshToken(refreshToken);
    }
}