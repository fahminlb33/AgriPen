using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgriPen.API.Recommendation;

public class FindByPlantRequest
{
    [BindFrom("q")]
    public string Name { get; set; }
}

public class FindByPlantEndpoint : Endpoint<FindByPlantRequest, List<ListItem>>
{
    private readonly AgriDataContext _context;

    public FindByPlantEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/recommendation/plants");
        Roles();
    }

    public override async Task HandleAsync(FindByPlantRequest req, CancellationToken ct)
    {
        // query plant
        var data = await _context.Plants
            .AsNoTracking()
            .Include(x => x.Season)
            .Where(x => EF.Functions.Like(x.Name, $"%{req.Name}%") || EF.Functions.Like(x.NameID, $"%{req.Name}%"))
            .ToListAsync(ct);

        // plant is not found
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
