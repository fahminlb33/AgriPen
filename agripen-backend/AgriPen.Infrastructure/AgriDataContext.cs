using AgriPen.Domain.Entities;
using AgriPen.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;

namespace AgriPen.Infrastructure;

public class AgriDataContext : DbContext
{
    public AgriDataContext(DbContextOptions<AgriDataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<GpsAddress> GpsAddresses { get; set; }
    public DbSet<LocalAddress> LocalAddresses { get; set; }
    public DbSet<LandObservation> LandObservations { get; set; }
    public DbSet<DiseasePrediction> DiseasePredictions { get; set; }
    public DbSet<WeatherPrediction> WeatherPredictions { get; set; }
    public DbSet<Telemetry> Telemetries { get; set; }

    public int HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        throw new NotSupportedException();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // register function
        var haversineRef = typeof(AgriDataContext).GetMethod(nameof(HaversineDistance), new[] { typeof(double), typeof(double), typeof(double), typeof(double) });
        modelBuilder
            .HasDbFunction(haversineRef)
            .HasName("HaversineDistance");

        // configure user
        modelBuilder
            .Entity<User>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();

        // configure gps address
        modelBuilder
            .Entity<GpsAddress>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<GpsAddress>()
            .HasMany(x => x.DiseasePredictions)
            .WithOne(x => x.GpsAddress)
            .HasForeignKey(x => x.GpsAddressId);
        modelBuilder
            .Entity<GpsAddress>()
            .HasMany(x => x.LandObservations)
            .WithOne(x => x.GpsAddress)
            .HasForeignKey(x => x.GpsAddressId);

        // configure local address
        modelBuilder
            .Entity<LocalAddress>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<LocalAddress>()
            .HasMany(x => x.WeatherPredictions)
            .WithOne(x => x.LocalAddress)
            .HasForeignKey(x => x.LocalAddressId);
        modelBuilder
            .Entity<LocalAddress>()
            .HasMany(x => x.DiseasePredictions)
            .WithOne(x => x.LocalAddress)
            .HasForeignKey(x => x.LocalAddressId);
        modelBuilder
            .Entity<LocalAddress>()
            .HasMany(x => x.LandObservations)
            .WithOne(x => x.LocalAddress)
            .HasForeignKey(x => x.LocalAddressId);

        // configure weather prediction
        modelBuilder
            .Entity<WeatherPrediction>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<WeatherPrediction>()
            .Property(x => x.LocalAddressId)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<WeatherPrediction>()
            .HasOne(x => x.LocalAddress)
            .WithMany(x => x.WeatherPredictions)
            .HasForeignKey(x => x.LocalAddressId);

        // configure disease probability
        modelBuilder
            .Entity<DiseaseProbability>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<DiseaseProbability>()
            .Property(x => x.PredictionId)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<DiseaseProbability>()
            .HasOne(x => x.Prediction)
            .WithOne(x => x.Probability)
            .HasForeignKey<DiseasePrediction>(x => x.ProbabilityId);

        // configure disease prediction
        modelBuilder
            .Entity<DiseasePrediction>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<DiseasePrediction>()
            .Property(x => x.ProbabilityId)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<DiseasePrediction>()
            .Property(x => x.GpsAddressId)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
           .Entity<DiseasePrediction>()
           .Property(x => x.LocalAddressId)
           .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<DiseasePrediction>()
            .HasOne(x => x.Probability)
            .WithOne(x => x.Prediction)
            .HasForeignKey<DiseaseProbability>(x => x.PredictionId);
        modelBuilder
            .Entity<DiseasePrediction>()
            .HasOne(x => x.GpsAddress)
            .WithMany(x => x.DiseasePredictions)
            .HasForeignKey(x => x.GpsAddressId);
        modelBuilder
            .Entity<DiseasePrediction>()
            .HasOne(x => x.LocalAddress)
            .WithMany(x => x.DiseasePredictions)
            .HasForeignKey(x => x.LocalAddressId);

        // configure telemetry
        modelBuilder
            .Entity<Telemetry>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<Telemetry>()
            .Property(x => x.ObservationId)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<Telemetry>()
            .HasOne(x => x.Observation)
            .WithMany(x => x.Telemetries)
            .HasForeignKey(x => x.ObservationId);

        // configure land observation
        modelBuilder
            .Entity<LandObservation>()
            .Property(x => x.Id)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<LandObservation>()
            .Property(x => x.GpsAddressId)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<LandObservation>()
            .Property(x => x.LocalAddressId)
            .HasConversion<EFUlidConverter, EFUlidComparer>();
        modelBuilder
            .Entity<LandObservation>()
            .HasOne(x => x.GpsAddress)
            .WithMany(x => x.LandObservations)
            .HasForeignKey(x => x.GpsAddressId);
        modelBuilder
            .Entity<LandObservation>()
            .HasOne(x => x.LocalAddress)
            .WithMany(x => x.LandObservations)
            .HasForeignKey(x => x.LocalAddressId);
        modelBuilder
            .Entity<LandObservation>()
            .HasMany(x => x.Telemetries)
            .WithOne(x => x.Observation)
            .HasForeignKey(x => x.ObservationId);
    }
}
