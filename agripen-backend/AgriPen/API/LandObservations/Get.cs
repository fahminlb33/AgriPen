using AgriPen.Domain.Entities;
using AgriPen.Helpers;
using AgriPen.Infrastructure.Services;
using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.LandObservations;

public class GetRequest
{
    public Ulid LandObservationId { get; set; }
}

public class GetResponse
{
    public Ulid Id { get; set; }
    public List<string> Images { get; set; }
    public List<TelemetryDto> Telemetry { get; set; }
    public GpsAddressDto GpsAddress { get; set; }
    public LocalAddressDto LocalAddress { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetEndpointMapper : Mapper<GetRequest, GetResponse, LandObservation>
{
    public override GetResponse FromEntity(LandObservation e)
    {
        return new()
        {
            Id = e.Id,

            Telemetry = e.Telemetries.Select(x => x.ToDto()).ToList(),
            GpsAddress = e.GpsAddress.ToDto(),
            LocalAddress = e.LocalAddress.ToDto(),

            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
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
        Get("/land-observations/{LandObservationId}");
        Roles();
    }

    public override async Task HandleAsync(GetRequest req, CancellationToken ct)
    {
        // find
        var pred = await _context.LandObservations
            .AsNoTracking()
            .Include(x => x.Telemetries)
            .Include(x => x.GpsAddress)
            .Include(x => x.LocalAddress)
            .FirstOrDefaultAsync(x => x.Id == req.LandObservationId, ct);

        // if not found,
        if (pred == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // map to DTO
        var dto = Map.FromEntity(pred);

        // get images
        dto.Images = await _storage.ListLandPhoto(pred.Id, ct);

        // send data
        await SendOkAsync(dto, ct);
    }
}
