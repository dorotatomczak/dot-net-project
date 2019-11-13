using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClinic.Migrations
{
    public partial class UpdatedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "visits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhysicianId",
                table: "visits",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "visits",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "visits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fullname = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    illnessHistory = table.Column<string>(nullable: true),
                    recommendedDrugs = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "physicians",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fullname = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    Specialization = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_physicians", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_visits_PatientId",
                table: "visits",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_visits_PhysicianId",
                table: "visits",
                column: "PhysicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_visits_patients_PatientId",
                table: "visits",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_visits_physicians_PhysicianId",
                table: "visits",
                column: "PhysicianId",
                principalTable: "physicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visits_patients_PatientId",
                table: "visits");

            migrationBuilder.DropForeignKey(
                name: "FK_visits_physicians_PhysicianId",
                table: "visits");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "physicians");

            migrationBuilder.DropIndex(
                name: "IX_visits_PatientId",
                table: "visits");

            migrationBuilder.DropIndex(
                name: "IX_visits_PhysicianId",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "PhysicianId",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "visits");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "visits");
        }
    }
}
