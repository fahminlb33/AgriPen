using AgriPen.Domain.Entities;

namespace AgriPen.API.Recommendation;

public class ListItem
{
    public string Label { get; set; }
    public Ulid Value { get; set; }
}

public class PlantRecommendationDto
{
    public Ulid Id { get; set; }
    public string Name { get; set; }
    public string NameID { get; set; }

    public PlantSeasonDto Season { get; set; }
    public List<PlantNitrogenDto> Nitrogen { get; set; }
    public List<PlantPhosporusDto> Phosporus { get; set; }
    public List<PlantPotassiumDto> Potassium { get; set; }
    public List<PlantPhDto> Ph { get; set; }

}

public class PlantSeasonDto
{
    public string Season { get; set; }
    public double TempDayLow { get; set; }
    public double TempDayHigh { set; get; }
    public double TempNightLow { get; set; }
    public double TempNightHigh { get; set; }
    public double HumidityLow { get; set; }
    public double HumidityHigh { get; set; }
    public double SoilMoistureLow { get; set; }
    public double SoilMoistureHigh { get; set; }
}

public class PlantNitrogenDto
{
    public double Nitrogen { get; set; }
    public string Notes { get; set; }
}

public class PlantPhosporusDto
{
    public double Category1 { get; set; }
    public double Category2 { get; set; }
    public double Category3 { get; set; }
    public double Category4 { get; set; }
    public double Category5 { get; set; }
    public string Notes { get; set; }
}

public class PlantPotassiumDto
{
    public double Category1 { get; set; }
    public double Category2 { get; set; }
    public double Category3 { get; set; }
    public double Category4 { get; set; }
    public double Category5 { get; set; }
    public string Notes { get; set; }
}

public class PlantPhDto
{
    public double Optimal { get; set; }
    public double Minimum { get; set; }
    public string Notes { get; set; }
}


public static class PlantHelpers
{
    public static PlantSeasonDto ToDto(this PlantSeason season)
    {
        return new()
        {
            Season = season.Season,
            TempDayLow = season.TempDayLow,
            TempDayHigh = season.TempDayHigh,
            TempNightLow = season.TempNightLow,
            TempNightHigh = season.TempNightHigh,
            HumidityLow = season.HumidityLow,
            HumidityHigh = season.HumidityHigh,
            SoilMoistureLow = season.SoilMoistureLow,
            SoilMoistureHigh = season.SoilMoistureHigh,
        };
    }

    public static List<PlantNitrogenDto> ToDto(this IEnumerable<PlantNitrogen> d)
    {
        return d
            .DistinctBy(x => x.Id)
            .Select(x => new PlantNitrogenDto()
            {
                Nitrogen = x.Nitrogen,
                Notes = x.Notes,
            })
            .ToList();
    }

    public static List<PlantPhosporusDto> ToDto(this IEnumerable<PlantPhosporus> d)
    {
        return d
            .DistinctBy(x => x.Id)
            .Select(x => new PlantPhosporusDto()
            {
                Category1 = x.Category1,
                Category2 = x.Category2,
                Category3 = x.Category3,
                Category4 = x.Category4,
                Category5 = x.Category5,
                Notes = x.Notes,
            })
            .ToList();
    }

    public static List<PlantPotassiumDto> ToDto(this IEnumerable<PlantPotassium> d)
    {
        return d
            .DistinctBy(x => x.Id)
            .Select(x => new PlantPotassiumDto()
            {
                Category1 = x.Category1,
                Category2 = x.Category2,
                Category3 = x.Category3,
                Category4 = x.Category4,
                Category5 = x.Category5,
                Notes = x.Notes,
            })
            .ToList();
    }

    public static List<PlantPhDto> ToDto(this IEnumerable<PlantPh> d)
    {
        return d
            .DistinctBy(x => x.Id)
            .Select(x => new PlantPhDto()
            {
                Optimal = x.Optimal,
                Minimum = x.Minimum,
                Notes = x.Notes,
            })
            .ToList();
    }
}
