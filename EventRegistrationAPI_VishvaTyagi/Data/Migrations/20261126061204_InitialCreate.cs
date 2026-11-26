using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventRegistrationAPI_VishvaTyagi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    RegisteredOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.RegistrationId);
                    table.ForeignKey(
                        name: "FK_Registrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Capacity", "EventDate", "Location", "Title" },
                values: new object[,]
                {
                    { 1, 80, new DateTime(2026, 1, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), "Student Centre Atrium", "Winter Welcome Social" },
                    { 2, 150, new DateTime(2027, 3, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), "Hazel McCallion Campus Gym", "Spring Career Fair" },
                    { 3, 2, new DateTime(2027, 2, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), "Library Room 204", "Resume Writing Workshop" },
                    { 4, 40, new DateTime(2027, 4, 17, 9, 0, 0, 0, DateTimeKind.Unspecified), "SCAET Lab 3", "Campus Hackathon 2027" }
                });

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "RegistrationId", "Email", "EventId", "RegisteredOn", "StudentName" },
                values: new object[,]
                {
                    { 1, "alex.chen@sheridancollege.ca", 1, new DateTime(2026, 1, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), "Alex Chen" },
                    { 2, "priya.shah@sheridancollege.ca", 2, new DateTime(2026, 11, 1, 9, 30, 0, 0, DateTimeKind.Unspecified), "Priya Shah" },
                    { 3, "jordan.lee@sheridancollege.ca", 3, new DateTime(2026, 11, 10, 15, 0, 0, 0, DateTimeKind.Unspecified), "Jordan Lee" },
                    { 4, "sam.patel@sheridancollege.ca", 3, new DateTime(2026, 11, 12, 11, 15, 0, 0, DateTimeKind.Unspecified), "Sam Patel" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId",
                table: "Registrations",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
