using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatusAndCurrencyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Trip",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DriverRating",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TripRating",
                schema: "Trip",
                table: "Trips",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Trip",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Trip",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ReferralCode",
                schema: "Passenger",
                table: "Passengers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Currency", "DriverRating", "TripRating" },
                values: new object[] { "QAR", null, null });

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Currency", "DriverRating", "TripRating" },
                values: new object[] { "QAR", null, null });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 24, 15, 56, 37, 203, DateTimeKind.Utc).AddTicks(1765));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 24, 15, 56, 37, 203, DateTimeKind.Utc).AddTicks(1770), new DateTime(2025, 10, 21, 15, 56, 37, 203, DateTimeKind.Utc).AddTicks(1771) });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Passengers_ReferralCode",
            //    schema: "Passenger",
            //    table: "Passengers",
            //    column: "ReferralCode",
            //    unique: true,
            //    filter: "[ReferralCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_Passengers_ReferralCode",
            //    schema: "Passenger",
            //    table: "Passengers");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DriverRating",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripRating",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Trip",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Trip",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "ReferralCode",
                schema: "Passenger",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 21, 14, 14, 33, 41, DateTimeKind.Utc).AddTicks(4219));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 21, 14, 14, 33, 41, DateTimeKind.Utc).AddTicks(4224), new DateTime(2025, 10, 18, 14, 14, 33, 41, DateTimeKind.Utc).AddTicks(4224) });
        }
    }
}
