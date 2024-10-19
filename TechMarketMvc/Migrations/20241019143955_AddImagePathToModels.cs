using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechMarketMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Smartwatches",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Phones",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Computers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Smartwatches");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Computers");
        }
    }
}
