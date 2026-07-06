using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityDataPlatform.Migrations
{
    /// <inheritdoc />
    public partial class SecurePasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEInhFmTF3UaRUXnRJaTYjpu9BgJaha6efUVp1FYfqzIv+DFzFkXfDJJquhh85675Zg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "123");
        }
    }
}
