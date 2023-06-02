using AgriPen.Domain.Entities;
using AgriPen.Domain.Enums;
using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Maps;

public class DiseasePredictionItem
{
    public Ulid Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Address { get; set; }

    public double Severity { get; set; }
    public double Probability { get; set; }
    public DiseasePredictionResult Result { get; set; }
    public DiseasePredictionStatus Status { get; set; }
}

public class DiseasePredictionEndpoint : EndpointWithoutRequest<List<DiseasePredictionItem>>
{
    private readonly AgriDataContext _context;

    public DiseasePredictionEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/maps/disease");
        Roles();
    }

    private static double GetMaximumProbability(DiseaseProbability p)
    {
        var proba = new double[] { p.BacterialBlight, p.Blast, p.BrownSpot, p.Healthy, p.Tungro };
        return proba.Max();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var points = await _context.DiseasePredictions
            .AsNoTracking()
            .Include(x => x.GpsAddress)
            .Include(x => x.Probability)
            .Select(x => new DiseasePredictionItem
            {
                Id = x.Id,
                Timestamp = x.CreatedAt,
                Latitude = x.GpsAddress.Latitude,
                Longitude = x.GpsAddress.Longitude,
                Address = x.GpsAddress.GeocodedAddress,

                Severity = x.Severity,
                Probability = GetMaximumProbability(x.Probability),
                Result = x.Result,
                Status = x.Status,
            })
            .ToListAsync(ct);

        await SendOkAsync(points, ct);
    }
}
