using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_application_user_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                schema: "User",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "ApplicationUsers",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "Points",
                value: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                schema: "User",
                table: "ApplicationUsers");

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
    }
}
