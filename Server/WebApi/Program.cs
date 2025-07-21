using Microsoft.EntityFrameworkCore;
using TraineeJWTAuth.Server.Filters;
using WebApp.Data;

namespace TraineeJWTAuth.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://trainspa.ua.local:8080",
                            "http://trainspa.ua.local:8081",
                            "http://trainspa.ua.local:8082" 
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); 
                });
            });
            builder.Services.AddControllers(options => { options.Filters.Add<ResponseFilter>(); });
            builder.ConfigureDatabase();
            builder.ConfigureIdentity();
            builder.ConfigureAuthentication();
            builder.ConfigureAuthorization();

            var app = builder.Build();
            app.Use(async (context, next) =>
            {
                // Логируем заголовки запроса
                Console.WriteLine($"Request method: {context.Request.Method}");
                Console.WriteLine("Request headers:");
                foreach (var header in context.Request.Headers)
                {
                    Console.WriteLine($"{header.Key}: {header.Value}");
                }

                await next();

                // Логируем заголовки ответа
                Console.WriteLine("Response headers:");
                foreach (var header in context.Response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {header.Value}");
                }
            });
            app.UseDefaultFiles();
            // app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();
            
            // WARNING КОООСТЫЛЬЬ
            using (var scope = app.Services.CreateScope())
            {
                Thread.Sleep(3000);
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<AppDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            // WARNING КОООСТЫЛЬЬ

            
            app.Run();
        }
    }
}