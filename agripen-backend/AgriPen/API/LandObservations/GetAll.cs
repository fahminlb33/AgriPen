using AgriPen.Infrastructure.Helpers;
using AgriPen.Infrastructure;
using AgriPen.SharedKernel.Enums;
using AgriPen.SharedKernel.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using AgriPen.Infrastructure.Extensions;

namespace AgriPen.API.LandObservations;


public class ListItem
{
    public Ulid Id { get; set; }
    public double AirTemperature { get; set; }
    public double AirHumidity { get; set; }
    public double AirHeatIndex { get; set; }
    public double SoilMoisture { get; set; }
    public double SunIllumination { get; set; }
    public string GpsLocation { get; set; }
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
        Get("/land-observations");
        Roles();
    }

    public override async Task HandleAsync(DefaultPaginationRequest req, CancellationToken ct)
    {
        // find query
        var query = _context.LandObservations
            .AsNoTracking()
            .AsQueryable()
            .Include(x => x.Telemetries)
            .Include(x => x.GpsAddress)
            .Select(x => new ListItem
            {
                Id = x.Id,

                AirTemperature = x.Telemetries.Average(x => x.AirTemperature),
                AirHumidity = x.Telemetries.Average(x => x.AirHumidity),
                AirHeatIndex = x.Telemetries.Average(x => x.AirHeatIndex),
                SoilMoisture = x.Telemetries.Average(x => x.SoilMoisture),
                SunIllumination = x.Telemetries.Average(x => x.SunIllumination),

                GpsLocation = x.GpsAddress.GeocodedAddress,
                CreatedAt = x.CreatedAt,
            });

        // filter by keyword
        if (!string.IsNullOrWhiteSpace(req.Keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.GpsLocation, $"%{req.Keyword}%"));
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

                "airtemperature" => query.ApplySort(x => x.AirTemperature, req.OrderDirection),
                "airhumidity" => query.ApplySort(x => x.AirHumidity, req.OrderDirection),
                "soilmoisture" => query.ApplySort(x => x.SoilMoisture, req.OrderDirection),
                "airheatindex" => query.ApplySort(x => x.AirHeatIndex, req.OrderDirection),
                "sunillumination" => query.ApplySort(x => x.SunIllumination, req.OrderDirection),

                "gpslocation" => query.ApplySort(x => x.GpsLocation, req.OrderDirection),
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
