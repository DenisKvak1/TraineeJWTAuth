namespace WebApp.Identity.Interfaces;

public interface TokenService
{
    string GenerateAccessToken(AppUser user);
    Task<string> GenerateRefreshToken(Guid userId);
    Task<(bool isValid, Guid userId)> VerifyRefreshToken(string refreshToken);
}