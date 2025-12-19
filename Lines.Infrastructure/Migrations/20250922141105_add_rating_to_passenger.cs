using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_rating_to_passenger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RideCount",
                schema: "Passenger",
                table: "Passengers",
                newName: "TotalTrips");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                schema: "Passenger",
                table: "Passengers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Rating",
                value: 4.7999999999999998);

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Rating",
                value: 4.7999999999999998);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "Passenger",
                table: "Passengers");

            migrationBuilder.RenameColumn(
                name: "TotalTrips",
                schema: "Passenger",
                table: "Passengers",
                newName: "RideCount");
        }
    }
}
