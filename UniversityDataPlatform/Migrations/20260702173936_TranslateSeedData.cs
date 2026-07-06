using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityDataPlatform.Migrations
{
    /// <inheritdoc />
    public partial class TranslateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Faculty of Engineering");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Faculty of Medicine");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Safranbolu Faculty of Architecture");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Faculty of Theology");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Faculty of Letters");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Faculty of Science");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Faculty of Business Administration");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Faculty of Forestry");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Safranbolu Faculty of Tourism");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Safranbolu Faculty of Fine Arts and Design");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Safranbolu Faculty of Communication");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Faculty of Health Sciences");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "Faculty of Technology");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Hasan Doğan Faculty of Sports Sciences");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Mühendislik Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Tıp Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Safranbolu Mimarlık Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "İlahiyat Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Edebiyat Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Fen Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "İşletme Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Orman Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Safranbolu Turizm Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Safranbolu Güzel Sanatlar ve Tasarım Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Safranbolu İletişim Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Sağlık Bilimleri Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "Teknoloji Fakültesi");

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Hasan Doğan Spor Bilimleri Fakültesi");
        }
    }
}
