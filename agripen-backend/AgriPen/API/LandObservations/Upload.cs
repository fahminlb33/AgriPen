using AgriPen.Domain.Enums;
using AgriPen.Infrastructure.Services;
using FastEndpoints;
using FluentValidation;

namespace AgriPen.API.LandObservations;

public class UploadRequest
{
    public Ulid LandObservationId { get; set; }
}

public class UploadEndpoint : Endpoint<UploadRequest, EmptyResponse>
{
    private readonly IStorageService _storage;

    public UploadEndpoint(IStorageService storage)
    {
        _storage = storage;
    }

    public override void Configure()
    {
        Put("/land-observations/{LandObservationId}/images");
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

        // check the number of images already uploaded
        var alreadyUploaded = await _storage.ListLandPhoto(req.LandObservationId, ct);
        if (alreadyUploaded.Count == 5)
        {
            AddError("The observation has a maximum of 5 images.");
            await SendErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        // get maximum image to upload
        var remainingImages = 5 - alreadyUploaded.Count;

        // upload to blob storage
        foreach (var image in Files.Take(remainingImages))
        {
            // upload
            await _storage.UploadLandPhoto(req.LandObservationId, image.OpenReadStream(), ct);
        }

        await SendNoContentAsync(ct);
    }
}