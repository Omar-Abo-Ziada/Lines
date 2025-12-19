using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class feedback_user_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_ApplicationUsers_FromUserId",
                schema: "Trip",
                table: "Feedbacks",
                column: "FromUserId",
                principalSchema: "User",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_ApplicationUsers_ToUserId",
                schema: "Trip",
                table: "Feedbacks",
                column: "ToUserId",
                principalSchema: "User",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_ApplicationUsers_FromUserId",
                schema: "Trip",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_ApplicationUsers_ToUserId",
                schema: "Trip",
                table: "Feedbacks");
        }
    }
}
