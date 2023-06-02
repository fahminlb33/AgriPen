using AgriPen.Domain.Entities;

namespace AgriPen.Helpers;

public class GpsAddressDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string GeocodedAddress { get; set; }
    public double Altitude { get; set; }
    public double HorizontalAccuracy { get; set; }
    public double VerticalAccuracy { get; set; }
}

public class LocalAddressDto
{
    public string Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Kecamatan { get; set; }
    public string Kabupaten { get; set; }
    public string Provinsi { get; set; }
}

public class TelemetryDto
{
    public DateTimeOffset Timestamp { get; set; }
    public double AirTemperature { get; set; }
    public double AirHumidity { get; set; }
    public double AirHeatIndex { get; set; }
    public double SoilMoisture { get; set; }
    public double SunIllumination { get; set; }
}


public static class MapperHelper
{
    public static GpsAddressDto ToDto(this GpsAddress address)
    {
        return new()
        {
            Latitude = address.Latitude,
            Longitude = address.Longitude,
            GeocodedAddress = address.GeocodedAddress,
            Altitude = address.Altitude,
            HorizontalAccuracy = address.HorizontalAccuracy,
            VerticalAccuracy = address.VerticalAccuracy
        };
    }

    public static LocalAddressDto ToDto(this LocalAddress location)
    {
        return new()
        {
            Id = location.Id.ToString(),
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Kecamatan = location.Kecamatan,
            Kabupaten = location.Kabupaten,
            Provinsi = location.Provinsi
        };
    }

    public static TelemetryDto ToDto(this Telemetry location)
    {
        return new()
        {
            AirTemperature = location.AirTemperature,
            AirHumidity = location.AirHumidity,
            AirHeatIndex = location.AirHeatIndex,
            SoilMoisture = location.SoilMoisture,
            SunIllumination = location.SunIllumination,
        };
    }
}
