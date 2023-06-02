using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Users;

public class DeleteRequest
{
    public Ulid UserId { get; set; }
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
        Delete("/users/{UserId}");
        Roles(UserExtensions.Admin);
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        // find user
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == req.UserId, ct);

        // if user is not found,
        if (user == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // delete user
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(ct);

        // send data
        await SendNoContentAsync(ct);
    }
}
