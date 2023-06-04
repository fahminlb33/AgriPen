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

public class FindByParametersEndpoint : Endpoint<FindByParametersRequest, List<PlantRecommendationDto>>
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
            .Include(x => x.Nitrogen)
            .Include(x => x.Phosporus)
            .Include(x => x.Potassium)
            .Include(x => x.Ph)
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
                .Where(x => x.Season.TempDayLow >= req.Temperature && x.Season.TempNightHigh <= req.Temperature)
                .Where(x => x.Season.TempNightLow >= req.Temperature && x.Season.TempNightHigh <= req.Temperature);
        }

        // filter by humidity
        if (req.Humidity > 0)
        {
            query = query.Where(x => x.Season.HumidityLow >= req.Humidity && x.Season.HumidityHigh <= req.Humidity);
        }

        // filter by soil moisture
        if (req.SoilMoisture > 0)
        {
            query = query.Where(x => x.Season.SoilMoistureLow >= req.SoilMoisture && x.Season.SoilMoistureHigh <= req.SoilMoisture);
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
            .Select(x => new PlantRecommendationDto()
            {
                Id = x.Id,
                Name = x.Name,
                NameID = x.NameID,

                Season = x.Season.ToDto(),
                Nitrogen = x.Nitrogen.ToDto(),
                Phosporus = x.Phosporus.ToDto(),
                Potassium = x.Potassium.ToDto(),
                Ph = x.Ph.ToDto(),
            })
            .ToList();

        await SendOkAsync(mapped, ct);
    }
}
