using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Recommendation;

public class FindByPlantRequest
{
    public Ulid PlantId { get; set; }
}

public class FindByPlantEndpoint : Endpoint<FindByPlantRequest, PlantRecommendationDto>
{
    private readonly AgriDataContext _context;

    public FindByPlantEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/recommendation/plants/{PlantId}");
        Roles();
    }

    public override async Task HandleAsync(FindByPlantRequest req, CancellationToken ct)
    {
        // query plant
        var plant = await _context.Plants
            .AsNoTracking()
            .Include(x => x.Season)
            .Include(x => x.Nitrogen)
            .Include(x => x.Phosporus)
            .Include(x => x.Potassium)
            .Include(x => x.Ph)
            .FirstOrDefaultAsync(x => x.Id == req.PlantId, ct);

        // plant is not found
        if (plant == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // project
        await SendOkAsync(new PlantRecommendationDto()
        {
            Id = plant.Id,
            Name = plant.Name,
            NameID = plant.NameID,

            Season = plant.Season.ToDto(),
            Nitrogen = plant.Nitrogen.ToDto(),
            Phosporus = plant.Phosporus.ToDto(),
            Potassium = plant.Potassium.ToDto(),
            Ph = plant.Ph.ToDto(),
        }, ct);
    }
}
