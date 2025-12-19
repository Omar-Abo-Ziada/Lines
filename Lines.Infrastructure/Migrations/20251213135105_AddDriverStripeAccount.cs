using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverStripeAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Payments");

            migrationBuilder.CreateTable(
                name: "DriverStripeAccounts",
                schema: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StripeAccountId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ChargesEnabled = table.Column<bool>(type: "bit", nullable: false),
                    PayoutsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DetailsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverStripeAccounts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 12, 13, 50, 58, 980, DateTimeKind.Utc).AddTicks(2989), new DateTime(2026, 1, 12, 13, 50, 58, 980, DateTimeKind.Utc).AddTicks(2989) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 12, 13, 50, 58, 980, DateTimeKind.Utc).AddTicks(2989), new DateTime(2026, 1, 12, 13, 50, 58, 980, DateTimeKind.Utc).AddTicks(2989) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 12, 13, 50, 58, 980, DateTimeKind.Utc).AddTicks(2989), new DateTime(2026, 1, 12, 13, 50, 58, 980, DateTimeKind.Utc).AddTicks(2989) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 10, 13, 50, 59, 229, DateTimeKind.Utc).AddTicks(7478));

            migrationBuilder.CreateIndex(
                name: "IX_DriverStripeAccounts_DriverId",
                schema: "Payments",
                table: "DriverStripeAccounts",
                column: "DriverId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverStripeAccounts",
                schema: "Payments");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 11, 7, 0, 10, 816, DateTimeKind.Utc).AddTicks(252), new DateTime(2026, 1, 11, 7, 0, 10, 816, DateTimeKind.Utc).AddTicks(252) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 11, 7, 0, 10, 816, DateTimeKind.Utc).AddTicks(252), new DateTime(2026, 1, 11, 7, 0, 10, 816, DateTimeKind.Utc).AddTicks(252) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 11, 7, 0, 10, 816, DateTimeKind.Utc).AddTicks(252), new DateTime(2026, 1, 11, 7, 0, 10, 816, DateTimeKind.Utc).AddTicks(252) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 9, 7, 0, 11, 24, DateTimeKind.Utc).AddTicks(5286));
        }
    }
}
