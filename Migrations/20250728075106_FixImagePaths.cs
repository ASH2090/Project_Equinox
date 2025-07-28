using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Equinox.Migrations
{
    /// <inheritdoc />
    public partial class FixImagePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 1,
                column: "ClassPicture",
                value: "/images/boxing101.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 2,
                column: "ClassPicture",
                value: "/images/yogaflow.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 3,
                column: "ClassPicture",
                value: "/images/hiitblast.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 4,
                column: "ClassPicture",
                value: "/images/strengthtraining.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 5,
                column: "ClassPicture",
                value: "/images/barrebasic.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 1,
                column: "ClassPicture",
                value: "boxing101.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 2,
                column: "ClassPicture",
                value: "yogaflow.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 3,
                column: "ClassPicture",
                value: "hiitblast.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 4,
                column: "ClassPicture",
                value: "strengthtraining.jpg");

            migrationBuilder.UpdateData(
                table: "Equinoxclasses",
                keyColumn: "EquinoxClassId",
                keyValue: 5,
                column: "ClassPicture",
                value: "barrebasic.jpg");
        }
    }
}
