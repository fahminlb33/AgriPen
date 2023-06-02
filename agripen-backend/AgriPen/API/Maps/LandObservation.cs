using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Maps;

public class LandObservationItem
{
    public Ulid Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Address { get; set; }

    public double AirTemperature { get; set; }
    public double AirHumidity { get; set; }
    public double SoilMoisture { get; set; }
    public double SoilTemperature { get; set; }
    public double SunIllumination { get; set; }
}

public class LandObservationEndpoint : EndpointWithoutRequest
{
    private readonly AgriDataContext _context;

    public LandObservationEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/maps/land-observation");
        Roles();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var points = await _context.LandObservations
            .AsNoTracking()
            .Include(x => x.GpsAddress)
            .Include(x => x.Telemetries)
            .Select(x => new LandObservationItem
            {
                Id = x.Id,
                Timestamp = x.CreatedAt,
                Latitude = x.GpsAddress.Latitude,
                Longitude = x.GpsAddress.Longitude,
                Address = x.GpsAddress.GeocodedAddress,

                AirTemperature = x.Telemetries.Average(x => x.AirTemperature),
                AirHumidity = x.Telemetries.Average(x => x.AirHumidity),
                SoilMoisture = x.Telemetries.Average(x => x.SoilMoisture),
                SoilTemperature = x.Telemetries.Average(x => x.SoilTemperature),
                SunIllumination = x.Telemetries.Average(x => x.SunIllumination),
            })
            .ToListAsync(ct);

        await SendOkAsync(points, ct);
    }
}
