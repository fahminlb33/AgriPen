using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using AgriPen.Infrastructure.Extensions;

namespace AgriPen.API.DiseasePredictions;

public class DeleteRequest 
{
    public Ulid DiseasePredictionId { get; set; }
}


public class DeleteEndpoint : Endpoint<DeleteRequest, EmptyResponse>
{
    private readonly AgriDataContext _context;

    public DeleteEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete("/disease-predictions/{DiseasePredictionId}");
        Roles(UserExtensions.Admin);
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        // find
        var data = await _context.DiseasePredictions.FirstOrDefaultAsync(x => x.Id == req.DiseasePredictionId, ct);

        // if not found,
        if (data == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // delete
        _context.DiseasePredictions.Remove(data);
        await _context.SaveChangesAsync(ct);

        // send data
        await SendNoContentAsync(ct);
    }
}