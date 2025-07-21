using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Identity.BackgroundServices;
using WebApp.Identity.Context;


public static partial class Program
{
    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        string? connStr = builder.Configuration.GetConnectionString("DefaultConnStr");
        if (connStr == null)
        {
            return builder;
        }
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(connStr));
        builder.Services.AddDbContext<TokenIdentityContext, AppDbContext>(options =>
            options.UseMySQL(connStr));
        builder.Services.AddHostedService<RefreshTokenCleanupService>();

        return builder;
    }
}