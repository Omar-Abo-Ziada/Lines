using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DriveEmailVerivication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondaryPhoneNumber",
                table: "DriverRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationCode",
                table: "DriverRegistrations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationExpiry",
                table: "DriverRegistrations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "DriverRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 19, 16, 29, 21, 261, DateTimeKind.Utc).AddTicks(1132));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 19, 16, 29, 21, 261, DateTimeKind.Utc).AddTicks(1138), new DateTime(2025, 10, 16, 16, 29, 21, 261, DateTimeKind.Utc).AddTicks(1138) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationCode",
                table: "DriverRegistrations");

            migrationBuilder.DropColumn(
                name: "EmailVerificationExpiry",
                table: "DriverRegistrations");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "DriverRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryPhoneNumber",
                table: "DriverRegistrations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

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
