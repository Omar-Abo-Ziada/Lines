using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverBlockedPassengers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverBlockedPassengers",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverBlockedPassengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverBlockedPassengers_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverBlockedPassengers_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalSchema: "Passenger",
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 31, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 12, 1, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 31, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 12, 1, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 31, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789), new DateTime(2025, 12, 1, 18, 52, 14, 75, DateTimeKind.Utc).AddTicks(8789) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 11, 1, 18, 52, 14, 397, DateTimeKind.Utc).AddTicks(5672));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 11, 1, 18, 52, 14, 397, DateTimeKind.Utc).AddTicks(5685), new DateTime(2025, 10, 29, 18, 52, 14, 397, DateTimeKind.Utc).AddTicks(5686) });

            migrationBuilder.CreateIndex(
                name: "IX_DriverBlockedPassengers_DriverId_PassengerId",
                schema: "Driver",
                table: "DriverBlockedPassengers",
                columns: new[] { "DriverId", "PassengerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverBlockedPassengers_PassengerId",
                schema: "Driver",
                table: "DriverBlockedPassengers",
                column: "PassengerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverBlockedPassengers",
                schema: "Driver");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798), new DateTime(2025, 11, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798), new DateTime(2025, 11, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 10, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798), new DateTime(2025, 11, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 29, 22, 56, 33, 273, DateTimeKind.Utc).AddTicks(5164));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 22, 56, 33, 273, DateTimeKind.Utc).AddTicks(5168), new DateTime(2025, 10, 26, 22, 56, 33, 273, DateTimeKind.Utc).AddTicks(5169) });
        }
    }
}
