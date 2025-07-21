using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Identity.Context;

public class TokenIdentityContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public TokenIdentityContext(DbContextOptions options) : base(options)
    {
    }

    public TokenIdentityContext()
    {
    }

    public async Task AddRefreshToken(string token, Guid userId, int expiresIn)
    {
        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = DateTime.Now.AddSeconds(expiresIn)
        };
        await DeleteExpiredRefreshTokens(userId);
        RefreshTokens.Add(refreshToken);
        await SaveChangesAsync();
    }

    public async Task<(bool isValid, Guid userId)> VerifyRefreshToken(string refreshToken)
    {
        RefreshToken? token = await RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        if (token == null) return (false, Guid.Empty);
        await DeleteExpiredRefreshTokens(token.UserId);

        return (true, token.UserId);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<RefreshToken>()
            .HasIndex(rt => new { rt.Token });
        base.OnModelCreating(builder);
    }

    public async Task DeleteExpiredRefreshTokens(Guid userId)
    {
        await RefreshTokens
            .Where(x => x.UserId == userId && x.ExpiresAt < DateTime.Now)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteExpiredRefreshTokens()
    {
        await RefreshTokens
            .Where(x => x.ExpiresAt < DateTime.Now)
            .ExecuteDeleteAsync();
    }
}