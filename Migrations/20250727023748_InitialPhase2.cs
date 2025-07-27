using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project_Equinox.Migrations
{
    /// <inheritdoc />
    public partial class InitialPhase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassCategories",
                columns: table => new
                {
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassCategories", x => x.ClassCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCoach = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Equinoxclasses",
                columns: table => new
                {
                    EquinoxClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ClassPicture = table.Column<string>(type: "TEXT", nullable: false),
                    ClassDay = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<string>(type: "TEXT", nullable: false),
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoachId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equinoxclasses", x => x.EquinoxClassId);
                    table.ForeignKey(
                        name: "FK_Equinoxclasses_ClassCategories_ClassCategoryId",
                        column: x => x.ClassCategoryId,
                        principalTable: "ClassCategories",
                        principalColumn: "ClassCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equinoxclasses_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equinoxclasses_Users_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquinoxClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Equinoxclasses_EquinoxClassId",
                        column: x => x.EquinoxClassId,
                        principalTable: "Equinoxclasses",
                        principalColumn: "EquinoxClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClassCategories",
                columns: new[] { "ClassCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Boxing" },
                    { 2, "Yoga" },
                    { 3, "HIIT" },
                    { 4, "Strength" },
                    { 5, "Barre" },
                    { 6, "Sculpt" },
                    { 7, "Dancing" },
                    { 8, "Running" },
                    { 9, "Palate" }
                });

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "ClubId", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Chicago Loop", "331-567-7657" },
                    { 2, "West Chicago", "331-678-3456" },
                    { 3, "Lincoln Park", "431-658-3256" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "IsCoach", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(1985, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "alex@equinox.com", true, "Alex Smith", "312-111-2222" },
                    { 2, new DateTime(1990, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria@equinox.com", true, "Maria Garcia", "312-111-3333" },
                    { 3, new DateTime(1992, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "priya@equinox.com", true, "Priya Patel", "312-111-4444" },
                    { 4, new DateTime(1988, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@equinox.com", true, "John Lee", "312-111-5555" }
                });

            migrationBuilder.InsertData(
                table: "Equinoxclasses",
                columns: new[] { "EquinoxClassId", "ClassCategoryId", "ClassDay", "ClassPicture", "ClubId", "CoachId", "Name", "Time" },
                values: new object[,]
                {
                    { 1, 1, "Monday", "boxing101.jpg", 1, 1, "Boxing 101", "8 AM – 9 AM" },
                    { 2, 2, "Tuesday", "yogaflow.jpg", 2, 2, "Yoga Flow", "6 PM – 7 PM" },
                    { 3, 3, "Wednesday", "hiitblast.jpg", 3, 3, "HIIT Blast", "5 PM – 6 PM" },
                    { 4, 4, "Thursday", "strengthtraining.jpg", 1, 4, "Strength Training", "7 AM – 8 AM" },
                    { 5, 5, "Friday", "barrebasic.jpg", 2, 1, "Barre Basics", "9 AM – 10 AM" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EquinoxClassId",
                table: "Bookings",
                column: "EquinoxClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Equinoxclasses_ClassCategoryId",
                table: "Equinoxclasses",
                column: "ClassCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Equinoxclasses_ClubId",
                table: "Equinoxclasses",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Equinoxclasses_CoachId",
                table: "Equinoxclasses",
                column: "CoachId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Equinoxclasses");

            migrationBuilder.DropTable(
                name: "ClassCategories");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
