using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_application_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                schema: "Passenger",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                schema: "User",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "StripeCustomerId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "StripeCustomerId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "StripeCustomerId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "StripeCustomerId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "StripeCustomerId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 6, 19, 22, 31, 171, DateTimeKind.Utc).AddTicks(9242), new DateTime(2026, 1, 6, 19, 22, 31, 171, DateTimeKind.Utc).AddTicks(9242) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 6, 19, 22, 31, 171, DateTimeKind.Utc).AddTicks(9242), new DateTime(2026, 1, 6, 19, 22, 31, 171, DateTimeKind.Utc).AddTicks(9242) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 6, 19, 22, 31, 171, DateTimeKind.Utc).AddTicks(9242), new DateTime(2026, 1, 6, 19, 22, 31, 171, DateTimeKind.Utc).AddTicks(9242) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 4, 19, 22, 31, 314, DateTimeKind.Utc).AddTicks(6923));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                schema: "User",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                schema: "Passenger",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995), new DateTime(2026, 1, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995), new DateTime(2026, 1, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995), new DateTime(2026, 1, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995) });

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "StripeCustomerId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "StripeCustomerId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 3, 16, 0, 8, 412, DateTimeKind.Utc).AddTicks(7413));
        }
    }
}
