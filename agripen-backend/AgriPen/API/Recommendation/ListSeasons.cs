using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Recommendation;

public class ListSeasonsEndpoint : EndpointWithoutRequest<List<DropListItem>>
{
    private readonly AgriDataContext _context;

    public ListSeasonsEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/recommendation/seasons");
        Roles();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var plants = await _context.PlantSeason
            .AsNoTracking()
            .Select(x => new DropListItem { Label = x.Season, Value = x.Id })
            .ToListAsync(ct);

        await SendOkAsync(plants, ct);
    }
}
