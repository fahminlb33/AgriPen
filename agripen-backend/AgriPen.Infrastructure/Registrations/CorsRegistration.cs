using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AgriPen.Infrastructure.Registrations;


public static class CorsRegistration
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddCors(options =>
        {
            if (environment.IsDevelopment())
            {
                // running in production but for local dev
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.AllowCredentials();
                    policy.WithOrigins();
                });
            }
            else
            {
                // running in production
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.AllowCredentials();
                    policy.WithOrigins("https://agripen.kodesiana.com", "http://localhost:4000");
                });
            }
        });

        return services;
    }

    public static WebApplication UseCustomCors(this WebApplication app)
    {
        app.UseCors();

        return app;
    }
}
