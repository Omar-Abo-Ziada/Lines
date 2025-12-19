using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class trip_request_user_reward_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripRequests_Rewards_RewardId",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.DropIndex(
                name: "IX_TripRequests_RewardId",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.RenameColumn(
                name: "RewardId",
                schema: "Trip",
                table: "TripRequests",
                newName: "UserRewardId");

            migrationBuilder.AddColumn<Guid>(
                name: "TripRequestId",
                schema: "User",
                table: "UserRewards",
                type: "uniqueidentifier",
                nullable: true);

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
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "TripRequestId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "TripRequestId", "UsedAt" },
                values: new object[] { null, new DateTime(2025, 11, 16, 21, 31, 59, 274, DateTimeKind.Utc).AddTicks(1228) });

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_TripRequestId",
                schema: "User",
                table: "UserRewards",
                column: "TripRequestId",
                unique: true,
                filter: "[TripRequestId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRewards_TripRequests_TripRequestId",
                schema: "User",
                table: "UserRewards",
                column: "TripRequestId",
                principalSchema: "Trip",
                principalTable: "TripRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRewards_TripRequests_TripRequestId",
                schema: "User",
                table: "UserRewards");

            migrationBuilder.DropIndex(
                name: "IX_UserRewards_TripRequestId",
                schema: "User",
                table: "UserRewards");

            migrationBuilder.DropColumn(
                name: "TripRequestId",
                schema: "User",
                table: "UserRewards");

            migrationBuilder.RenameColumn(
                name: "UserRewardId",
                schema: "Trip",
                table: "TripRequests",
                newName: "RewardId");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 8, 45, 63, DateTimeKind.Utc).AddTicks(1238), new DateTime(2025, 12, 15, 22, 8, 45, 63, DateTimeKind.Utc).AddTicks(1238) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 8, 45, 63, DateTimeKind.Utc).AddTicks(1238), new DateTime(2025, 12, 15, 22, 8, 45, 63, DateTimeKind.Utc).AddTicks(1238) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 8, 45, 63, DateTimeKind.Utc).AddTicks(1238), new DateTime(2025, 12, 15, 22, 8, 45, 63, DateTimeKind.Utc).AddTicks(1238) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 12, 22, 8, 45, 238, DateTimeKind.Utc).AddTicks(297));

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
    }
}
