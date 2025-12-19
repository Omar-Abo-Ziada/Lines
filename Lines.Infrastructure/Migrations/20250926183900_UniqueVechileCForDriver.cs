using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UniqueVechileCForDriver : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "DriverLicenseId",
            schema: "Driver",
            table: "Drivers",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");


        migrationBuilder.CreateIndex(
            name: "IX_Drivers_VehicleId",
            schema: "Driver",
            table: "Drivers",
            column: "VehicleId",
            unique: true,
            filter: "[VehicleId] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Drivers_VehicleId",
            schema: "Driver",
            table: "Drivers");

        migrationBuilder.AlterColumn<Guid>(
            name: "DriverLicenseId",
            schema: "Driver",
            table: "Drivers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.UpdateData(
            schema: "Driver",
            table: "Drivers",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            columns: new[] { "CreatedBy", "DriverLicenseId", "FirstName", "IsAvailable", "IsDeleted", "IsNotifiedForOnlyTripsAboveMyPrice", "LastName", "RatedTripsCount", "Rating", "TotalTrips", "UpdatedBy", "UpdatedDate", "VehicleId" },
            values: new object[] { null, new Guid("11111111-1111-1111-1111-111111111111"), "Mohamed", true, false, true, "Driver", 0, 4.7999999999999998, 120, null, null, new Guid("11111111-1111-1111-1111-111111111111") });

        migrationBuilder.UpdateData(
            schema: "Driver",
            table: "Drivers",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
            columns: new[] { "CreatedBy", "DriverLicenseId", "FirstName", "IsAvailable", "IsDeleted", "IsNotifiedForOnlyTripsAboveMyPrice", "LastName", "RatedTripsCount", "Rating", "TotalTrips", "UpdatedBy", "UpdatedDate", "VehicleId" },
            values: new object[] { null, new Guid("22222222-2222-2222-2222-222222222222"), "Khaled", false, false, false, "Driver", 1, 4.5, 75, null, null, new Guid("22222222-2222-2222-2222-222222222222") });

        migrationBuilder.UpdateData(
            schema: "Vehicle",
            table: "Vehicles",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            column: "DriverId",
            value: new Guid("11111111-1111-1111-1111-111111111111"));

        migrationBuilder.UpdateData(
            schema: "Vehicle",
            table: "Vehicles",
            keyColumn: "Id",
            keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
            column: "DriverId",
            value: new Guid("22222222-2222-2222-2222-222222222222"));
    }
}
