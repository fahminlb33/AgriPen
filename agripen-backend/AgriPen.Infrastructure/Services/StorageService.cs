using AgriPen.Domain.Enums;
using AgriPen.Infrastructure.Helpers;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace AgriPen.Infrastructure.Services;

public interface IStorageService
{
    Task DeleteDiseasePrediction(Ulid id, CancellationToken ct = default);
    Task DeleteLandPhoto(Ulid id, CancellationToken ct = default);
    Task<List<string>> ListDiseasePrediction(Ulid id, CancellationToken ct = default);
    Task<List<string>> ListLandPhoto(Ulid id, CancellationToken ct = default);
    Task UploadDiseasePrediction(Ulid id, DiseasePredictionResult category, Stream stream, CancellationToken ct = default);
    Task UploadLandPhoto(Ulid id, Stream stream, CancellationToken ct = default);
}

public class StorageService : IStorageService
{
    private const string DiseasePredictionContainer = "disease-prediction";
    private const string LandObservationContainer = "land-observation";

    private readonly BlobServiceClient _client;

    public StorageService(string connectionString)
    {
        _client = new(connectionString);
    }

    private async Task InternalCompress(Stream input, Stream output, CancellationToken ct)
    {
        // load image
        var image = await Image.LoadAsync(input, ct);

        // resize width to maximum 1000 px
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size() { Width = 1000 }
        }));

        // save
        var encoder = new JpegEncoder { Quality = 80 };
        image.Save(output, encoder);
    }

    public async Task UploadDiseasePrediction(Ulid id, DiseasePredictionResult category, Stream stream, CancellationToken ct = default)
    {
        // create blob name
        var blobName = $"predictions/{id}/{category.ToString().ToLower()}.jpg";
        var container = _client.GetBlobContainerClient(DiseasePredictionContainer);

        // create blob
        var blob = container.GetBlobClient(blobName);
        var opts = new BlobUploadOptions
        {
            Tags = new Dictionary<string, string>() { { "InferenceID", id.ToString() } }
        };

        // compress
        using var result = new MemoryStream();
        await InternalCompress(stream, result, ct);

        // upload blob
        result.Seek(0, SeekOrigin.Begin);
        await blob.UploadAsync(result, opts, ct);
    }

    public async Task UploadLandPhoto(Ulid id, Stream stream, CancellationToken ct = default)
    {
        // create blob name
        var blobName = $"{id}/{PasswordHelper.GeneratePseudoRandomString(5)}.jpg";
        var container = _client.GetBlobContainerClient(LandObservationContainer);

        // create blob
        var blob = container.GetBlobClient(blobName);
        var opts = new BlobUploadOptions
        {
            Tags = new Dictionary<string, string>() { { "ObservationID", id.ToString() } }
        };

        // compress
        using var result = new MemoryStream();
        await InternalCompress(stream, result, ct);

        // upload blob
        result.Seek(0, SeekOrigin.Begin);
        await blob.UploadAsync(result, opts, ct);
    }

    public async Task<List<string>> ListDiseasePrediction(Ulid id, CancellationToken ct = default)
    {
        // create search query
        var sql = $"\"InferenceID\" = '{id}'";
        var container = _client.GetBlobContainerClient(DiseasePredictionContainer);

        // fetch all blob
        var blobs = await InternalListBlobs(container, sql, ct);
        return blobs.Select(x => $"{container.Uri}/{x.BlobName}").ToList();
    }

    public async Task<List<string>> ListLandPhoto(Ulid id, CancellationToken ct = default)
    {
        // create search query
        var sql = $"\"ObservationID\" = '{id}'";
        var container = _client.GetBlobContainerClient(LandObservationContainer);

        // fetch all blob
        var blobs = await InternalListBlobs(container, sql, ct);
        return blobs.Select(x => $"{container.Uri}/{x.BlobName}").ToList();
    }

    public async Task DeleteDiseasePrediction(Ulid id, CancellationToken ct = default)
    {
        // create search query
        var sql = $"\"InferenceID\" = '{id}'";
        var container = _client.GetBlobContainerClient(LandObservationContainer);

        // delete all blob
        var blobs = await InternalListBlobs(container, sql, ct);
        foreach (var blob in blobs)
        {
            await container.DeleteBlobAsync(blob.BlobName, cancellationToken: ct);
        }
    }

    public async Task DeleteLandPhoto(Ulid id, CancellationToken ct = default)
    {
        // create search query
        var sql = $"\"ObservationID\" = '{id}'";
        var container = _client.GetBlobContainerClient(LandObservationContainer);

        // delete all blob
        var blobs = await InternalListBlobs(container, sql, ct);
        foreach (var blob in blobs)
        {
            await container.DeleteBlobAsync(blob.BlobName, cancellationToken: ct);
        }
    }

    private async Task<List<TaggedBlobItem>> InternalListBlobs(BlobContainerClient container, string query, CancellationToken ct)
    {
        // fetch all blobs
        var list = new List<TaggedBlobItem>();
        await foreach (var blob in container.FindBlobsByTagsAsync(query, cancellationToken: ct))
        {
            list.Add(blob);
        }

        return list;
    }
}
