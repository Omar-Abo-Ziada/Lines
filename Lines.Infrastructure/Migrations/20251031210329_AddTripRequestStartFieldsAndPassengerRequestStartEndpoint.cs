using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripRequestStartFieldsAndPassengerRequestStartEndpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DriverArrivedAt",
                schema: "Trip",
                table: "TripRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PassengerStartRequestedAt",
                schema: "Trip",
                table: "TripRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "TripRequests",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "DriverArrivedAt", "PassengerStartRequestedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "TripRequests",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "DriverArrivedAt", "PassengerStartRequestedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 31, 21, 3, 23, 931, DateTimeKind.Utc).AddTicks(4360));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 31, 21, 3, 23, 931, DateTimeKind.Utc).AddTicks(4366), new DateTime(2025, 10, 28, 21, 3, 23, 931, DateTimeKind.Utc).AddTicks(4366) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverArrivedAt",
                schema: "Trip",
                table: "TripRequests");

            migrationBuilder.DropColumn(
                name: "PassengerStartRequestedAt",
                schema: "Trip",
                table: "TripRequests");

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
