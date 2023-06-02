using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using AgriPen.Domain.Entities;
using AgriPen.Domain.Enums;
using AgriPen.Infrastructure.Services;
using AgriPen.Helpers;

namespace AgriPen.API.DiseasePredictions;

public class GetRequest
{
    public Ulid DiseasePredictionId { get; set; }
}

public class GetResponse
{
    public Ulid Id { get; set; }

    public double Severity { get; set; }
    public DiseasePredictionResult Result { get; set; }
    public DiseasePredictionStatus Status { get; set; }
    public DiseaseProbabilityDto Probabilities { get; set; }

    public List<string> Images { get; set; }
    public GpsAddressDto GpsAddress { get; set; }
    public LocalAddressDto LocalAddress { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetEndpointMapper : Mapper<GetRequest, GetResponse, DiseasePrediction>
{
    public override GetResponse FromEntity(DiseasePrediction e)
    {
        return new()
        {
            Id = e.Id,

            Severity = e.Severity,
            Result = e.Result,
            Status = e.Status,
            Probabilities = new DiseaseProbabilityDto
            {
                BacterialBlight = e.Probability.BacterialBlight,
                Blast = e.Probability.Blast,
                BrownSpot = e.Probability.BrownSpot,
                Tungro = e.Probability.Tungro,
                Healthy = e.Probability.Healthy
            },

            GpsAddress = e.GpsAddress.ToDto(),
            LocalAddress = e.LocalAddress.ToDto(),

            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };
    }
}

public class GetEndpoint : Endpoint<GetRequest, GetResponse, GetEndpointMapper>
{
    private readonly AgriDataContext _context;
    private readonly IStorageService _storage;

    public GetEndpoint(AgriDataContext context, IStorageService storage)
    {
        _context = context;
        _storage = storage;
    }

    public override void Configure()
    {
        Get("/disease-predictions/{DiseasePredictionId}");
        Roles();
    }

    public override async Task HandleAsync(GetRequest req, CancellationToken ct)
    {
        // find
        var pred = await _context.DiseasePredictions
            .AsNoTracking()
            .Include(x => x.Probability)
            .Include(x => x.GpsAddress)
            .Include(x => x.LocalAddress)
            .FirstOrDefaultAsync(x => x.Id == req.DiseasePredictionId, ct);

        // if not found,
        if (pred == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // map to DTO
        var dto = Map.FromEntity(pred);

        // get images
        dto.Images = await _storage.ListDiseasePrediction(pred.Id, ct);

        // send data
        await SendOkAsync(dto, ct);
    }
}
