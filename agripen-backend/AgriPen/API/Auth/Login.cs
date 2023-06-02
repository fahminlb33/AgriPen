using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure.Helpers;
using AgriPen.Services;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Auth;

public class LoginRequest
{
    public string User { get; set; }
    public string Password { get; set; }
}

public class LoginEndpoint : Endpoint<LoginRequest, TokenResponse>
{
    private readonly AgriDataContext _context;

    public LoginEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // find user, if not found return unauthorized
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == req.User || x.Email == req.User, ct);
        if (user == null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        // verify password, if not match return unauthorized
        if (!PasswordHelper.Verify(req.Password, user.HashedPassword))
        {
            await SendUnauthorizedAsync(ct);
        }

        // set last login
        user.LastLoginAt = DateTimeOffset.Now;
        await _context.SaveChangesAsync(ct);

        // generate access token and refresh token
        Response = await CreateTokenWith<RefreshTokenService>(user.Id.ToString(), u =>
        {
            u.PopulateFromUser(user);
        });
    }
}
