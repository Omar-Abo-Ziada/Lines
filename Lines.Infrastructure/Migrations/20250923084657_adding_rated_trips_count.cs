using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adding_rated_trips_count : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatedTripsCount",
                schema: "Passenger",
                table: "Passengers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RatedTripsCount",
                schema: "Driver",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RatedTripsCount",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "RatedTripsCount",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "RatedTripsCount",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "RatedTripsCount",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatedTripsCount",
                schema: "Passenger",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "RatedTripsCount",
                schema: "Driver",
                table: "Drivers");
        }
    }
}
