using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedeemedAt",
                schema: "User",
                table: "UserRewards");

            migrationBuilder.RenameColumn(
                name: "IsAnonymous",
                schema: "Trip",
                table: "Trips",
                newName: "IsRewardApplied");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                schema: "User",
                table: "Rewards",
                newName: "DiscountPercentage");

            migrationBuilder.AddColumn<decimal>(
                name: "FareAfterRewardApplied",
                schema: "Trip",
                table: "Trips",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FareAfterRewardApplied",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "IsRewardApplied",
                schema: "Trip",
                table: "Trips",
                newName: "IsAnonymous");

            migrationBuilder.RenameColumn(
                name: "DiscountPercentage",
                schema: "User",
                table: "Rewards",
                newName: "DiscountAmount");

            migrationBuilder.AddColumn<DateTime>(
                name: "RedeemedAt",
                schema: "User",
                table: "UserRewards",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }
    }
}
