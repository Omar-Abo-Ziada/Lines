using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_payment_method_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                schema: "PaymentMethod",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "ExpirationMonth",
                schema: "PaymentMethod",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "ExpirationYear",
                schema: "PaymentMethod",
                table: "PaymentMethods");

            migrationBuilder.RenameColumn(
                name: "Token",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                newName: "PaymentMethodId");

            migrationBuilder.RenameColumn(
                name: "Details",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                newName: "CustomerId");

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995), new DateTime(2026, 1, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995), new DateTime(2026, 1, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995), new DateTime(2026, 1, 5, 16, 0, 8, 260, DateTimeKind.Utc).AddTicks(3995) });

            migrationBuilder.UpdateData(
                schema: "PaymentMethod",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CustomerId", "PaymentMethodId" },
                values: new object[] { "cus_test_12345", "pm_test_67890" });

            migrationBuilder.UpdateData(
                schema: "PaymentMethod",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "CustomerId", "PaymentMethodId" },
                values: new object[] { "cus_test_67895", "pm_test_12345" });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 3, 16, 0, 8, 412, DateTimeKind.Utc).AddTicks(7413));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethodId",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                newName: "Details");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExpirationMonth",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpirationYear",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607), new DateTime(2026, 1, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607), new DateTime(2026, 1, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607) });

            migrationBuilder.UpdateData(
                schema: "Driver",
                table: "DriverServiceFeeOffers",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "ValidFrom", "ValidUntil" },
                values: new object[] { new DateTime(2025, 12, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607), new DateTime(2026, 1, 5, 12, 56, 23, 295, DateTimeKind.Utc).AddTicks(6607) });

            migrationBuilder.UpdateData(
                schema: "PaymentMethod",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Brand", "Details", "ExpirationMonth", "ExpirationYear", "Token" },
                values: new object[] { "Visa", "**** **** **** 4242", 12, 2026, "tok-test-visa-4242" });

            migrationBuilder.UpdateData(
                schema: "PaymentMethod",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Brand", "Details", "ExpirationMonth", "ExpirationYear", "Token" },
                values: new object[] { "Mastercard", "**** **** **** 5555", 11, 2027, "tok-test-mastercard-5555" });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "UserRewards",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "UsedAt",
                value: new DateTime(2025, 12, 3, 12, 56, 23, 448, DateTimeKind.Utc).AddTicks(1060));
        }
    }
}
