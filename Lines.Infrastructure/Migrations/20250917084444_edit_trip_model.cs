using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_trip_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Trip",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "Trip",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "VehicleId",
                value: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "VehicleId",
                value: new Guid("22222222-2222-2222-2222-222222222222"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                schema: "Trip",
                table: "Trips",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "Trip",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Trip",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Feedbacks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Feedbacks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Rating",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Rating",
                value: 4);
        }
    }
}
