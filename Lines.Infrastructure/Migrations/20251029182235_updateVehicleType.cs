using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateVehicleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageSpeedKmPerHour",
                schema: "Vehicle",
                table: "VehicleTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 29, 18, 22, 30, 587, DateTimeKind.Utc).AddTicks(5698));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 18, 22, 30, 587, DateTimeKind.Utc).AddTicks(5707), new DateTime(2025, 10, 26, 18, 22, 30, 587, DateTimeKind.Utc).AddTicks(5708) });

            migrationBuilder.UpdateData(
                schema: "Vehicle",
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "AverageSpeedKmPerHour",
                value: 0m);

            migrationBuilder.UpdateData(
                schema: "Vehicle",
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "AverageSpeedKmPerHour",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageSpeedKmPerHour",
                schema: "Vehicle",
                table: "VehicleTypes");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 22, 19, 56, 2, 529, DateTimeKind.Utc).AddTicks(9954));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 22, 19, 56, 2, 529, DateTimeKind.Utc).AddTicks(9959), new DateTime(2025, 10, 19, 19, 56, 2, 529, DateTimeKind.Utc).AddTicks(9959) });
        }
    }
}
