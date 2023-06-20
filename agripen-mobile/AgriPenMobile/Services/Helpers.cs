using AgriPenMobile.API;
using System.Globalization;

namespace AgriPenMobile.Services;

public static class Helpers
{
    public static double ParseDouble(string value, double defaultValue = 0)
    {
        if (double.TryParse(value, CultureInfo.GetCultureInfo("en-US"), out var result))
        {
            return result;
        }

        return defaultValue;
    }

    public static TelemetryItem ToTelemetry(this SensorData d)
    {
        return new()
        {
            AirTemperature = d.AirTemperature,
            AirHumidity = d.AirHumidity,
            AirHeatIndex = d.AirHeatIndex,
            SunIllumination = d.SunIllumination,
            SoilMoisture = d.SoilMoisture,
        };
    }

    public static GpsData ToGpsData(this Location l)
    {
        return new()
        {
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            Altitude = l.Altitude ?? 0,
            HorizontalAccuracy = l.Accuracy ?? 0,
            VerticalAccuracy = l.VerticalAccuracy ?? 0
        };
    }
}
