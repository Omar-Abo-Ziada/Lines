using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntegrateWithStripe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeChargeId",
                schema: "Trip",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                schema: "Trip",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TripId",
                schema: "Driver",
                table: "Earnings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 26, 19, 35, 5, 50, DateTimeKind.Utc).AddTicks(2752), new DateTime(2025, 12, 27, 19, 35, 5, 50, DateTimeKind.Utc).AddTicks(2752) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 26, 19, 35, 5, 50, DateTimeKind.Utc).AddTicks(2752), new DateTime(2025, 12, 27, 19, 35, 5, 50, DateTimeKind.Utc).AddTicks(2752) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 26, 19, 35, 5, 50, DateTimeKind.Utc).AddTicks(2752), new DateTime(2025, 12, 27, 19, 35, 5, 50, DateTimeKind.Utc).AddTicks(2752) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Earnings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "TripId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Earnings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "TripId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 24, 19, 35, 5, 216, DateTimeKind.Utc).AddTicks(4992));

            migrationBuilder.CreateIndex(
                name: "IX_Earnings_TripId",
                schema: "Driver",
                table: "Earnings",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Earnings_Trips_TripId",
                schema: "Driver",
                table: "Earnings",
                column: "TripId",
                principalSchema: "Trip",
                principalTable: "Trips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Earnings_Trips_TripId",
                schema: "Driver",
                table: "Earnings");

            migrationBuilder.DropIndex(
                name: "IX_Earnings_TripId",
                schema: "Driver",
                table: "Earnings");

            migrationBuilder.DropColumn(
                name: "StripeChargeId",
                schema: "Trip",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                schema: "Trip",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TripId",
                schema: "Driver",
                table: "Earnings");

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
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 17, 16, 10, 50, 944, DateTimeKind.Utc).AddTicks(773));
        }
    }
}
