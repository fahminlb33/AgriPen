using AgriPenMobile.API;

namespace AgriPenMobile.Services;

public class SensorData
{
    public double AirTemperature { get; set; }
    public double AirHumidity { get; set; }
    public double SunIllumination { get; set; }
    public double SoilTemperature { get; set; }
    public double SoilMoisture { get; set; }

    public static SensorData Parse(string data)
    {
        var split = data.Split(',');

        return new()
        {
            AirTemperature = Helpers.ParseDouble(split[1]),
            AirHumidity = Helpers.ParseDouble(split[2]),
            SunIllumination = 12,
            SoilTemperature = 12,
            SoilMoisture = 12,
        };
    }
}
