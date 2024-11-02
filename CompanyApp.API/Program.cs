using Serilog.Events;
using Serilog;
using Microsoft.EntityFrameworkCore;
using CompanyApp.Database.Context;
using CompanyApp.Infrastructure.Initializers;

namespace CompanyApp.API
{
    /// <summary>
    /// Класс точки входа
    /// </summary>
    public class Program
    {
        private static IConfiguration _configuration;

        /// <summary>
        /// Точка входа
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            _configuration = builder.Configuration;
            Log.Logger = ConfigureLogger();
            builder.Host.UseSerilog();
            ConfigureServices(builder.Services);
            var app = builder.Build();
            Configure(app);

            await DbInit(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CompanyAppDbContext>(opt => opt
             .UseSqlite(_configuration.GetConnectionString("Sqlite"))
             .UseLazyLoadingProxies()
            );





            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                    .WithOrigins(_configuration["Ports:ClientPort"]!)
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                    builder.WithOrigins(_configuration["Ports:DockerClientPort"]!)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthorization();
            app.MapControllers();

            app.UseSerilogRequestLogging();
        }

        private static Serilog.ILogger ConfigureLogger() => new LoggerConfiguration()
            .ReadFrom.Configuration(_configuration)
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.File($"Logs/Info/Full-CompanyApp.API[ {DateTime.Now:yyyy-MM-dd}].log", LogEventLevel.Information)
            .WriteTo.File($"Logs/Error/Error-CompanyApp.API[ {DateTime.Now:yyyy-MM-dd}].log", LogEventLevel.Error)
            .CreateLogger();

        private static async Task DbInit(WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                IDbInitializer dbInitializer = services.GetRequiredService<IDbInitializer>();
                await dbInitializer.InitializeAsync();
            }
        }
    }
}
