using FastEndpoints.Security;
using FastEndpoints;
using AgriPen.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using AgriPen.Infrastructure.Extensions;
using StackExchange.Redis;

namespace AgriPen.Services;

public class RefreshTokenService : RefreshTokenService<TokenRequest, TokenResponse>
{
    private readonly AgriDataContext _context;
    private readonly IConnectionMultiplexer _redisCn;

    public RefreshTokenService(IConfiguration configuration, AgriDataContext context, IConnectionMultiplexer redisCn)
    {
        _context = context;
        _redisCn = redisCn;
        
        Setup(c =>
        {
            var config = configuration.Get<AgriConfiguration>()!;
            c.TokenSigningKey = config.JwtAuth.Key;
            c.AccessTokenValidity = TimeSpan.FromHours(1);
            c.RefreshTokenValidity = TimeSpan.FromDays(7);

            c.Endpoint("/auth/refresh-token", ep =>
            {
                ep.Summary(s => s.Summary = "Generates new access token using refresh token");
            });
        });
    }

    public override async Task PersistTokenAsync(TokenResponse response)
    {
        await _redisCn
            .GetDatabase(0)
            .StringSetAsync($"agripen:refresh_token:{response.UserId}", response.RefreshToken, TimeSpan.FromDays(7));
    }

    public override async Task RefreshRequestValidationAsync(TokenRequest req)
    {
        var refreshToken = await _redisCn
            .GetDatabase(0)
            .StringGetAsync($"agripen:refresh_token:{req.UserId}");

        if (refreshToken.IsNullOrEmpty)
        {
            AddError("Refresh token is expired or revoked");
        }
    }

    public override async Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
    {
        // find user
        var user = await _context.Users.SingleAsync(x => x.Id == Ulid.Parse(request.UserId));

        // set privileges
        privileges.PopulateFromUser(user);
    }
}
