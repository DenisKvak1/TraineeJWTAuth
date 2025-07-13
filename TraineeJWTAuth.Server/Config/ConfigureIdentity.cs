using Microsoft.AspNetCore.Identity;
using WebApp.Data;
using WebApp.Identity;
using WebApp.Identity.Interfaces;
using WebApp.Identity.Services;


public static partial class Program
{
    public static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
    {
        // builder.Services.AddScoped<TokenService, JwtService>();
        builder.Services.AddScoped<TokenService, JwtDbService>();

        builder.Services.AddIdentityCore<AppUser>()
            .AddUserManager<AppUserManager>()
            .AddSignInManager<AppSignInManager>()
            .AddRoles<AppRole>()
            .AddRoleManager<AppRoleManager>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        return builder;
    }
}