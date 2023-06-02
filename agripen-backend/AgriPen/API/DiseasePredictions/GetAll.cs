using AgriPen.Domain.Entities;
using AgriPen.Infrastructure;
using AgriPen.Infrastructure.Extensions;
using AgriPen.Infrastructure.Helpers;
using AgriPen.SharedKernel.Enums;
using AgriPen.SharedKernel.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.DiseasePredictions;

public class ListItem
{
    public Ulid Id { get; set; }
    public string Status { get; set; }
    public string Result { get; set; }
    public double Severity { get; set; }
    public double Probability { get; set; }
    public string Address { get; set; }
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
        Get("/disease-predictions");
        Roles();
    }

    private static double GetMaximumProbability(DiseaseProbability p)
    {
        var proba = new double[] { p.BacterialBlight, p.Blast, p.BrownSpot, p.Healthy, p.Tungro };
        return proba.Max();
    }

    public override async Task HandleAsync(DefaultPaginationRequest req, CancellationToken ct)
    {
        // find query
        var query = _context.DiseasePredictions
            .AsNoTracking()
            .AsQueryable()
            .Include(x => x.GpsAddress)
            .Include(x => x.Probability)
            .Select(x => new ListItem
            {
                Id = x.Id,

                Status = x.Status.ToString(),
                Result = x.Result.ToString(),
                Severity = x.Severity,
                Probability = GetMaximumProbability(x.Probability),
                Address = x.GpsAddress.GeocodedAddress,

                CreatedAt = x.CreatedAt,
            });

        // filter by keyword
        if (!string.IsNullOrWhiteSpace(req.Keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Address, $"%{req.Keyword}%"));
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
                "status" => query.ApplySort(x => x.Status, req.OrderDirection),
                "result" => query.ApplySort(x => x.Result, req.OrderDirection),
                "severity" => query.ApplySort(x => x.Severity, req.OrderDirection),
                "probability" => query.ApplySort(x => x.Probability, req.OrderDirection),
                "address" => query.ApplySort(x => x.Address, req.OrderDirection),
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
