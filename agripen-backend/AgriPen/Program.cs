using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure.Registrations;
using AgriPen.Infrastructure.Services;
using Cysharp.Serialization.Json;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using StackExchange.Redis;
using System.Text.Json.Serialization;

// ---------------------- REGISTER SERVICES ---------------------- 

// setup logging
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

// create web builder
var builder = WebApplication.CreateBuilder(args);

// add configuration source
builder.Configuration.AddEnvironmentVariables("AGRIPEN_");
builder.Services.Configure<AgriConfiguration>(builder.Configuration);

var config = builder.Configuration.Get<AgriConfiguration>()!;

// add serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    var telemetryService = services.GetRequiredService<TelemetryConfiguration>();
    loggerConfig
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Async(w => w.Console())
        .WriteTo.ApplicationInsights(telemetryService, TelemetryConverter.Traces);
});

// add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry();

// add FastEndpoints
builder.Services.AddFastEndpoints();

// add Swagger
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "AgriPen API";
        s.Version = "v1.0.0";
    };
});

// add Authentication
builder.Services.AddJWTBearerAuth(config.JwtAuth.Key);

// add CORS
builder.Services.AddCustomCors(builder.Environment);

// add health check
builder.Services.AddCustomHealthCheck();

// add database
builder.Services.AddCustomDatabaseContext(builder.Configuration, builder.Environment);

// add services
builder.Services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(o =>
{
    var cn = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(cn);
});
builder.Services.AddSingleton<IStorageService, StorageService>(o =>
{
    var cn = builder.Configuration.GetConnectionString("Storage");
    return new StorageService(cn);
});
builder.Services.AddSingleton<IMapService, MapService>(o =>
{
    var key = builder.Configuration.GetConnectionString("BingMaps");
    return new MapService(key);
});

// increase upload max limit
builder.WebHost.ConfigureKestrel(o =>
{
    o.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
});

// ---------------------- BUILD APP ---------------------- 

// build app
var app = builder.Build();

// cors
app.UseCustomCors();

// auth
app.UseAuthentication();
app.UseAuthorization();

// map health check
app.MapCustomHealthCheck();

// map app routes
app.UseFastEndpoints(o =>
{
    o.Serializer.Options.Converters.Add(new UlidJsonConverter());
    o.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
});

// map Swagger
app.UseSwaggerGen();

// ---------------------- BOOTSTRAP APP ---------------------- 

try
{
    Log.Information("Starting web host");
    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return -1;
}
finally
{
    Log.CloseAndFlush();
}
