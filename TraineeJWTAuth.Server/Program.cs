using TraineeJWTAuth.Server.Filters;

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
                            "https://localhost:53229",
                            "http://localhost:53229",
                            "https://127.0.0.1:53229",  
                            "http://127.0.0.1:53229"    
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
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowFrontend");

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}