﻿// <auto-generated />
using System;
using AgriPen.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AgriPen.Migrations
{
    [DbContext(typeof(AgriDataContext))]
    partial class AgriDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AgriPen.Domain.Entities.DiseasePrediction", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("created_at");

                    b.Property<string>("GpsAddressId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("gps_address_id");

                    b.Property<string>("LocalAddressId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("local_address_id");

                    b.Property<string>("ProbabilityId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("probability_id");

                    b.Property<int>("Result")
                        .HasColumnType("int")
                        .HasColumnName("result");

                    b.Property<double>("Severity")
                        .HasColumnType("float")
                        .HasColumnName("severity");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_disease_predictions");

                    b.HasIndex("GpsAddressId")
                        .HasDatabaseName("ix_disease_predictions_gps_address_id");

                    b.HasIndex("LocalAddressId")
                        .HasDatabaseName("ix_disease_predictions_local_address_id");

                    b.ToTable("disease_predictions", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.DiseaseProbability", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<double>("BacterialBlight")
                        .HasColumnType("float")
                        .HasColumnName("bacterial_blight");

                    b.Property<double>("Blast")
                        .HasColumnType("float")
                        .HasColumnName("blast");

                    b.Property<double>("BrownSpot")
                        .HasColumnType("float")
                        .HasColumnName("brown_spot");

                    b.Property<double>("Healthy")
                        .HasColumnType("float")
                        .HasColumnName("healthy");

                    b.Property<string>("PredictionId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("prediction_id");

                    b.Property<double>("Tungro")
                        .HasColumnType("float")
                        .HasColumnName("tungro");

                    b.HasKey("Id")
                        .HasName("pk_disease_probability");

                    b.HasIndex("PredictionId")
                        .IsUnique()
                        .HasDatabaseName("ix_disease_probability_prediction_id");

                    b.ToTable("disease_probability", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.GpsAddress", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<double>("Altitude")
                        .HasColumnType("float")
                        .HasColumnName("altitude");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("created_at");

                    b.Property<string>("GeocodedAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("geocoded_address");

                    b.Property<double>("HorizontalAccuracy")
                        .HasColumnType("float")
                        .HasColumnName("horizontal_accuracy");

                    b.Property<double>("Latitude")
                        .HasColumnType("float")
                        .HasColumnName("latitude");

                    b.Property<double>("Longitude")
                        .HasColumnType("float")
                        .HasColumnName("longitude");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("updated_at");

                    b.Property<double>("VerticalAccuracy")
                        .HasColumnType("float")
                        .HasColumnName("vertical_accuracy");

                    b.HasKey("Id")
                        .HasName("pk_gps_addresses");

                    b.ToTable("gps_addresses", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.LandObservation", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("created_at");

                    b.Property<string>("GpsAddressId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("gps_address_id");

                    b.Property<string>("LocalAddressId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("local_address_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_land_observations");

                    b.HasIndex("GpsAddressId")
                        .HasDatabaseName("ix_land_observations_gps_address_id");

                    b.HasIndex("LocalAddressId")
                        .HasDatabaseName("ix_land_observations_local_address_id");

                    b.ToTable("land_observations", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.LocalAddress", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)")
                        .HasColumnName("code");

                    b.Property<string>("Kabupaten")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("kabupaten");

                    b.Property<string>("Kecamatan")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("kecamatan");

                    b.Property<double>("Latitude")
                        .HasColumnType("float")
                        .HasColumnName("latitude");

                    b.Property<double>("Longitude")
                        .HasColumnType("float")
                        .HasColumnName("longitude");

                    b.Property<string>("Provinsi")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("provinsi");

                    b.HasKey("Id")
                        .HasName("pk_local_addresses");

                    b.ToTable("local_addresses", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.Plant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("NameID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name_id");

                    b.Property<string>("SeasonId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("season_id");

                    b.HasKey("Id")
                        .HasName("pk_plants");

                    b.HasIndex("SeasonId")
                        .HasDatabaseName("ix_plants_season_id");

                    b.ToTable("plants", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantNitrogen", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<double>("Nitrogen")
                        .HasColumnType("float")
                        .HasColumnName("nitrogen");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<string>("PlantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("plant_id");

                    b.HasKey("Id")
                        .HasName("pk_plant_nitrogen");

                    b.HasIndex("PlantId")
                        .HasDatabaseName("ix_plant_nitrogen_plant_id");

                    b.ToTable("plant_nitrogen", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantPh", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<double>("Minimum")
                        .HasColumnType("float")
                        .HasColumnName("minimum");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<double>("Optimal")
                        .HasColumnType("float")
                        .HasColumnName("optimal");

                    b.Property<string>("PlantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("plant_id");

                    b.HasKey("Id")
                        .HasName("pk_plant_ph");

                    b.HasIndex("PlantId")
                        .HasDatabaseName("ix_plant_ph_plant_id");

                    b.ToTable("plant_ph", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantPhosporus", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<double>("Category1")
                        .HasColumnType("float")
                        .HasColumnName("category1");

                    b.Property<double>("Category2")
                        .HasColumnType("float")
                        .HasColumnName("category2");

                    b.Property<double>("Category3")
                        .HasColumnType("float")
                        .HasColumnName("category3");

                    b.Property<double>("Category4")
                        .HasColumnType("float")
                        .HasColumnName("category4");

                    b.Property<double>("Category5")
                        .HasColumnType("float")
                        .HasColumnName("category5");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<string>("PlantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("plant_id");

                    b.HasKey("Id")
                        .HasName("pk_plant_phosporus");

                    b.HasIndex("PlantId")
                        .HasDatabaseName("ix_plant_phosporus_plant_id");

                    b.ToTable("plant_phosporus", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantPotassium", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<double>("Category1")
                        .HasColumnType("float")
                        .HasColumnName("category1");

                    b.Property<double>("Category2")
                        .HasColumnType("float")
                        .HasColumnName("category2");

                    b.Property<double>("Category3")
                        .HasColumnType("float")
                        .HasColumnName("category3");

                    b.Property<double>("Category4")
                        .HasColumnType("float")
                        .HasColumnName("category4");

                    b.Property<double>("Category5")
                        .HasColumnType("float")
                        .HasColumnName("category5");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("notes");

                    b.Property<string>("PlantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("plant_id");

                    b.HasKey("Id")
                        .HasName("pk_plant_potassium");

                    b.HasIndex("PlantId")
                        .HasDatabaseName("ix_plant_potassium_plant_id");

                    b.ToTable("plant_potassium", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantSeason", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<double>("HumidityHigh")
                        .HasColumnType("float")
                        .HasColumnName("humidity_high");

                    b.Property<double>("HumidityLow")
                        .HasColumnType("float")
                        .HasColumnName("humidity_low");

                    b.Property<string>("Season")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("season");

                    b.Property<double>("SoilMoistureHigh")
                        .HasColumnType("float")
                        .HasColumnName("soil_moisture_high");

                    b.Property<double>("SoilMoistureLow")
                        .HasColumnType("float")
                        .HasColumnName("soil_moisture_low");

                    b.Property<double>("TempDayHigh")
                        .HasColumnType("float")
                        .HasColumnName("temp_day_high");

                    b.Property<double>("TempDayLow")
                        .HasColumnType("float")
                        .HasColumnName("temp_day_low");

                    b.Property<double>("TempNightHigh")
                        .HasColumnType("float")
                        .HasColumnName("temp_night_high");

                    b.Property<double>("TempNightLow")
                        .HasColumnType("float")
                        .HasColumnName("temp_night_low");

                    b.HasKey("Id")
                        .HasName("pk_plant_season");

                    b.ToTable("plant_season", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.Telemetry", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<double>("AirHeatIndex")
                        .HasColumnType("float")
                        .HasColumnName("air_heat_index");

                    b.Property<double>("AirHumidity")
                        .HasColumnType("float")
                        .HasColumnName("air_humidity");

                    b.Property<double>("AirTemperature")
                        .HasColumnType("float")
                        .HasColumnName("air_temperature");

                    b.Property<string>("ObservationId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("observation_id");

                    b.Property<double>("SoilMoisture")
                        .HasColumnType("float")
                        .HasColumnName("soil_moisture");

                    b.Property<double>("SunIllumination")
                        .HasColumnType("float")
                        .HasColumnName("sun_illumination");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("timestamp");

                    b.HasKey("Id")
                        .HasName("pk_telemetries");

                    b.HasIndex("ObservationId")
                        .HasDatabaseName("ix_telemetries_observation_id");

                    b.ToTable("telemetries", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("hashed_password");

                    b.Property<DateTimeOffset?>("LastLoginAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("last_login_at");

                    b.Property<int>("Role")
                        .HasColumnType("int")
                        .HasColumnName("role");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.WeatherPrediction", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("id");

                    b.Property<double>("Humidity")
                        .HasColumnType("float")
                        .HasColumnName("humidity");

                    b.Property<double>("HumidityHigh")
                        .HasColumnType("float")
                        .HasColumnName("humidity_high");

                    b.Property<double>("HumidityLow")
                        .HasColumnType("float")
                        .HasColumnName("humidity_low");

                    b.Property<string>("LocalAddressId")
                        .IsRequired()
                        .HasMaxLength(26)
                        .HasColumnType("nvarchar(26)")
                        .HasColumnName("local_address_id");

                    b.Property<double>("Temperature")
                        .HasColumnType("float")
                        .HasColumnName("temperature");

                    b.Property<double>("TemperatureHigh")
                        .HasColumnType("float")
                        .HasColumnName("temperature_high");

                    b.Property<double>("TemperatureLow")
                        .HasColumnType("float")
                        .HasColumnName("temperature_low");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("timestamp");

                    b.Property<int>("Weather")
                        .HasColumnType("int")
                        .HasColumnName("weather");

                    b.Property<int>("Wind")
                        .HasColumnType("int")
                        .HasColumnName("wind");

                    b.Property<double>("WindSpeed")
                        .HasColumnType("float")
                        .HasColumnName("wind_speed");

                    b.HasKey("Id")
                        .HasName("pk_weather_predictions");

                    b.HasIndex("LocalAddressId")
                        .HasDatabaseName("ix_weather_predictions_local_address_id");

                    b.ToTable("weather_predictions", (string)null);
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.DiseasePrediction", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.GpsAddress", "GpsAddress")
                        .WithMany("DiseasePredictions")
                        .HasForeignKey("GpsAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_disease_predictions_gps_addresses_gps_address_id");

                    b.HasOne("AgriPen.Domain.Entities.LocalAddress", "LocalAddress")
                        .WithMany("DiseasePredictions")
                        .HasForeignKey("LocalAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_disease_predictions_local_addresses_local_address_id");

                    b.Navigation("GpsAddress");

                    b.Navigation("LocalAddress");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.DiseaseProbability", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.DiseasePrediction", "Prediction")
                        .WithOne("Probability")
                        .HasForeignKey("AgriPen.Domain.Entities.DiseaseProbability", "PredictionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_disease_probability_disease_predictions_prediction_id");

                    b.Navigation("Prediction");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.LandObservation", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.GpsAddress", "GpsAddress")
                        .WithMany("LandObservations")
                        .HasForeignKey("GpsAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_land_observations_gps_addresses_gps_address_id");

                    b.HasOne("AgriPen.Domain.Entities.LocalAddress", "LocalAddress")
                        .WithMany("LandObservations")
                        .HasForeignKey("LocalAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_land_observations_local_addresses_local_address_id");

                    b.Navigation("GpsAddress");

                    b.Navigation("LocalAddress");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.Plant", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.PlantSeason", "Season")
                        .WithMany("Plant")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_plants_plant_season_season_id");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantNitrogen", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.Plant", "Plant")
                        .WithMany("Nitrogen")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_plant_nitrogen_plants_plant_id");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantPh", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.Plant", "Plant")
                        .WithMany("Ph")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_plant_ph_plants_plant_id");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantPhosporus", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.Plant", "Plant")
                        .WithMany("Phosporus")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_plant_phosporus_plants_plant_id");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantPotassium", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.Plant", "Plant")
                        .WithMany("Potassium")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_plant_potassium_plants_plant_id");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.Telemetry", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.LandObservation", "Observation")
                        .WithMany("Telemetries")
                        .HasForeignKey("ObservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_telemetries_land_observations_observation_id");

                    b.Navigation("Observation");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.WeatherPrediction", b =>
                {
                    b.HasOne("AgriPen.Domain.Entities.LocalAddress", "LocalAddress")
                        .WithMany("WeatherPredictions")
                        .HasForeignKey("LocalAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_weather_predictions_local_addresses_local_address_id");

                    b.Navigation("LocalAddress");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.DiseasePrediction", b =>
                {
                    b.Navigation("Probability")
                        .IsRequired();
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.GpsAddress", b =>
                {
                    b.Navigation("DiseasePredictions");

                    b.Navigation("LandObservations");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.LandObservation", b =>
                {
                    b.Navigation("Telemetries");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.LocalAddress", b =>
                {
                    b.Navigation("DiseasePredictions");

                    b.Navigation("LandObservations");

                    b.Navigation("WeatherPredictions");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.Plant", b =>
                {
                    b.Navigation("Nitrogen");

                    b.Navigation("Ph");

                    b.Navigation("Phosporus");

                    b.Navigation("Potassium");
                });

            modelBuilder.Entity("AgriPen.Domain.Entities.PlantSeason", b =>
                {
                    b.Navigation("Plant");
                });
#pragma warning restore 612, 618
        }
    }
}
