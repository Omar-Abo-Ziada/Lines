using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorToDatabaseBasedDriverRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddressCompleted",
                schema: "Driver",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountId",
                schema: "Driver",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialRegistration",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ContactInfoCompleted",
                schema: "Driver",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "Driver",
                table: "Drivers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdentityType",
                schema: "Driver",
                table: "Drivers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LicenseCompleted",
                schema: "Driver",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PersonalInfoCompleted",
                schema: "Driver",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPictureUrl",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegistrationStatus",
                schema: "Driver",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryPhoneNumber",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "Driver",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VehicleCompleted",
                schema: "Driver",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WithdrawalCompleted",
                schema: "Driver",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    SWIFT = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    AccountHolderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationToken = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PersonalPictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CommercialRegistration = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdentityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SecondaryPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LicenseData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAccountData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPersonalInfoCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsContactInfoCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsAddressCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsLicenseCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsVehicleCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsWithdrawalInfoCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverRegistrations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_DriverId",
                table: "BankAccounts",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverRegistrations_CreatedDate",
                table: "DriverRegistrations",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_DriverRegistrations_Email",
                table: "DriverRegistrations",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_DriverRegistrations_RegistrationToken",
                table: "DriverRegistrations",
                column: "RegistrationToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverRegistrations_Status",
                table: "DriverRegistrations",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "DriverRegistrations");

            migrationBuilder.DropColumn(
                name: "AddressCompleted",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CommercialRegistration",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ContactInfoCompleted",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IdentityType",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseCompleted",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PersonalInfoCompleted",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PersonalPictureUrl",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Region",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "RegistrationStatus",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "SecondaryPhoneNumber",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Street",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "VehicleCompleted",
                schema: "Driver",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "WithdrawalCompleted",
                schema: "Driver",
                table: "Drivers");
        }
    }
}
