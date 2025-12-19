using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_activity_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "User",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "StartTime",
                schema: "User",
                table: "Activities");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Day",
                schema: "User",
                table: "Activities",
                type: "date",
                nullable: false,
                defaultValueSql: "CAST(GETDATE() AS DATE)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                schema: "User",
                table: "Activities");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                schema: "User",
                table: "Activities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                schema: "User",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }
    }
}
