using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rewards_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RewardActions",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PointsRequired = table.Column<int>(type: "int", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    MaxValue = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRewards",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RewardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RedeemedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RewardId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRewards_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRewards_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalSchema: "User",
                        principalTable: "Rewards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRewards_Rewards_RewardId1",
                        column: x => x.RewardId1,
                        principalSchema: "User",
                        principalTable: "Rewards",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "RewardActions",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "Name", "Points", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, false, "Complete 5 Trips", 50, null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, false, "Invite a Friend", 100, null, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, false, "Complete Profile", 15, null, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, false, "Visit app daily for one week", 40, null, null }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "Rewards",
                columns: new[] { "Id", "CreatedBy", "Description", "DiscountAmount", "IsDeleted", "MaxValue", "PointsRequired", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Redeem 100 points to get 15% off your next trip.", 15.00m, false, 12.00m, 100, "15% Discount on Next Trip", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Redeem 300 points for a free ride up to 15 EGP.", 100.00m, false, 15.00m, 300, "Free Ride up to 50 EGP", null, null }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "UserRewards",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "RedeemedAt", "RewardId", "RewardId1", "UpdatedBy", "UpdatedDate", "UsedAt", "UserId" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), null, false, new DateTime(2025, 10, 11, 18, 49, 35, 184, DateTimeKind.Utc).AddTicks(1886), new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, null, new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                schema: "User",
                table: "UserRewards",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "IsUsed", "RedeemedAt", "RewardId", "RewardId1", "UpdatedBy", "UpdatedDate", "UsedAt", "UserId" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), null, false, true, new DateTime(2025, 10, 11, 18, 49, 35, 184, DateTimeKind.Utc).AddTicks(1894), new Guid("22222222-2222-2222-2222-222222222222"), null, null, null, new DateTime(2025, 10, 8, 18, 49, 35, 184, DateTimeKind.Utc).AddTicks(1895), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_RewardId",
                schema: "User",
                table: "UserRewards",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_RewardId1",
                schema: "User",
                table: "UserRewards",
                column: "RewardId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_UserId",
                schema: "User",
                table: "UserRewards",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardActions",
                schema: "User");

            migrationBuilder.DropTable(
                name: "UserRewards",
                schema: "User");

            migrationBuilder.DropTable(
                name: "Rewards",
                schema: "User");
        }
    }
}
