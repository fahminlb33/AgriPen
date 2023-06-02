using AgriPen.Domain.Enums;
using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure.Helpers;
using AgriPen.SharedKernel.Enums;
using AgriPen.SharedKernel.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Users;

public class ListItem
{
    public Ulid Id { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class GetAllEndpoint : Endpoint<DefaultPaginationRequest, PaginationResponse<ListItem>>
{
    private readonly AgriDataContext _context;

    public GetAllEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/users");
        Roles(UserExtensions.Admin);
    }

    public override async Task HandleAsync(DefaultPaginationRequest req, CancellationToken ct)
    {
        // find query
        var query = _context.Users
            .AsNoTracking()
            .AsQueryable()
            .Select(x => new ListItem
            {
                Id = x.Id,
                Username = x.Username,
                Role = x.Role,
                LastLoginAt = x.LastLoginAt,
                CreatedAt = x.CreatedAt,
            });

        // filter by keyword
        if (!string.IsNullOrWhiteSpace(req.Keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Username, $"%{req.Keyword}%"));
        }

        // get total
        var totalRows = await query.CountAsync(ct);

        // apply sorting
        if (req.OrderDirection == SortDirection.Default)
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }
        else
        {
            query = req.OrderBy.ToLower() switch
            {
                "id" => query.ApplySort(x => x.Id, req.OrderDirection),
                "username" => query.ApplySort(x => x.Username, req.OrderDirection),
                "role" => query.ApplySort(x => x.Role, req.OrderDirection),
                "lastloginat" => query.ApplySort(x => x.LastLoginAt, req.OrderDirection),
                "createdat" => query.ApplySort(x => x.CreatedAt, req.OrderDirection),
                _ => query.OrderByDescending(x => x.CreatedAt),
            };
        }

        // set pagination
        var rows = await query
            .Skip(PaginationHelper.GetOffset(req.Page, req.Limit))
            .Take(req.Limit)
            .ToListAsync(ct);

        // send response
        await SendOkAsync(new PaginationResponse<ListItem>(rows, totalRows, req.Page, req.Limit), ct);
    }
}
