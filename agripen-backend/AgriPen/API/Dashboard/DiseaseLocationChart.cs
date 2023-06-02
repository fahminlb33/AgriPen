using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Dashboard;

public class DiseaseLocationChart : Endpoint<DashboardRequest, List<ChartPoint>>
{
    private readonly AgriDataContext _context;

    public DiseaseLocationChart(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/dashboard/disease-location-chart");
        Roles();
    }

    public override async Task HandleAsync(DashboardRequest req, CancellationToken ct)
    {
        var rows = await _context.DiseasePredictions
            .AsNoTracking()
            .Include(x => x.LocalAddress)
            .GroupBy(x => x.LocalAddress.Kecamatan)
            .Select(x => new ChartPoint() { Name = x.Key, Value = x.LongCount() })
            .Take(100)
            .ToListAsync(ct);

        await SendOkAsync(rows, ct);
    }
}
