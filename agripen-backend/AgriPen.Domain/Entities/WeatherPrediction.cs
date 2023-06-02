using AgriPen.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgriPen.Domain.Entities;

public class WeatherPrediction
{
    [Key, StringLength(26)]
    public Ulid Id { get; set; }

    public DateTimeOffset Timestamp { get; set; }
    public double Temperature { get; set; }
    public double TemperatureLow { get; set; }
    public double TemperatureHigh { get; set; }
    public double Humidity { get; set; }
    public double HumidityLow { get; set; }
    public double HumidityHigh { get; set; }
    public WeatherCode Weather { get; set; }
    public WindCode Wind { get; set; }
    public double WindSpeed { get; set; }

    [StringLength(26)]
    public Ulid LocalAddressId { get; set; }

    // navigation
    public LocalAddress LocalAddress { get; set; }
}
