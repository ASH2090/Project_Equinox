using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Equinox.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToClassCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ClassCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 1,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 2,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 3,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 4,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 5,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 6,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 7,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 8,
                column: "Image",
                value: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 9,
                column: "Image",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ClassCategories");
        }
    }
}
