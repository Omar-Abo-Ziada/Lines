using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDriverAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Region",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "DriverRegistrations");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "DriverRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "DriverRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LimousineBadgeUrl",
                table: "DriverRegistrations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SectorId",
                table: "DriverRegistrations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DriverAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LimousineBadgeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverAddresses_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Sites",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverAddresses_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverAddresses_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalSchema: "Sites",
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverAddresses_CityId",
                table: "DriverAddresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverAddresses_DriverId",
                table: "DriverAddresses",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverAddresses_DriverId_IsPrimary",
                table: "DriverAddresses",
                columns: new[] { "DriverId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_DriverAddresses_SectorId",
                table: "DriverAddresses",
                column: "SectorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverAddresses");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "DriverRegistrations");

            migrationBuilder.DropColumn(
                name: "LimousineBadgeUrl",
                table: "DriverRegistrations");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "DriverRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "DriverRegistrations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "DriverRegistrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
