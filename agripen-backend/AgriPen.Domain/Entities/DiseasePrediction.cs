using System.ComponentModel.DataAnnotations;
using AgriPen.Domain.Enums;

namespace AgriPen.Domain.Entities;

public class DiseasePrediction : IEntityModel
{
    [Key, StringLength(26)]
    public Ulid Id { get; set; }

    public double Severity { get; set; }
    public DiseasePredictionResult Result { get; set; }
    public DiseasePredictionStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    [StringLength(26)]
    public Ulid ProbabilityId { get; set; }
    [StringLength(26)]
    public Ulid GpsAddressId { get; set; }
    [StringLength(26)]
    public Ulid LocalAddressId { get; set; }

    // navigation
    public DiseaseProbability Probability { get; set; }
    public GpsAddress GpsAddress { get; set; }
    public LocalAddress LocalAddress { get; set; }
}
