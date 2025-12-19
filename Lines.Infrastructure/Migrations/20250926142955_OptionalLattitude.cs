using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OptionalLattitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Start_Longitude",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Start_Latitude",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Start_Longitude",
                schema: "Driver",
                table: "Drivers",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Start_Latitude",
                schema: "Driver",
                table: "Drivers",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Start_Longitude",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Start_Latitude",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Start_Longitude",
                schema: "Driver",
                table: "Drivers",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Start_Latitude",
                schema: "Driver",
                table: "Drivers",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedBy", "DriverLicenseId", "FirstName", "IsAvailable", "IsDeleted", "IsNotifiedForOnlyTripsAboveMyPrice", "LastName", "Rating", "TotalTrips", "UpdatedBy", "UpdatedDate", "VehicleId" },
                values: new object[] { null, new Guid("11111111-1111-1111-1111-111111111111"), "Mohamed", true, false, true, "Driver", 4.7999999999999998, 120, null, null, new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedBy", "DriverLicenseId", "FirstName", "IsAvailable", "IsDeleted", "IsNotifiedForOnlyTripsAboveMyPrice", "LastName", "Rating", "TotalTrips", "UpdatedBy", "UpdatedDate", "VehicleId" },
                values: new object[] { null, new Guid("22222222-2222-2222-2222-222222222222"), "Khaled", false, false, false, "Driver", 4.5, 75, null, null, new Guid("22222222-2222-2222-2222-222222222222") });
        }
    }
}
