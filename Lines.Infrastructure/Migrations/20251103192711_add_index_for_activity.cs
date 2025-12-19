using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_index_for_activity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Activities_UserId",
                schema: "User",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "UX_Activities_UserId_Day",
                schema: "User",
                table: "Activities",
                columns: new[] { "UserId", "Day" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Activities_UserId_Day",
                schema: "User",
                table: "Activities");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                schema: "User",
                table: "Activities",
                column: "UserId");
        }
    }
}
