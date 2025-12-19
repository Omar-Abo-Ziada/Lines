using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTipsAndDriverServiceFeeOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Tips",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "DriverServiceFeeOffers",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceFeePercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverServiceFeeOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverServiceFeeOffers_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Tips",
                value: 0m);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Tips",
                value: 0m);

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
                name: "IX_DriverServiceFeeOffers_DriverId",
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                column: "DriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverServiceFeeOffers",
                schema: "Driver");

            migrationBuilder.DropColumn(
                name: "Tips",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 22, 19, 56, 2, 529, DateTimeKind.Utc).AddTicks(9954));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 22, 19, 56, 2, 529, DateTimeKind.Utc).AddTicks(9959), new DateTime(2025, 10, 19, 19, 56, 2, 529, DateTimeKind.Utc).AddTicks(9959) });
        }
    }
}
