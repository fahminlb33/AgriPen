using AgriPen.Domain.Entities;
using AgriPen.Domain.Enums;
using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriPen.API.Users;

public class ProfileRequest
{
    [FromClaim(ClaimTypes.NameIdentifier)]
    public Ulid UserId { get; set; }
}

public class ProfileResponse
{
    public Ulid Id { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ProfileMapper: ResponseMapper<ProfileResponse, User>
{
    public override ProfileResponse FromEntity(User e)
    {
        return new()
        {
            Id = e.Id,

            Username = e.Username,
            Role = e.Role,
            LastLoginAt = e.LastLoginAt,

            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
        };
    }
}

public class ProfileEndpoint : Endpoint<ProfileRequest, ProfileResponse, ProfileMapper>
{
    private readonly AgriDataContext _context;

    public ProfileEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/users/profile");
        Roles();
    }

    public override async Task HandleAsync(ProfileRequest req, CancellationToken ct)
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
