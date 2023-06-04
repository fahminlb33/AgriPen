using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Recommendation;

public class FindByObservationRequest
{
    public Ulid ObservationId { get; set; }
}

public class FindByObservationEndpoint : Endpoint<FindByObservationRequest, List<PlantRecommendationDto>>
{
    private readonly AgriDataContext _context;

    public FindByObservationEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/recommendation/observations/{ObservationId}");
        Roles();
    }

    public override async Task HandleAsync(FindByObservationRequest req, CancellationToken ct)
    {
        // get the observation
        var observation = await _context.LandObservations
            .AsNoTracking()
            .Include(x => x.Telemetries)
            .FirstOrDefaultAsync(x => req.ObservationId == req.ObservationId, ct);

        // observation not found
        if (observation == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // get all matching plants
        var query = _context.Plants
          .AsNoTracking()
          .Include(x => x.Season)
          .Include(x => x.Nitrogen)
          .Include(x => x.Phosporus)
          .Include(x => x.Potassium)
          .Include(x => x.Ph)
          .AsQueryable();

        // filter by temperature
        var avgTemperature = observation.Telemetries.Average(x => x.AirTemperature);
        query = query
            .Where(x => x.Season.TempDayLow >= avgTemperature && x.Season.TempNightHigh <= avgTemperature)
            .Where(x => x.Season.TempNightLow >= avgTemperature && x.Season.TempNightHigh <= avgTemperature);

        // filter by humidity
        var avgHumidity = observation.Telemetries.Average(x => x.AirHumidity);
        query = query.Where(x => x.Season.HumidityLow >= avgHumidity && x.Season.HumidityHigh <= avgHumidity);

        // filter by soil moisture
        var avgSoilMoisture = observation.Telemetries.Average(x => x.SoilMoisture);
        query = query.Where(x => x.Season.SoilMoistureLow >= avgSoilMoisture && x.Season.SoilMoistureHigh <= avgSoilMoisture);

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