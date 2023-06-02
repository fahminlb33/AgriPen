using System.ComponentModel.DataAnnotations;

namespace AgriPen.Domain.Entities;

public class LandObservation : IEntityModel
{
    [Key, StringLength(26)]
    public Ulid Id { get; set; }

    [StringLength(26)]
    public Ulid GpsAddressId { get; set; }
    [StringLength(26)]
    public Ulid LocalAddressId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // navigation
    public GpsAddress GpsAddress { get; set; }
    public LocalAddress LocalAddress { get; set; }
    public List<Telemetry> Telemetries { get; set; }
}
