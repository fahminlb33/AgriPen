using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriPen.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gps_addresses",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    geocoded_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    altitude = table.Column<double>(type: "float", nullable: false),
                    horizontal_accuracy = table.Column<double>(type: "float", nullable: false),
                    vertical_accuracy = table.Column<double>(type: "float", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gps_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "local_addresses",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    code = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    kecamatan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    kabupaten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    provinsi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_local_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    hashed_password = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    last_login_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "disease_predictions",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    severity = table.Column<double>(type: "float", nullable: false),
                    result = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    probability_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    gps_address_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    local_address_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_disease_predictions", x => x.id);
                    table.ForeignKey(
                        name: "fk_disease_predictions_gps_addresses_gps_address_temp_id",
                        column: x => x.gps_address_id,
                        principalTable: "gps_addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_disease_predictions_local_addresses_local_address_temp_id",
                        column: x => x.local_address_id,
                        principalTable: "local_addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "land_observations",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    full_address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    gps_address_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    local_address_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_land_observations", x => x.id);
                    table.ForeignKey(
                        name: "fk_land_observations_gps_addresses_gps_address_temp_id1",
                        column: x => x.gps_address_id,
                        principalTable: "gps_addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_land_observations_local_addresses_local_address_temp_id1",
                        column: x => x.local_address_id,
                        principalTable: "local_addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "weather_predictions",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    temperature_low = table.Column<double>(type: "float", nullable: false),
                    temperature_high = table.Column<double>(type: "float", nullable: false),
                    humidity_low = table.Column<double>(type: "float", nullable: false),
                    humidity_high = table.Column<double>(type: "float", nullable: false),
                    humidity = table.Column<double>(type: "float", nullable: false),
                    weather = table.Column<int>(type: "int", nullable: false),
                    wind = table.Column<int>(type: "int", nullable: false),
                    wind_speed = table.Column<double>(type: "float", nullable: false),
                    local_address_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_weather_predictions", x => x.id);
                    table.ForeignKey(
                        name: "fk_weather_predictions_local_addresses_local_address_temp_id2",
                        column: x => x.local_address_id,
                        principalTable: "local_addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "disease_probability",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    bacterial_blight = table.Column<double>(type: "float", nullable: false),
                    blast = table.Column<double>(type: "float", nullable: false),
                    brown_spot = table.Column<double>(type: "float", nullable: false),
                    tungro = table.Column<double>(type: "float", nullable: false),
                    healthy = table.Column<double>(type: "float", nullable: false),
                    prediction_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_disease_probability", x => x.id);
                    table.ForeignKey(
                        name: "fk_disease_probability_disease_predictions_prediction_id",
                        column: x => x.prediction_id,
                        principalTable: "disease_predictions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "telemetries",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    air_temperature = table.Column<double>(type: "float", nullable: false),
                    air_humidity = table.Column<double>(type: "float", nullable: false),
                    soil_moisture = table.Column<double>(type: "float", nullable: false),
                    soil_temperature = table.Column<double>(type: "float", nullable: false),
                    sun_illumination = table.Column<double>(type: "float", nullable: false),
                    observation_id = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_telemetries", x => x.id);
                    table.ForeignKey(
                        name: "fk_telemetries_land_observations_observation_temp_id",
                        column: x => x.observation_id,
                        principalTable: "land_observations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_disease_predictions_gps_address_id",
                table: "disease_predictions",
                column: "gps_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_disease_predictions_local_address_id",
                table: "disease_predictions",
                column: "local_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_disease_probability_prediction_id",
                table: "disease_probability",
                column: "prediction_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_land_observations_gps_address_id",
                table: "land_observations",
                column: "gps_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_land_observations_local_address_id",
                table: "land_observations",
                column: "local_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_telemetries_observation_id",
                table: "telemetries",
                column: "observation_id");

            migrationBuilder.CreateIndex(
                name: "ix_weather_predictions_local_address_id",
                table: "weather_predictions",
                column: "local_address_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "disease_probability");

            migrationBuilder.DropTable(
                name: "telemetries");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "weather_predictions");

            migrationBuilder.DropTable(
                name: "disease_predictions");

            migrationBuilder.DropTable(
                name: "land_observations");

            migrationBuilder.DropTable(
                name: "gps_addresses");

            migrationBuilder.DropTable(
                name: "local_addresses");
        }
    }
}
