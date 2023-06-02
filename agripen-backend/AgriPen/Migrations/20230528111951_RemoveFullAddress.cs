using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriPen.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFullAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_address",
                table: "land_observations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_address",
                table: "land_observations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
