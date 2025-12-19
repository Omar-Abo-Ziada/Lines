using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_user_rewards_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRewards_Rewards_RewardId1",
                schema: "User",
                table: "UserRewards");

            migrationBuilder.DropIndex(
                name: "IX_UserRewards_RewardId1",
                schema: "User",
                table: "UserRewards");

            migrationBuilder.DropColumn(
                name: "RewardId1",
                schema: "User",
                table: "UserRewards");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 11, 18, 55, 49, 682, DateTimeKind.Utc).AddTicks(6672));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 11, 18, 55, 49, 682, DateTimeKind.Utc).AddTicks(6677), new DateTime(2025, 10, 8, 18, 55, 49, 682, DateTimeKind.Utc).AddTicks(6678) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RewardId1",
                schema: "User",
                table: "UserRewards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "RedeemedAt", "RewardId1" },
                values: new object[] { new DateTime(2025, 10, 11, 18, 49, 35, 184, DateTimeKind.Utc).AddTicks(1886), null });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "RewardId1", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 11, 18, 49, 35, 184, DateTimeKind.Utc).AddTicks(1894), null, new DateTime(2025, 10, 8, 18, 49, 35, 184, DateTimeKind.Utc).AddTicks(1895) });

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_RewardId1",
                schema: "User",
                table: "UserRewards",
                column: "RewardId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRewards_Rewards_RewardId1",
                schema: "User",
                table: "UserRewards",
                column: "RewardId1",
                principalSchema: "User",
                principalTable: "Rewards",
                principalColumn: "Id");
        }
    }
}
