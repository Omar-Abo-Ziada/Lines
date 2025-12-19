using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWalletRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Drivers_DriverId",
                schema: "Driver",
                table: "Wallets");

            migrationBuilder.EnsureSchema(
                name: "Finance");

            migrationBuilder.RenameTable(
                name: "Wallets",
                schema: "Driver",
                newName: "Wallets",
                newSchema: "Finance");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                schema: "Finance",
                table: "Wallets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_DriverId",
                schema: "Finance",
                table: "Wallets",
                newName: "IX_Wallets_UserId");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478), new DateTime(2026, 1, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478), new DateTime(2026, 1, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478), new DateTime(2026, 1, 2, 13, 53, 40, 475, DateTimeKind.Utc).AddTicks(5478) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 30, 13, 53, 40, 655, DateTimeKind.Utc).AddTicks(5779));

            migrationBuilder.AddCheckConstraint(
                name: "CK_ApplicationUser_DriverOrPassenger",
                schema: "User",
                table: "ApplicationUsers",
                sql: "(\r\n                    (\"DriverId\" IS NOT NULL AND \"PassengerId\" IS NULL) OR\r\n                    (\"DriverId\" IS NULL AND \"PassengerId\" IS NOT NULL) OR\r\n                    (\"DriverId\" IS NULL AND \"PassengerId\" IS NULL)\r\n                )");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_ApplicationUsers_UserId",
                schema: "Finance",
                table: "Wallets",
                column: "UserId",
                principalSchema: "User",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_ApplicationUsers_UserId",
                schema: "Finance",
                table: "Wallets");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ApplicationUser_DriverOrPassenger",
                schema: "User",
                table: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "Wallets",
                schema: "Finance",
                newName: "Wallets",
                newSchema: "Driver");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Driver",
                table: "Wallets",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_UserId",
                schema: "Driver",
                table: "Wallets",
                newName: "IX_Wallets_DriverId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Drivers_DriverId",
                schema: "Driver",
                table: "Wallets",
                column: "DriverId",
                principalSchema: "Driver",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
    