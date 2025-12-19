using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFireBasceFCMAndRefactorTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverArrivedAt",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "PassengerStartRequestedAt",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.EnsureSchema(
                name: "Notify");

            migrationBuilder.CreateTable(
                name: "FCMUserTokens",
                schema: "Notify",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 128, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    Locale = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSeenUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCMUserTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637), new DateTime(2025, 12, 15, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637), new DateTime(2025, 12, 15, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 14, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637), new DateTime(2025, 12, 15, 22, 30, 26, 301, DateTimeKind.Utc).AddTicks(7637) });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 12, 22, 30, 26, 497, DateTimeKind.Utc).AddTicks(232));

            migrationBuilder.CreateIndex(
                name: "IX_FCMUserTokens_Token",
                schema: "Notify",
                table: "FCMUserTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FCMUserTokens_UserId_Platform",
                schema: "Notify",
                table: "FCMUserTokens",
                columns: new[] { "UserId", "Platform" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FCMUserTokens",
                schema: "Notify");

            migrationBuilder.AddColumn<DateTime>(
                name: "DriverArrivedAt",
                schema: "Trip",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PassengerStartRequestedAt",
                schema: "Trip",
                table: "Trips",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 12, 12, 5, 11, 6, DateTimeKind.Utc).AddTicks(319), new DateTime(2025, 12, 13, 12, 5, 11, 6, DateTimeKind.Utc).AddTicks(319) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 12, 12, 5, 11, 6, DateTimeKind.Utc).AddTicks(319), new DateTime(2025, 12, 13, 12, 5, 11, 6, DateTimeKind.Utc).AddTicks(319) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 11, 12, 12, 5, 11, 6, DateTimeKind.Utc).AddTicks(319), new DateTime(2025, 12, 13, 12, 5, 11, 6, DateTimeKind.Utc).AddTicks(319) });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "DriverArrivedAt", "PassengerStartRequestedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "DriverArrivedAt", "PassengerStartRequestedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 11, 10, 12, 5, 11, 190, DateTimeKind.Utc).AddTicks(3828));
        }
    }
}
