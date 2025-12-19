using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_app_user_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                schema: "User",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                schema: "Passenger",
                table: "Passengers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                schema: "Driver",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Points",
                value: 100);

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Points",
                value: 60);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 20, 7, 33, 4, 888, DateTimeKind.Utc).AddTicks(2624));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 20, 7, 33, 4, 888, DateTimeKind.Utc).AddTicks(2634), new DateTime(2025, 10, 17, 7, 33, 4, 888, DateTimeKind.Utc).AddTicks(2634) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                schema: "Passenger",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "Points",
                schema: "Driver",
                table: "Drivers");

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
                value: new DateTime(2025, 10, 18, 21, 3, 38, 232, DateTimeKind.Utc).AddTicks(1845));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 18, 21, 3, 38, 232, DateTimeKind.Utc).AddTicks(1850), new DateTime(2025, 10, 15, 21, 3, 38, 232, DateTimeKind.Utc).AddTicks(1850) });
        }
    }
}
