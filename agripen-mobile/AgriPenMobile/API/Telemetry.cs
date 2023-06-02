namespace AgriPenMobile.API;

public class TelemetryRequest
{
    public GpsData GpsAddress { get; set; }
    public List<TelemetryItem> TimeSeries { get; set; }
}

public class TelemetryResponse
{
    public string Id { get; set; }
}

public class TelemetryItem
{
    public DateTime Timestamp { get; set; }
    public double AirTemperature { get; set; }
    public double AirHumidity { get; set; }
    public double SoilTemperature { get; set; }
    public double SoilMoisture { get; set; }
    public double SunIllumination { get; set; }
}
