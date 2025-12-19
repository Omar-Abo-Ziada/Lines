using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_models_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRewardApplied",
                schema: "Trip",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsRewardApplied",
                schema: "Trip",
                table: "TripRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RewardId",
                schema: "Trip",
                table: "TripRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 5, 21, 57, 9, 549, DateTimeKind.Utc).AddTicks(4548), new DateTime(2025, 12, 6, 21, 57, 9, 549, DateTimeKind.Utc).AddTicks(4548) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 5, 21, 57, 9, 549, DateTimeKind.Utc).AddTicks(4548), new DateTime(2025, 12, 6, 21, 57, 9, 549, DateTimeKind.Utc).AddTicks(4548) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 5, 21, 57, 9, 549, DateTimeKind.Utc).AddTicks(4548), new DateTime(2025, 12, 6, 21, 57, 9, 549, DateTimeKind.Utc).AddTicks(4548) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "DiscountPercentage",
                value: 0.15m);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "DiscountPercentage",
                value: 1.00m);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "TripRequests",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RewardId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "TripRequests",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "RewardId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 3, 21, 57, 9, 706, DateTimeKind.Utc).AddTicks(5300));

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_RewardId",
                schema: "Trip",
                table: "TripRequests",
                column: "RewardId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripRequests_Rewards_RewardId",
                schema: "Trip",
                table: "TripRequests",
                column: "RewardId",
                principalSchema: "User",
                principalTable: "Rewards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_Rewards_RewardId",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.DropIndex(
                name: "IX_TripRequests_RewardId",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "IsRewardApplied",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "RewardId",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRewardApplied",
                schema: "Trip",
                table: "Trips",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 12, 20, 43, 46, 363, DateTimeKind.Utc).AddTicks(5021), new DateTime(2025, 12, 13, 20, 43, 46, 363, DateTimeKind.Utc).AddTicks(5021) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 12, 20, 43, 46, 363, DateTimeKind.Utc).AddTicks(5021), new DateTime(2025, 12, 13, 20, 43, 46, 363, DateTimeKind.Utc).AddTicks(5021) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 12, 20, 43, 46, 363, DateTimeKind.Utc).AddTicks(5021), new DateTime(2025, 12, 13, 20, 43, 46, 363, DateTimeKind.Utc).AddTicks(5021) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "DiscountPercentage",
                value: 15.00m);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Rewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "DiscountPercentage",
                value: 100.00m);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 10, 20, 43, 46, 532, DateTimeKind.Utc).AddTicks(5530));
        }
    }
}
