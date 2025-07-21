using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApp.Identity.Interfaces;

namespace WebApp.Identity.Services;

public class JwtService(IConfiguration _configuration) : TokenService
{
    public string GenerateAccessToken(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var ttl = jwtSettings.GetValue<int>("accessTokenLifetime");

        var claims = new[]
        {
            new Claim("token_type", "access"),
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
        var ttl = jwtSettings.GetValue<int>("refreshTokenLifetime");

        var claims = new[]
        {
            new Claim("token_type", "refresh"),
            new Claim("id", userId.ToString())
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

    public async Task<(bool isValid, Guid userId)> VerifyRefreshToken(string refreshToken)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };

        try
        {
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);
            if (principal.FindFirstValue("token_type") != "refresh") return (isValid: false, id: Guid.Empty);
            Guid userId = Guid.Parse(principal.FindFirstValue("id"));

            return (isValid: true, userId: userId);
        }
        catch (SecurityTokenException ex)
        {
            return (isValid: false, userId: Guid.Empty);
        }
    }
}