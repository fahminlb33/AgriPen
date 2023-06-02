using AgriPen.Domain.Enums;
using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Maps;

public class WeatherItem
{
    public Ulid Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Kecamatan { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public double TemperatureLow { get; set; }
    public double TemperatureHigh { get; set; }
    public double HumidityLow { get; set; }
    public double HumidityHigh { get; set; }
    public double Humidity { get; set; }
    public WeatherCode Weather { get; set; }
    public WindCode Wind { get; set; }
    public double WindSpeed { get; set; }
}

public class WeatherPredictionEndpoint : EndpointWithoutRequest<List<WeatherItem>>
{
    private readonly AgriDataContext _context;

    public WeatherPredictionEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/maps/weather");
        Roles();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var points = await _context.WeatherPredictions
            .AsNoTracking()
            .Include(x => x.LocalAddress)
            .Where(x => EF.Functions.Like(x.LocalAddress.Kabupaten, "%bogor"))
            .Select(x => new WeatherItem
            {
                Id = x.Id,
                Timestamp = x.Timestamp,
                Latitude = x.LocalAddress.Latitude,
                Longitude = x.LocalAddress.Longitude,
                Kecamatan = x.LocalAddress.Kecamatan,

                TemperatureLow = x.TemperatureLow,
                TemperatureHigh = x.TemperatureHigh,
                Humidity = x.Humidity,
                HumidityLow = x.HumidityLow,
                HumidityHigh = x.HumidityHigh,
                Weather = x.Weather,
                Wind = x.Wind,
                WindSpeed = x.WindSpeed,
            })
            .ToListAsync(ct);

        var pointsUnique = points
            .GroupBy(x => x.Kecamatan)
            .Select(x => new WeatherItem
            {
                Id = x.Last().Id,
                Timestamp = x.Last().Timestamp,
                Latitude = x.Last().Latitude,
                Longitude = x.Last().Longitude,
                Kecamatan = x.Last().Kecamatan,

                TemperatureLow = x.Last().TemperatureLow,
                TemperatureHigh = x.Last().TemperatureHigh,
                Humidity = x.Last().Humidity,
                HumidityLow = x.Last().HumidityLow,
                HumidityHigh = x.Last().HumidityHigh,
                Weather = x.Last().Weather,
                Wind = x.Last().Wind,
                WindSpeed = x.Last().WindSpeed,
            })
            .ToList();

        await SendOkAsync(pointsUnique, ct);
    }
}
