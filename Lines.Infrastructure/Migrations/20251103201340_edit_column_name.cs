using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_column_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "consecutiveActiveDays",
                schema: "Passenger",
                table: "Passengers",
                newName: "ConsecutiveActiveDays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConsecutiveActiveDays",
                schema: "Passenger",
                table: "Passengers",
                newName: "consecutiveActiveDays");
        }
    }
}
