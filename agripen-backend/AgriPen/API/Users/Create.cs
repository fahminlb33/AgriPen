using AgriPen.Domain.Entities;
using AgriPen.Domain.Enums;
using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure.Helpers;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriPen.API.Users;

public class CreateRequest
{
    [FromClaim(ClaimTypes.NameIdentifier)]
    public Ulid CreatorId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}

public class CreateValidator : Validator<CreateRequest>
{
    public CreateValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(5);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(5);
        RuleFor(x => x.Role)
            .NotEmpty()
            .IsInEnum();
    }
}

public class CreateMapper : Mapper<CreateRequest, ProfileResponse, User>
{
    public override User ToEntity(CreateRequest r)
    {
        return new()
        {
            Id = Ulid.NewUlid(),

            Username = r.Username,
            Email = r.Email.ToLower(),
            HashedPassword = PasswordHelper.Hash(r.Password),
            Role = r.Role,

            LastLoginAt = null,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now,
        };
    }

    public override ProfileResponse FromEntity(User e)
    {
        return new()
        {
            Id = e.Id,
            Username = e.Username,
            Role = e.Role,

            LastLoginAt = e.LastLoginAt,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };
    }
}

public class CreateEndpoint : Endpoint<CreateRequest, ProfileResponse, CreateMapper>
{
    private readonly AgriDataContext _context;

    public CreateEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post("/users");
        Roles(UserExtensions.Admin);
    }

    public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
    {
        // find user
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == req.Username || x.Email == req.Email.ToLower(), ct);

        // user is exists
        if (user != null)
        {
            AddError("Username atau email sudah terdaftar.");
            await SendErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        // map data
        user = Map.ToEntity(req);

        // save
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);

        // send data
        await SendOkAsync(Map.FromEntity(user), ct);
    }
}