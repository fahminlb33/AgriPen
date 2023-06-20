namespace AgriPenMobile.Services;

public class SensorData
{
    public double AirTemperature { get; set; }
    public double AirHumidity { get; set; }
    public double SunIllumination { get; set; }
    public double AirHeatIndex { get; set; }
    public double SoilMoisture { get; set; }

    public static SensorData Parse(string data)
    {
        var dict = data.Split(',')
            .Where(x => x.Contains(':'))
            .Select(x => x.Split(':'))
            .ToDictionary(x => x[0], y => Helpers.ParseDouble(y[1]));

        return new()
        {
            AirTemperature = dict["air_temp"],
            AirHumidity = dict["air_humi"],
            AirHeatIndex = dict["air_heat"],
            SoilMoisture = dict["soil_moi"],
            SunIllumination = dict["sun_illu"],
        };
    }
}
