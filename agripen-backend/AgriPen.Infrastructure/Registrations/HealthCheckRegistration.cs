using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AgriPen.Infrastructure.Registrations;

public static class HealthCheckRegistration
{
    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<AgriDataContext>("Main Database");

        return services;
    }

    public static WebApplication MapCustomHealthCheck(this WebApplication app)
    {
        app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = ResponseWriter
        });

        return app;
    }

    // https://digitaldrummerj.me/aspnet-core-health-checks-json/
    public static async Task ResponseWriter(HttpContext context, HealthReport report)
    {
        var body = new
        {
            Status = report.Status.ToString(),
            Duration = report.TotalDuration,
            Info = report.Entries
                .Select(e =>
                    new
                    {
                        e.Key,
                        e.Value.Description,
                        e.Value.Duration,
                        Status = e.Value.Status.ToString(),
                    })
                .ToList()
        };

        await context.Response.WriteAsJsonAsync(body, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}
