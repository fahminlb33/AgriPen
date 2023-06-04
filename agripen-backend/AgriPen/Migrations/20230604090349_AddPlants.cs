using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriPen.Migrations
{
    /// <inheritdoc />
    public partial class AddPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_disease_predictions_gps_addresses_gps_address_temp_id",
                table: "disease_predictions");

            migrationBuilder.DropForeignKey(
                name: "fk_disease_predictions_local_addresses_local_address_temp_id",
                table: "disease_predictions");

            migrationBuilder.DropForeignKey(
                name: "fk_land_observations_gps_addresses_gps_address_temp_id1",
                table: "land_observations");

            migrationBuilder.DropForeignKey(
                name: "fk_land_observations_local_addresses_local_address_temp_id1",
                table: "land_observations");

            migrationBuilder.DropForeignKey(
                name: "fk_telemetries_land_observations_observation_temp_id",
                table: "telemetries");

            migrationBuilder.DropForeignKey(
                name: "fk_weather_predictions_local_addresses_local_address_temp_id2",
                table: "weather_predictions");

            migrationBuilder.CreateTable(
                name: "plants",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name_id = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "plant_nitrogen",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nitrogen = table.Column<double>(type: "float", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    plant_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plant_nitrogen", x => x.id);
                    table.ForeignKey(
                        name: "fk_plant_nitrogen_plants_plant_id",
                        column: x => x.plant_id,
                        principalTable: "plants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plant_ph",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    optimal = table.Column<double>(type: "float", nullable: false),
                    minimum = table.Column<double>(type: "float", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    plant_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plant_ph", x => x.id);
                    table.ForeignKey(
                        name: "fk_plant_ph_plants_plant_id",
                        column: x => x.plant_id,
                        principalTable: "plants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plant_phosporus",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    category1 = table.Column<double>(type: "float", nullable: false),
                    category2 = table.Column<double>(type: "float", nullable: false),
                    category3 = table.Column<double>(type: "float", nullable: false),
                    category4 = table.Column<double>(type: "float", nullable: false),
                    category5 = table.Column<double>(type: "float", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    plant_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plant_phosporus", x => x.id);
                    table.ForeignKey(
                        name: "fk_plant_phosporus_plants_plant_id",
                        column: x => x.plant_id,
                        principalTable: "plants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plant_potassium",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    category1 = table.Column<double>(type: "float", nullable: false),
                    category2 = table.Column<double>(type: "float", nullable: false),
                    category3 = table.Column<double>(type: "float", nullable: false),
                    category4 = table.Column<double>(type: "float", nullable: false),
                    category5 = table.Column<double>(type: "float", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    plant_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plant_potassium", x => x.id);
                    table.ForeignKey(
                        name: "fk_plant_potassium_plants_plant_id",
                        column: x => x.plant_id,
                        principalTable: "plants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plant_season",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    season = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    temp_day_low = table.Column<double>(type: "float", nullable: false),
                    temp_day_high = table.Column<double>(type: "float", nullable: false),
                    temp_night_low = table.Column<double>(type: "float", nullable: false),
                    temp_night_high = table.Column<double>(type: "float", nullable: false),
                    humidity_low = table.Column<double>(type: "float", nullable: false),
                    humidity_high = table.Column<double>(type: "float", nullable: false),
                    soil_moisture_low = table.Column<double>(type: "float", nullable: false),
                    soil_moisture_high = table.Column<double>(type: "float", nullable: false),
                    plant_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plant_season", x => x.id);
                    table.ForeignKey(
                        name: "fk_plant_season_plants_plant_id",
                        column: x => x.plant_id,
                        principalTable: "plants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_plant_nitrogen_plant_id",
                table: "plant_nitrogen",
                column: "plant_id");

            migrationBuilder.CreateIndex(
                name: "ix_plant_ph_plant_id",
                table: "plant_ph",
                column: "plant_id");

            migrationBuilder.CreateIndex(
                name: "ix_plant_phosporus_plant_id",
                table: "plant_phosporus",
                column: "plant_id");

            migrationBuilder.CreateIndex(
                name: "ix_plant_potassium_plant_id",
                table: "plant_potassium",
                column: "plant_id");

            migrationBuilder.CreateIndex(
                name: "ix_plant_season_plant_id",
                table: "plant_season",
                column: "plant_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_disease_predictions_gps_addresses_gps_address_id",
                table: "disease_predictions",
                column: "gps_address_id",
                principalTable: "gps_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_disease_predictions_local_addresses_local_address_id",
                table: "disease_predictions",
                column: "local_address_id",
                principalTable: "local_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_land_observations_gps_addresses_gps_address_id",
                table: "land_observations",
                column: "gps_address_id",
                principalTable: "gps_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_land_observations_local_addresses_local_address_id",
                table: "land_observations",
                column: "local_address_id",
                principalTable: "local_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_telemetries_land_observations_observation_id",
                table: "telemetries",
                column: "observation_id",
                principalTable: "land_observations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_weather_predictions_local_addresses_local_address_id",
                table: "weather_predictions",
                column: "local_address_id",
                principalTable: "local_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_disease_predictions_gps_addresses_gps_address_id",
                table: "disease_predictions");

            migrationBuilder.DropForeignKey(
                name: "fk_disease_predictions_local_addresses_local_address_id",
                table: "disease_predictions");

            migrationBuilder.DropForeignKey(
                name: "fk_land_observations_gps_addresses_gps_address_id",
                table: "land_observations");

            migrationBuilder.DropForeignKey(
                name: "fk_land_observations_local_addresses_local_address_id",
                table: "land_observations");

            migrationBuilder.DropForeignKey(
                name: "fk_telemetries_land_observations_observation_id",
                table: "telemetries");

            migrationBuilder.DropForeignKey(
                name: "fk_weather_predictions_local_addresses_local_address_id",
                table: "weather_predictions");

            migrationBuilder.DropTable(
                name: "plant_nitrogen");

            migrationBuilder.DropTable(
                name: "plant_ph");

            migrationBuilder.DropTable(
                name: "plant_phosporus");

            migrationBuilder.DropTable(
                name: "plant_potassium");

            migrationBuilder.DropTable(
                name: "plant_season");

            migrationBuilder.DropTable(
                name: "plants");

            migrationBuilder.AddForeignKey(
                name: "fk_disease_predictions_gps_addresses_gps_address_temp_id",
                table: "disease_predictions",
                column: "gps_address_id",
                principalTable: "gps_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_disease_predictions_local_addresses_local_address_temp_id",
                table: "disease_predictions",
                column: "local_address_id",
                principalTable: "local_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_land_observations_gps_addresses_gps_address_temp_id1",
                table: "land_observations",
                column: "gps_address_id",
                principalTable: "gps_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_land_observations_local_addresses_local_address_temp_id1",
                table: "land_observations",
                column: "local_address_id",
                principalTable: "local_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_telemetries_land_observations_observation_temp_id",
                table: "telemetries",
                column: "observation_id",
                principalTable: "land_observations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_weather_predictions_local_addresses_local_address_temp_id2",
                table: "weather_predictions",
                column: "local_address_id",
                principalTable: "local_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
