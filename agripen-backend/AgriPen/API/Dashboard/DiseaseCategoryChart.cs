using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Dashboard;

public class DiseaseChartEndpoint : Endpoint<DashboardRequest, List<ChartPoint>>
{
    private readonly AgriDataContext _context;

    public DiseaseChartEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/dashboard/disease-category-chart");
        Roles();
    }

    public override async Task HandleAsync(DashboardRequest req, CancellationToken ct)
    {
        var rows = await _context.DiseasePredictions
            .AsNoTracking()
            .GroupBy(x => x.Result)
            .Select(x => new ChartPoint() { Name = x.Key.ToString(), Value = x.LongCount() })
            .ToListAsync(ct);

        await SendOkAsync(rows, ct);
    }
}
