using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.LandObservations;

public class DeleteRequest
{
    public Ulid LandObservationId { get; set; }
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
        Delete("/land-observations/{LandObservationId}");
        Roles(UserExtensions.Admin);
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        // find
        var data = await _context.LandObservations.FirstOrDefaultAsync(x => x.Id == req.LandObservationId, ct);

        // if not found,
        if (data == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // delete
        _context.LandObservations.Remove(data);
        await _context.SaveChangesAsync(ct);

        // send data
        await SendNoContentAsync(ct);
    }
}