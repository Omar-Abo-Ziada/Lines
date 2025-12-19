using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class data_seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "User",
                table: "RewardActions",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "Name", "Points", "Type", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), null, false, "Leave Tips for the driver", 5, 4, null, null });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 18, 21, 3, 38, 232, DateTimeKind.Utc).AddTicks(1845));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 18, 21, 3, 38, 232, DateTimeKind.Utc).AddTicks(1850), new DateTime(2025, 10, 15, 21, 3, 38, 232, DateTimeKind.Utc).AddTicks(1850) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "User",
                table: "RewardActions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 15, 14, 25, 12, 739, DateTimeKind.Utc).AddTicks(8232));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 15, 14, 25, 12, 739, DateTimeKind.Utc).AddTicks(8239), new DateTime(2025, 10, 12, 14, 25, 12, 739, DateTimeKind.Utc).AddTicks(8240) });
        }
    }
}
