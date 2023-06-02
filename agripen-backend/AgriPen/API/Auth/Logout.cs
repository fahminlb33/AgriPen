using FastEndpoints;
using StackExchange.Redis;
using System.Security.Claims;

namespace AgriPen.API.Auth;

public class LogoutRequest
{
    [FromClaim(ClaimTypes.NameIdentifier)]
    public Ulid UserId { get; set; }
}

public class LogoutEndpoint : Endpoint<LogoutRequest>
{
    private readonly IConnectionMultiplexer _redisCn;

    public LogoutEndpoint(IConnectionMultiplexer redisCn)
    {
        _redisCn = redisCn;
    }

    public override void Configure()
    {
        Post("/auth/logout");
        Roles();
    }

    public override async Task HandleAsync(LogoutRequest req, CancellationToken ct)
    {
        // remove refresh token
        await _redisCn.GetDatabase(0).KeyDeleteAsync($"agripen:refresh_token:{req.UserId}");

        await SendNoContentAsync(ct);
    }
}
