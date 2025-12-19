using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class refactor_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "IsRewardApplied",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 21, 15, 1, 16, 815, DateTimeKind.Utc).AddTicks(1146), new DateTime(2025, 12, 22, 15, 1, 16, 815, DateTimeKind.Utc).AddTicks(1146) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 21, 15, 1, 16, 815, DateTimeKind.Utc).AddTicks(1146), new DateTime(2025, 12, 22, 15, 1, 16, 815, DateTimeKind.Utc).AddTicks(1146) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 21, 15, 1, 16, 815, DateTimeKind.Utc).AddTicks(1146), new DateTime(2025, 12, 22, 15, 1, 16, 815, DateTimeKind.Utc).AddTicks(1146) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 19, 15, 1, 16, 957, DateTimeKind.Utc).AddTicks(650));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                schema: "Trip",
                table: "TripRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRewardApplied",
                schema: "Trip",
                table: "TripRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 18, 21, 31, 59, 124, DateTimeKind.Utc).AddTicks(2695), new DateTime(2025, 12, 19, 21, 31, 59, 124, DateTimeKind.Utc).AddTicks(2695) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 18, 21, 31, 59, 124, DateTimeKind.Utc).AddTicks(2695), new DateTime(2025, 12, 19, 21, 31, 59, 124, DateTimeKind.Utc).AddTicks(2695) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 18, 21, 31, 59, 124, DateTimeKind.Utc).AddTicks(2695), new DateTime(2025, 12, 19, 21, 31, 59, 124, DateTimeKind.Utc).AddTicks(2695) });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "TripRequests",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "IsAnonymous", "IsRewardApplied" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "TripRequests",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "IsAnonymous", "IsRewardApplied" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 16, 21, 31, 59, 274, DateTimeKind.Utc).AddTicks(1228));
        }
    }
}
