using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_StripeCustomerId_to_driver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[] { new DateTime(2025, 12, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607), new DateTime(2026, 1, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607), new DateTime(2026, 1, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607), new DateTime(2026, 1, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 3, 12, 56, 23, 448, DateTimeKind.Utc).AddTicks(1060));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                schema: "Driver",
                table: "Drivers");

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
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 3, 11, 47, 9, 490, DateTimeKind.Utc).AddTicks(8204));
        }
    }
}
