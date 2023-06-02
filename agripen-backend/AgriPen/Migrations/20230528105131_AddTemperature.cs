using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriPen.Migrations
{
    /// <inheritdoc />
    public partial class AddTemperature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "temperature",
                table: "weather_predictions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "temperature",
                table: "weather_predictions");
        }
    }
}
