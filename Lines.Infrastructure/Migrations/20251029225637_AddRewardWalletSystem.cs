using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRewardWalletSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsGloballyActive",
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DriverOfferActivations",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverOfferActivations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverOfferActivations_DriverServiceFeeOffers_OfferId",
                        column: x => x.OfferId,
                        principalSchema: "Driver",
                        principalTable: "DriverServiceFeeOffers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DriverOfferActivations_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransactionCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "Driver",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                columns: new[] { "Id", "CreatedBy", "Description", "DriverId", "DurationDays", "IsDeleted", "IsGloballyActive", "Price", "ServiceFeePercent", "Title", "UpdatedBy", "UpdatedDate", "ValidFrom", "ValidUntil" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), null, "Don't miss out — activate now and maximize every trip!", new Guid("11111111-1111-1111-1111-111111111111"), 3, false, true, 5.00m, 7.0m, "Service Fee Cap: 7%", null, null, new DateTime(2025, 10, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798), new DateTime(2025, 11, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798) },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, "Premium offer! Reduce service fee to only 5% for a week.", new Guid("11111111-1111-1111-1111-111111111111"), 7, false, true, 10.00m, 5.0m, "Service Fee Cap: 5%", null, null, new DateTime(2025, 10, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798), new DateTime(2025, 11, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798) },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), null, "Best offer! Maximum earnings with only 3% service fee for 2 weeks.", new Guid("11111111-1111-1111-1111-111111111111"), 14, false, true, 20.00m, 3.0m, "Service Fee Cap: 3%", null, null, new DateTime(2025, 10, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798), new DateTime(2025, 11, 28, 22, 56, 33, 168, DateTimeKind.Utc).AddTicks(4798) }
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 29, 22, 56, 33, 273, DateTimeKind.Utc).AddTicks(5164));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 29, 22, 56, 33, 273, DateTimeKind.Utc).AddTicks(5168), new DateTime(2025, 10, 26, 22, 56, 33, 273, DateTimeKind.Utc).AddTicks(5169) });

            migrationBuilder.CreateIndex(
                name: "IX_DriverOfferActivations_DriverId_IsActive_ExpirationDate",
                schema: "Driver",
                table: "DriverOfferActivations",
                columns: new[] { "DriverId", "IsActive", "ExpirationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DriverOfferActivations_OfferId",
                schema: "Driver",
                table: "DriverOfferActivations",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_DriverId",
                schema: "Driver",
                table: "Wallets",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_Reference",
                schema: "Driver",
                table: "WalletTransactions",
                column: "Reference");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId_CreatedDate",
                schema: "Driver",
                table: "WalletTransactions",
                columns: new[] { "WalletId", "CreatedDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverOfferActivations",
                schema: "Driver");

            migrationBuilder.DropTable(
                name: "WalletTransactions",
                schema: "Driver");

            migrationBuilder.DropTable(
                name: "Wallets",
                schema: "Driver");

            migrationBuilder.DeleteData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Driver",
                table: "DriverServiceFeeOffers");

            migrationBuilder.DropColumn(
                name: "DurationDays",
                schema: "Driver",
                table: "DriverServiceFeeOffers");

            migrationBuilder.DropColumn(
                name: "IsGloballyActive",
                schema: "Driver",
                table: "DriverServiceFeeOffers");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Driver",
                table: "DriverServiceFeeOffers");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Driver",
                table: "DriverServiceFeeOffers");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 27, 22, 18, 18, 150, DateTimeKind.Utc).AddTicks(2906));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 27, 22, 18, 18, 150, DateTimeKind.Utc).AddTicks(2914), new DateTime(2025, 10, 24, 22, 18, 18, 150, DateTimeKind.Utc).AddTicks(2914) });
        }
    }
}
