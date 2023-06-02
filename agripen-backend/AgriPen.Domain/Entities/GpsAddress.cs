using System.ComponentModel.DataAnnotations;

namespace AgriPen.Domain.Entities;

public class GpsAddress : IEntityModel
{
    [Key, StringLength(26)]
    public Ulid Id { get; set; }

    public string GeocodedAddress { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double HorizontalAccuracy { get; set; }
    public double VerticalAccuracy { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // navigation
    public List<DiseasePrediction> DiseasePredictions { get; set; }
    public List<LandObservation> LandObservations { get; set; }
}
