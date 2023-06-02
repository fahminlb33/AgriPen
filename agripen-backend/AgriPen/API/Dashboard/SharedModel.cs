using FastEndpoints;

namespace AgriPen.API.Dashboard;

public class DashboardRequest
{
    [BindFrom("start_date")]
    public DateOnly StartDate { get; set; }
    [BindFrom("end_date")]
    public DateOnly EndDate { get; set; }
}

public class ChartPoint
{
    public string Name { get; set; }
    public long Value { get; set; }
}
