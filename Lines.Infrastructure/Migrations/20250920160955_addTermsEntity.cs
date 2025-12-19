using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTermsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Config");

            migrationBuilder.CreateTable(
                name: "TermsAndConditions",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Header = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TermsType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndConditions", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Config",
                table: "TermsAndConditions",
                columns: new[] { "Id", "Content", "CreatedBy", "Header", "IsDeleted", "Order", "TermsType", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "This is the privacy policy content 1.", null, "Privacy Policy 1", false, 1, 0, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "This is the terms of use content 1.", null, "Terms of Use 1", false, 1, 1, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "These are the service rules content 1.", null, "Service Rules 1", false, 1, 2, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "This is the privacy policy content 2.", null, "Privacy Policy 2", false, 2, 0, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "This is the terms of use content 2.", null, "Terms of Use 2", false, 2, 1, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "These are the service rules content 2.", null, "Service Rules 2", false, 2, 2, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "This is the privacy policy content 3.", null, "Privacy Policy 3", false, 3, 0, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "This is the terms of use content 3.", null, "Terms of Use 3", false, 3, 1, null, null },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "These are the service rules content 3.", null, "Service Rules 3", false, 3, 2, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermsAndConditions",
                schema: "Config");
        }
    }
}
