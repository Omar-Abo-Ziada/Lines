using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantRatingFieldsFromTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverRating",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripRating",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 1, 20, 56, 34, 145, DateTimeKind.Utc).AddTicks(5605), new DateTime(2025, 12, 2, 20, 56, 34, 145, DateTimeKind.Utc).AddTicks(5605) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 1, 20, 56, 34, 145, DateTimeKind.Utc).AddTicks(5605), new DateTime(2025, 12, 2, 20, 56, 34, 145, DateTimeKind.Utc).AddTicks(5605) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 1, 20, 56, 34, 145, DateTimeKind.Utc).AddTicks(5605), new DateTime(2025, 12, 2, 20, 56, 34, 145, DateTimeKind.Utc).AddTicks(5605) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 11, 2, 20, 56, 34, 223, DateTimeKind.Utc).AddTicks(3851));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 11, 2, 20, 56, 34, 223, DateTimeKind.Utc).AddTicks(3855), new DateTime(2025, 10, 30, 20, 56, 34, 223, DateTimeKind.Utc).AddTicks(3855) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DriverRating",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TripRating",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 31, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 12, 1, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 31, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 12, 1, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 31, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 12, 1, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "DriverRating", "TripRating" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "DriverRating", "TripRating" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 11, 1, 18, 52, 14, 397, DateTimeKind.Utc).AddTicks(5672));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 11, 1, 18, 52, 14, 397, DateTimeKind.Utc).AddTicks(5685), new DateTime(2025, 10, 29, 18, 52, 14, 397, DateTimeKind.Utc).AddTicks(5686) });
        }
    }
}
