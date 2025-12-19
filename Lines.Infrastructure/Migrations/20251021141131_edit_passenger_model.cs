using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_passenger_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferralCode",
                schema: "Passenger",
                table: "Passengers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "ReferralCode",
                value: "REF12345");

            migrationBuilder.UpdateData(
                schema: "Passenger",
                table: "Passengers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "ReferralCode",
                value: "REF67890");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 21, 14, 11, 28, 155, DateTimeKind.Utc).AddTicks(5686));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 21, 14, 11, 28, 155, DateTimeKind.Utc).AddTicks(5692), new DateTime(2025, 10, 18, 14, 11, 28, 155, DateTimeKind.Utc).AddTicks(5692) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferralCode",
                schema: "Passenger",
                table: "Passengers");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "AddressCompleted", "BankAccountId", "CommercialRegistration", "CompanyName", "ContactInfoCompleted", "CreatedBy", "DateOfBirth", "FirstName", "IdentityType", "IsAvailable", "IsDeleted", "IsNotifiedForOnlyTripsAboveMyPrice", "LastName", "LicenseCompleted", "PersonalInfoCompleted", "PersonalPictureUrl", "Points", "RatedTripsCount", "Rating", "RegistrationStatus", "SecondaryPhoneNumber", "TotalTrips", "UpdatedBy", "UpdatedDate", "VehicleCompleted", "WithdrawalCompleted" },
                values: new object[] { false, null, null, null, false, null, null, "Mohamed", null, true, false, true, "Driver", false, false, null, 50, 2, 4.7999999999999998, 0, null, 120, null, null, false, false });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "Drivers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "AddressCompleted", "BankAccountId", "CommercialRegistration", "CompanyName", "ContactInfoCompleted", "CreatedBy", "DateOfBirth", "FirstName", "IdentityType", "IsAvailable", "IsDeleted", "IsNotifiedForOnlyTripsAboveMyPrice", "LastName", "LicenseCompleted", "PersonalInfoCompleted", "PersonalPictureUrl", "Points", "RatedTripsCount", "Rating", "RegistrationStatus", "SecondaryPhoneNumber", "TotalTrips", "UpdatedBy", "UpdatedDate", "VehicleCompleted", "WithdrawalCompleted" },
                values: new object[] { false, null, null, null, false, null, null, "Khaled", null, false, false, false, "Driver", false, false, null, 60, 3, 4.5, 0, null, 75, null, null, false, false });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "RedeemedAt",
                value: new DateTime(2025, 10, 20, 7, 33, 4, 888, DateTimeKind.Utc).AddTicks(2624));

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "RedeemedAt", "UsedAt" },
                values: new object[] { new DateTime(2025, 10, 20, 7, 33, 4, 888, DateTimeKind.Utc).AddTicks(2634), new DateTime(2025, 10, 17, 7, 33, 4, 888, DateTimeKind.Utc).AddTicks(2634) });
        }
    }
}
