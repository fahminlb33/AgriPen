using AgriPen.Helpers;
using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Services;
using AgriPen.Infrastructure.Extensions;
using FastEndpoints;
using FluentValidation;
using AgriPen.Domain.Entities;

namespace AgriPen.API.LandObservations;

public class TelemetryRequest
{
    public GpsAddressDto GpsAddress { get; set; }
    public List<TelemetryDto> TimeSeries { get; set; }
}

public class TelemetryResponse
{
    public Ulid Id { get; set; }
}

public class TelemetryEndpointValidator : Validator<TelemetryRequest>
{
    public TelemetryEndpointValidator()
    {
        RuleFor(x => x.GpsAddress)
            .NotEmpty()
            .ChildRules(x =>
            {
                x.RuleFor(p => p.Latitude).GreaterThanOrEqualTo(-90).LessThanOrEqualTo(90);
                x.RuleFor(p => p.Longitude).GreaterThanOrEqualTo(-180).LessThanOrEqualTo(180);
                x.RuleFor(p => p.Altitude).GreaterThanOrEqualTo(-1);
                x.RuleFor(p => p.HorizontalAccuracy).GreaterThanOrEqualTo(-1);
                x.RuleFor(p => p.VerticalAccuracy).GreaterThanOrEqualTo(-1);
            });
    }
}

public class TelemetryEndpoint : Endpoint<TelemetryRequest, TelemetryResponse>
{
    private readonly AgriDataContext _context;
    private readonly IMapService _map;

    public TelemetryEndpoint(AgriDataContext context, IMapService map)
    {
        _context = context;
        _map = map;
    }

    public override void Configure()
    {
        Post("/land-observations");
        Roles();
    }

    public override async Task HandleAsync(TelemetryRequest req, CancellationToken ct)
    {
        // create new ID
        var id = Ulid.NewUlid();
        var now = DateTimeOffset.Now;

        // run reverse geocoding
        var geocodeBingAddress = await _map.ReverseGeocodeBingAsync(req.GpsAddress.Latitude, req.GpsAddress.Longitude);
        var geocodeBmkgAddress = await _context.ReverseGeocodeAsync(req.GpsAddress.Latitude, req.GpsAddress.Longitude, ct);

        // save to DB
        await _context.LandObservations.AddAsync(new()
        {
            Id = id,

            LocalAddress = geocodeBmkgAddress,
            GpsAddress = new()
            {
                Id = Ulid.NewUlid(),
                GeocodedAddress = geocodeBingAddress,
                Latitude = req.GpsAddress.Latitude,
                Longitude = req.GpsAddress.Longitude,
                HorizontalAccuracy = req.GpsAddress.HorizontalAccuracy,
                VerticalAccuracy = req.GpsAddress.VerticalAccuracy,
            },
            Telemetries = req.TimeSeries.Select(x => new Telemetry()
            {
                Id = Ulid.NewUlid(),
                Timestamp = x.Timestamp,
                AirTemperature = x.AirTemperature,
                AirHumidity = x.AirHumidity,
                SoilMoisture = x.SoilMoisture,
                SoilTemperature = x.SoilTemperature,
                SunIllumination = x.SunIllumination,
            }).ToList(),

            CreatedAt = now,
            UpdatedAt = now,
        }, ct);

        await _context.SaveChangesAsync(ct);

        // return
        await SendOkAsync(new() { Id = id }, ct);
    }
}
