using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactorDriverEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Drivers_DriverId",
                schema: "Common",
                table: "Licenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                schema: "Vehicle",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_DriverId",
                schema: "Vehicle",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_DriverId",
                schema: "Common",
                table: "Licenses");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_VehicleId",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DriverLicenseId",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "kmPrice",
                schema: "Vehicle",
                table: "Vehicles",
                newName: "KmPrice");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Vehicle",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                schema: "Vehicle",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationDocumentUrls",
                schema: "Vehicle",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Common",
                table: "Licenses",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                schema: "Common",
                table: "Licenses",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "IsActive", "IsCurrent" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                schema: "Common",
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "IsActive", "IsCurrent" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                schema: "Vehicle",
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "IsActive", "IsPrimary", "RegistrationDocumentUrls" },
                values: new object[] { true, true, null });

            migrationBuilder.UpdateData(
                schema: "Vehicle",
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "IsActive", "RegistrationDocumentUrls" },
                values: new object[] { true, null });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                schema: "Vehicle",
                table: "Vehicles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_DriverId",
                schema: "Common",
                table: "Licenses",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Drivers_DriverId",
                schema: "Common",
                table: "Licenses",
                column: "DriverId",
                principalSchema: "Driver",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                schema: "Vehicle",
                table: "Vehicles",
                column: "DriverId",
                principalSchema: "Driver",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Drivers_DriverId",
                schema: "Common",
                table: "Licenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                schema: "Vehicle",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_DriverId",
                schema: "Vehicle",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_DriverId",
                schema: "Common",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Vehicle",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                schema: "Vehicle",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RegistrationDocumentUrls",
                schema: "Vehicle",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Common",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                schema: "Common",
                table: "Licenses");

            migrationBuilder.RenameColumn(
                name: "KmPrice",
                schema: "Vehicle",
                table: "Vehicles",
                newName: "kmPrice");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverLicenseId",
                schema: "Driver",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleId",
                schema: "Driver",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                schema: "Vehicle",
                table: "Vehicles",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_DriverId",
                schema: "Common",
                table: "Licenses",
                column: "DriverId",
                unique: true,
                filter: "[DriverId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_VehicleId",
                schema: "Driver",
                table: "Drivers",
                column: "VehicleId",
                unique: true,
                filter: "[VehicleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Drivers_DriverId",
                schema: "Common",
                table: "Licenses",
                column: "DriverId",
                principalSchema: "Driver",
                principalTable: "Drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                schema: "Vehicle",
                table: "Vehicles",
                column: "DriverId",
                principalSchema: "Driver",
                principalTable: "Drivers",
                principalColumn: "Id");
        }
    }
}
