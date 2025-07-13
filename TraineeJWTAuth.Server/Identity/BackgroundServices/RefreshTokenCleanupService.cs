using WebApp.Identity.Context;

namespace WebApp.Identity.BackgroundServices;

public class RefreshTokenCleanupService(IServiceScopeFactory _scopeFactory) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TokenIdentityContext>();
        await dbContext.DeleteExpiredRefreshTokens();
        await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
    }
}