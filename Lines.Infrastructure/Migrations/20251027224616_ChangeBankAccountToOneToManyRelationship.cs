using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBankAccountToOneToManyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_DriverId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 27, 22, 46, 11, 920, DateTimeKind.Utc).AddTicks(9489));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 27, 22, 46, 11, 920, DateTimeKind.Utc).AddTicks(9501), new DateTime(2025, 10, 24, 22, 46, 11, 920, DateTimeKind.Utc).AddTicks(9501) });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_DriverId",
                table: "BankAccounts",
                column: "DriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_DriverId",
                table: "BankAccounts");

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountId",
                schema: "Driver",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 27, 11, 55, 41, 602, DateTimeKind.Utc).AddTicks(48));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 27, 11, 55, 41, 602, DateTimeKind.Utc).AddTicks(54), new DateTime(2025, 10, 24, 11, 55, 41, 602, DateTimeKind.Utc).AddTicks(54) });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_DriverId",
                table: "BankAccounts",
                column: "DriverId",
                unique: true);
        }
    }
}
