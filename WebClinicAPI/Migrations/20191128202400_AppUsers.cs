using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClinicAPI.Migrations
{
    public partial class AppUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    IllnessHistory = table.Column<string>(nullable: true),
                    RecommendedDrugs = table.Column<string>(nullable: true),
                    Specialization = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(nullable: true),
                    PhysicianId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_AppUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_AppUsers_PhysicianId",
                        column: x => x.PhysicianId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "DateOfBirth", "Discriminator", "Email", "FirstName", "LastName", "Password", "Role", "Sex", "IllnessHistory", "RecommendedDrugs" },
                values: new object[] { 6, new DateTime(1979, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patient", "patient1@gmail.com", "Geralt", "Z Rivii", "10/w7o2juYBrGMh32/KbveULW9jk2tejpyUAD+uC6PE=", "Patient", 0, null, null });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "DateOfBirth", "Discriminator", "Email", "FirstName", "LastName", "Password", "Role", "Sex", "Specialization" },
                values: new object[,]
                {
                    { 1, new DateTime(1970, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Physician", "physician1@gmail.com", "Nathan", "Drake", "10/w7o2juYBrGMh32/KbveULW9jk2tejpyUAD+uC6PE=", "Physician", 0, 5 },
                    { 2, new DateTime(1975, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Physician", "physician2@gmail.com", "Elena", "Fisher", "10/w7o2juYBrGMh32/KbveULW9jk2tejpyUAD+uC6PE=", "Physician", 1, 4 },
                    { 3, new DateTime(1967, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Physician", "physician3@gmail.com", "Victor", "Sullivan", "10/w7o2juYBrGMh32/KbveULW9jk2tejpyUAD+uC6PE=", "Physician", 0, 8 }
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "DateOfBirth", "Discriminator", "Email", "FirstName", "LastName", "Password", "Role", "Sex" },
                values: new object[,]
                {
                    { 4, new DateTime(1990, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Receptionist", "receptionist1@gmail.com", "Rajesh", "Koothrappali", "10/w7o2juYBrGMh32/KbveULW9jk2tejpyUAD+uC6PE=", "Receptionist", 0 },
                    { 5, new DateTime(1986, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Receptionist", "receptionist2@gmail.com", "Penny", "Hofstadter", "10/w7o2juYBrGMh32/KbveULW9jk2tejpyUAD+uC6PE=", "Receptionist", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PhysicianId",
                table: "Appointments",
                column: "PhysicianId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
