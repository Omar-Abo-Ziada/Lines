using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_StripeCustomerId_to_passenger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                schema: "Passenger",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 11, 47, 9, 363, DateTimeKind.Utc).AddTicks(5321), new DateTime(2026, 1, 5, 11, 47, 9, 363, DateTimeKind.Utc).AddTicks(5321) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 11, 47, 9, 363, DateTimeKind.Utc).AddTicks(5321), new DateTime(2026, 1, 5, 11, 47, 9, 363, DateTimeKind.Utc).AddTicks(5321) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 11, 47, 9, 363, DateTimeKind.Utc).AddTicks(5321), new DateTime(2026, 1, 5, 11, 47, 9, 363, DateTimeKind.Utc).AddTicks(5321) });

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
                value: new DateTime(2025, 12, 3, 11, 47, 9, 490, DateTimeKind.Utc).AddTicks(8204));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                schema: "Passenger",
                table: "Passengers");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478), new DateTime(2026, 1, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478), new DateTime(2026, 1, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478), new DateTime(2026, 1, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 30, 13, 53, 40, 655, DateTimeKind.Utc).AddTicks(5779));
        }
    }
}
