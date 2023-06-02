using AgriPen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.Infrastructure.Extensions;

public static class ReverseGeocodeExtensions
{
    public static async Task<LocalAddress> ReverseGeocodeAsync(this AgriDataContext context, double latitude, double longitude, CancellationToken ct = default)
    {
        return await context.LocalAddresses
            .OrderBy(x => context.HaversineDistance(x.Latitude, x.Longitude, latitude, longitude))
            .FirstAsync(ct);
    }
}
