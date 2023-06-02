using System.ComponentModel.DataAnnotations;

namespace AgriPen.Domain.Entities;

public class DiseaseProbability
{
    [Key, StringLength(26)]
    public Ulid Id { get; set; }

    public double BacterialBlight { get; set; }
    public double Blast { get; set; }
    public double BrownSpot { get; set; }
    public double Tungro { get; set; }
    public double Healthy { get; set; }

    [StringLength(26)]
    public Ulid PredictionId { get; set; }

    // navigation
    public DiseasePrediction Prediction { get; set; }
}
