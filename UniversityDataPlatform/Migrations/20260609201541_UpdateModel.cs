using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityDataPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoiOrUrl",
                table: "DatasetPapers",
                newName: "Url");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "DatasetPapers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "DatasetPapers",
                newName: "DoiOrUrl");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "DatasetPapers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
