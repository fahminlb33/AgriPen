namespace AgriPen.Domain.Entities;

public class Plant
{
    public Ulid Id { get; set; }
    public string Name { get; set; }
    public string NameID { get; set; }

    public Ulid SeasonId { get; set; }
    public PlantSeason Season { get; set; }
    public List<PlantNitrogen> Nitrogen { get; set; }
    public List<PlantPhosporus> Phosporus { get; set; }
    public List<PlantPotassium> Potassium { get; set; }
    public List<PlantPh> Ph { get; set; }
}

public class PlantSeason
{
    public Ulid Id { get; set; }
    public string Season { get; set; }
    public double TempDayLow { get; set; }
    public double TempDayHigh { set; get; }
    public double TempNightLow { get; set; }
    public double TempNightHigh { get; set; }
    public double HumidityLow { get; set; }
    public double HumidityHigh { get; set; }
    public double SoilMoistureLow { get; set; }
    public double SoilMoistureHigh { get; set; }

    public List<Plant> Plant { get; set; }
}

public class PlantNitrogen
{
    public Ulid Id { get; set; }
    public double Nitrogen { get; set; }
    public string Notes { get; set; }

    public Ulid PlantId { get; set; }
    public Plant Plant { get; set; }
}

public class PlantPhosporus
{
    public Ulid Id { get; set; }
    public double Category1 { get; set; }
    public double Category2 { get; set; }
    public double Category3 { get; set; }
    public double Category4 { get; set; }
    public double Category5 { get; set; }
    public string Notes { get; set; }

    public Ulid PlantId { get; set; }
    public Plant Plant { get; set; }
}

public class PlantPotassium
{
    public Ulid Id { get; set; }
    public double Category1 { get; set; }
    public double Category2 { get; set; }
    public double Category3 { get; set; }
    public double Category4 { get; set; }
    public double Category5 { get; set; }
    public string Notes { get; set; }

    public Ulid PlantId { get; set; }
    public Plant Plant { get; set; }
}

public class PlantPh
{
    public Ulid Id { get; set; }
    public double Optimal { get; set; }
    public double Minimum { get; set; }
    public string Notes { get; set; }

    public Ulid PlantId { get; set; }
    public Plant Plant { get; set; }
}
