using AgriPen.Domain.Enums;
using AgriPen.EventHandlers;
using AgriPen.Helpers;
using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure.Services;
using FastEndpoints;
using FluentValidation;
using static System.Collections.Specialized.BitVector32;

namespace AgriPen.API.DiseasePredictions;

public class UploadRequest
{
    public GpsAddressDto GpsAddress { get; set; }
}

public class UploadResponse
{
    public Ulid Id { get; set; }
}

public class UploadRequestValidator : Validator<UploadRequest>
{
    public UploadRequestValidator()
    {
        RuleFor(x => x.GpsAddress).NotNull();
    }
}

public class UploadEndpoint : Endpoint<UploadRequest, UploadResponse>
{
    private readonly AgriDataContext _context;
    private readonly IMapService _map;
    private readonly IStorageService _storage;

    public UploadEndpoint(AgriDataContext context, IMapService map, IStorageService storage)
    {
        _context = context;
        _map = map;
        _storage = storage;
    }

    public override void Configure()
    {
        Post("/disease-predictions");
        Roles();
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadRequest req, CancellationToken ct)
    {
        // check file existance
        if (Files.Count == 0)
        {
            AddError("At least one file must be uploaded");
            await SendErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        // create new ID
        var id = Ulid.NewUlid();
        var now = DateTimeOffset.Now;

        // run reverse geocoding
        var geocodeBingAddress = await _map.ReverseGeocodeBingAsync(req.GpsAddress.Latitude, req.GpsAddress.Longitude);
        var geocodeBmkgAddress = await _context.ReverseGeocodeAsync(req.GpsAddress.Latitude, req.GpsAddress.Longitude, ct);

        // upload to blob storage
        await _storage.UploadDiseasePrediction(id, DiseasePredictionResult.Unknown, Files[0].OpenReadStream(), ct);

        // save to DB
        await _context.DiseasePredictions.AddAsync(new()
        {
            Id = id,

            Severity = 0.0,
            Result = DiseasePredictionResult.Unknown,
            Status = DiseasePredictionStatus.Queued,

            Probability = new()
            {
                Id = Ulid.NewUlid(),
            },
            LocalAddress = geocodeBmkgAddress,
            GpsAddress = new()
            {
                Id = Ulid.NewUlid(),
                GeocodedAddress = geocodeBingAddress,
                Latitude = req.GpsAddress.Latitude,
                Longitude = req.GpsAddress.Longitude,
                Altitude = req.GpsAddress.Altitude,
                HorizontalAccuracy = req.GpsAddress.HorizontalAccuracy,
                VerticalAccuracy = req.GpsAddress.VerticalAccuracy,
            },

            CreatedAt = now,
            UpdatedAt = now,
        }, ct);

        await _context.SaveChangesAsync(ct);

        // publish predict event
        await PublishAsync(new PredictionCreatedEvent { Id = id }, Mode.WaitForNone, ct);

        // return
        await SendOkAsync(new() { Id = id }, ct);
    }
}

