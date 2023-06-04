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

public class Telemetry
{
    [Key, StringLength(26)]
    public Ulid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public double AirTemperature { get; set; }
    public double AirHumidity { get; set; }
    public double AirHeatIndex { get; set; }
    public double SoilMoisture { get; set; }
    public double SunIllumination { get; set; }

    [StringLength(26)]
    public Ulid ObservationId { get; set; }

    // navigation
    public LandObservation Observation { get; set; }
}
