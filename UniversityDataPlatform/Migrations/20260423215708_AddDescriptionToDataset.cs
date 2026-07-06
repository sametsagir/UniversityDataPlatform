using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityDataPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToDataset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Datasets");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Datasets",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Datasets",
                newName: "SubjectArea");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeatureType",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InstancesCount",
                table: "Datasets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "FeatureType",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "InstancesCount",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Datasets");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Datasets",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "SubjectArea",
                table: "Datasets",
                newName: "FileExtension");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Datasets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
