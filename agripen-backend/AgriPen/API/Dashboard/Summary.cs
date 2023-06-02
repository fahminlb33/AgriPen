using AgriPen.Domain.Enums;
using AgriPen.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.API.Dashboard;

public class SummaryResponse
{
    public long LandObservations { get; set; }
    public long DiseasePredictions { get; set; }
    public long PositiveDisease { get; set; }
    public long NegativeDisease { get; set; }
}

public class SummaryEndpoint : Endpoint<DashboardRequest, SummaryResponse>
{
    private readonly AgriDataContext _context;

    public SummaryEndpoint(AgriDataContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/dashboard/summary");
        Roles();
    }

    public override async Task HandleAsync(DashboardRequest req, CancellationToken ct)
    {
        var landObservations = await _context.LandObservations.LongCountAsync(ct);
        var diseasePredictions = await _context.DiseasePredictions.LongCountAsync(ct);
        var positiveDisease = await _context.DiseasePredictions.LongCountAsync(x => x.Result != DiseasePredictionResult.Healthy, ct);
        var negativeDisease = await _context.DiseasePredictions.LongCountAsync(x => x.Result == DiseasePredictionResult.Healthy, ct);

        await SendOkAsync(new()
        {
            LandObservations = landObservations,
            DiseasePredictions = diseasePredictions,
            PositiveDisease = positiveDisease,
            NegativeDisease = negativeDisease
        }, ct);
    }
}