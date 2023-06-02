using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AgriPen.Infrastructure.Converters;

public class EFUlidConverter : ValueConverter<Ulid, string>
{
    public EFUlidConverter() : base(v => v.ToString(), v => Ulid.Parse(v))
    {
    }
}

public class EFUlidComparer : ValueComparer<Ulid>
{
    public EFUlidComparer() : base((l, r) => l.Equals(r), v => v.GetHashCode(), v => Ulid.Parse(v.ToString()))
    {
    }
}
