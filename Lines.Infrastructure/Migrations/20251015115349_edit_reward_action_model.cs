using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_reward_action_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "User",
                table: "RewardActions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "RewardActions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Type",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "RewardActions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Type",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "RewardActions",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Type",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "RewardActions",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Type",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 15, 11, 53, 45, 293, DateTimeKind.Utc).AddTicks(17));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 11, 53, 45, 293, DateTimeKind.Utc).AddTicks(26), new DateTime(2025, 10, 12, 11, 53, 45, 293, DateTimeKind.Utc).AddTicks(26) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "User",
                table: "RewardActions");

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
    }
}
