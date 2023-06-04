using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Recommendation;

public class FindByParametersRequest
{
    [BindFrom("season_id")]
    public Ulid? SeasonId { get; set; }
    [BindFrom("t")]
    public double? Temperature { get; set; }
    [BindFrom("rh")]
    public double? Humidity { get; set; }
    [BindFrom("srh")]
    public double? SoilMoisture { get; set; }
}

public class FindByParametersEndpoint : Endpoint<FindByParametersRequest, List<ListItem>>
{
    private readonly AgriDataContext _context;

    public FindByParametersEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/recommendation/params");
        Roles();
    }

    public override async Task HandleAsync(FindByParametersRequest req, CancellationToken ct)
    {
        // query all plants
        var query = _context.Plants
            .AsNoTracking()
            .Include(x => x.Season)
            .AsQueryable();

        // filter by season
        if (req.SeasonId.HasValue)
        {
            query = query.Where(x => x.Season.Id == req.SeasonId);
        }

        // filter by temperature
        if (req.Temperature > 0)
        {
            query = query
                .Where(x => req.Temperature >= x.Season.TempDayLow && req.Temperature <= x.Season.TempNightHigh)
                .Where(x => req.Temperature >=x.Season.TempNightLow && req.Temperature <= x.Season.TempNightHigh);
        }

        // filter by humidity
        if (req.Humidity > 0)
        {
            query = query.Where(x => req.Humidity >= x.Season.HumidityLow &&  req.Humidity<= x.Season.HumidityHigh);
        }

        // filter by soil moisture
        if (req.SoilMoisture > 0)
        {
            query = query.Where(x => req.SoilMoisture >= x.Season.SoilMoistureLow && req.SoilMoisture <= x.Season.SoilMoistureHigh);
        }

        // get all
        var data = await query.ToListAsync(ct);
        if (data.Count == 0)
        {
            await SendNoContentAsync(ct);
            return;
        }

        // project
        var mapped = data
            .Select(x => new ListItem()
            {
                ID = x.Id,
                Name = x.Name,
                NameID = x.NameID,
                Season = x.Season.Season,
            })
            .ToList();

        await SendOkAsync(mapped, ct);
    }
}
