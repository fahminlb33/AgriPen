using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Recommendation;

public class ListPlantsEndpoint: EndpointWithoutRequest<List<ListItem>>
{
    private readonly AgriDataContext _context;

    public ListPlantsEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/recommendation/plants");
        Roles();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var plants = await _context.Plants
            .AsNoTracking()
            .Select(x => new ListItem { Label = $"{x.NameID} ({x.Name})", Value = x.Id })
            .ToListAsync(ct);

        await SendOkAsync(plants, ct);
    }
}
