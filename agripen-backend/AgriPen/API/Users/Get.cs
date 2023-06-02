using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Users;

public class GetRequest
{
    public Ulid UserId { get; set; }
}

public class GetEndpoint : Endpoint<GetRequest, ProfileResponse, ProfileMapper>
{
    private readonly AgriDataContext _context;

    public GetEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/users/{UserId}");
        Roles(UserExtensions.Admin);
    }

    public override async Task HandleAsync(GetRequest req, CancellationToken ct)
    {
        // find user
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == req.UserId, ct);

        // if user is not found,
        if (user == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // send data
        await SendOkAsync(Map.FromEntity(user), ct);
    }
}
