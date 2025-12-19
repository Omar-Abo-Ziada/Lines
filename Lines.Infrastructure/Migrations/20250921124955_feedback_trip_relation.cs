using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class feedback_trip_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Feedbacks_FeedbackId",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_FeedbackId",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                schema: "Trip",
                table: "Trips");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TripId",
                schema: "Trip",
                table: "Feedbacks",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Trips_TripId",
                schema: "Trip",
                table: "Feedbacks",
                column: "TripId",
                principalSchema: "Trip",
                principalTable: "Trips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Trips_TripId",
                schema: "Trip",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_TripId",
                schema: "Trip",
                table: "Feedbacks");

            migrationBuilder.AddColumn<Guid>(
                name: "FeedbackId",
                schema: "Trip",
                table: "Trips",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "FeedbackId",
                value: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.UpdateData(
                schema: "Trip",
                table: "Trips",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "FeedbackId",
                value: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.CreateIndex(
                name: "IX_Trips_FeedbackId",
                schema: "Trip",
                table: "Trips",
                column: "FeedbackId",
                unique: true,
                filter: "[FeedbackId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Feedbacks_FeedbackId",
                schema: "Trip",
                table: "Trips",
                column: "FeedbackId",
                principalSchema: "Trip",
                principalTable: "Feedbacks",
                principalColumn: "Id");
        }
    }
}
