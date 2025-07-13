public static partial class Program
{

    public static WebApplicationBuilder ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddAuthorization(options => {
            options.AddPolicy("RequireModerator",
                policy => policy.RequireRole("Administrator", "Moderator").RequireAuthenticatedUser());

            options.AddPolicy("RequireAdministrator",
                policy => policy.RequireRole("Administrator").RequireAuthenticatedUser());
        });

        builder.Services.AddAuthorization(options => {
            options.AddPolicy("RequireModerator",
                policy => policy.RequireRole("Administrator", "Moderator").RequireAuthenticatedUser());

            options.AddPolicy("RequireAdministrator",
                policy => policy.RequireRole("Administrator").RequireAuthenticatedUser());
        });
        
        return builder;
    }
}