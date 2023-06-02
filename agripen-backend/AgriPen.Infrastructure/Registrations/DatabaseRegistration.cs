using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AgriPen.Infrastructure.Registrations;

public static class DatabaseRegistration
{
    public static IServiceCollection AddCustomDatabaseContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddDbContext<AgriDataContext>(options =>
        {
            options.UseSnakeCaseNamingConvention();
            options.UseSqlServer(configuration.GetConnectionString("Database"), o =>
            {
                o.MigrationsAssembly("AgriPen");
            });

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        return services;
    }
}
