using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure.Helpers;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Users;

public class ChangePasswordRequest
{
    public Ulid Id { get; set; }
    public string NewPassword { get; set; }
}

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(5);
    }
}

public class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest, EmptyResponse>
{
    private readonly AgriDataContext _context;

    public ChangePasswordEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Put("/users/{UserId}/password");
        Roles(UserExtensions.Admin);
    }

    public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
    {
        // find user
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        // user is exists
        if (user == null)
        {
            AddError("User not found.");
            await SendErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        // change password
        user.HashedPassword = PasswordHelper.Hash(req.NewPassword);

        // save
        await _context.SaveChangesAsync(ct);

        // send data
        await SendNoContentAsync(ct);
    }
}
