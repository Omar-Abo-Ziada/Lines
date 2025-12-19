using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripCode",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 19, 16, 10, 50, 810, DateTimeKind.Utc).AddTicks(1197), new DateTime(2025, 12, 20, 16, 10, 50, 810, DateTimeKind.Utc).AddTicks(1197) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 19, 16, 10, 50, 810, DateTimeKind.Utc).AddTicks(1197), new DateTime(2025, 12, 20, 16, 10, 50, 810, DateTimeKind.Utc).AddTicks(1197) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 19, 16, 10, 50, 810, DateTimeKind.Utc).AddTicks(1197), new DateTime(2025, 12, 20, 16, 10, 50, 810, DateTimeKind.Utc).AddTicks(1197) });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "TripCode",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "TripCode",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 17, 16, 10, 50, 944, DateTimeKind.Utc).AddTicks(773));

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripCode",
                schema: "Trip",
                table: "Trips",
                column: "TripCode",
                unique: true,
                filter: "[TripCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_TripCode",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripCode",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637), new DateTime(2025, 12, 15, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637), new DateTime(2025, 12, 15, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637), new DateTime(2025, 12, 15, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 12, 22, 30, 26, 497, DateTimeKind.Utc).AddTicks(232));
        }
    }
}
