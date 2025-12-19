using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "Sites");

            migrationBuilder.EnsureSchema(
                name: "Vehicle");

            migrationBuilder.EnsureSchema(
                name: "Driver");

            migrationBuilder.EnsureSchema(
                name: "Trip");

            migrationBuilder.EnsureSchema(
                name: "License");

            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.EnsureSchema(
                name: "Chat");

            migrationBuilder.EnsureSchema(
                name: "Passenger");

            migrationBuilder.EnsureSchema(
                name: "PaymentMethod");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Start_Latitude = table.Column<double>(type: "float", nullable: false),
                    Start_Longitude = table.Column<double>(type: "float", nullable: false),
                    Start_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    IsNotifiedForOnlyTripsAboveMyPrice = table.Column<bool>(type: "bit", nullable: false),
                    TotalTrips = table.Column<int>(type: "int", nullable: false),
                    DriverLicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Examples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                schema: "Trip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                schema: "Passenger",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RideCount = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                schema: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    PerKmCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerMinuteDelayCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                schema: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectors_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Sites",
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Earnings",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Earnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Earnings_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalSchema: "Passenger",
                        principalTable: "Passengers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CityVehicleTypes",
                schema: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityVehicleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityVehicleTypes_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Sites",
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CityVehicleTypes_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "Vehicle",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                schema: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    kmPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "Vehicle",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteLocations",
                schema: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteLocations_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Sites",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteLocations_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalSchema: "Passenger",
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteLocations_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalSchema: "Sites",
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyNumbers",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmergencyNumberType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyNumbers_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Otp",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    OTPGenerationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Otp_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentMethodType = table.Column<int>(type: "int", nullable: false),
                    ExpirationYear = table.Column<int>(type: "int", nullable: false),
                    ExpirationMonth = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    licenseType = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenses_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Licenses_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "Vehicle",
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehiclePhotos",
                schema: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclePhotos_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "Vehicle",
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "Trip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    TransactionReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EarningId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Earnings_EarningId",
                        column: x => x.EarningId,
                        principalSchema: "Driver",
                        principalTable: "Earnings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "PaymentMethod",
                        principalTable: "PaymentMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TripRequests",
                schema: "Trip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsScheduled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PassengerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start_Latitude = table.Column<double>(type: "float", nullable: false),
                    Start_Longitude = table.Column<double>(type: "float", nullable: false),
                    Start_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripRequests_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TripRequests_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalSchema: "Passenger",
                        principalTable: "Passengers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TripRequests_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "PaymentMethod",
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripRequests_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalSchema: "Vehicle",
                        principalTable: "VehicleTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LicensePhotos",
                schema: "License",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicensePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicensePhotos_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalSchema: "Common",
                        principalTable: "Licenses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                schema: "Driver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeToArriveInMinutes = table.Column<int>(type: "int", nullable: false),
                    DistanceToArriveInMeters = table.Column<float>(type: "real", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TripRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_TripRequests_TripRequestId",
                        column: x => x.TripRequestId,
                        principalSchema: "Trip",
                        principalTable: "TripRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                schema: "Trip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EstimatedPickupTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start_Latitude = table.Column<double>(type: "float", nullable: false),
                    Start_Longitude = table.Column<double>(type: "float", nullable: false),
                    Start_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethodType = table.Column<int>(type: "int", nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TripRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Driver",
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalSchema: "Trip",
                        principalTable: "Feedbacks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalSchema: "Passenger",
                        principalTable: "Passengers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "PaymentMethod",
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "Trip",
                        principalTable: "Payments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trips_TripRequests_TripRequestId",
                        column: x => x.TripRequestId,
                        principalSchema: "Trip",
                        principalTable: "TripRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EndTripLocations",
                schema: "Trip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TripRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start_Latitude = table.Column<double>(type: "float", nullable: false),
                    Start_Longitude = table.Column<double>(type: "float", nullable: false),
                    Start_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndTripLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndTripLocations_TripRequests_TripRequestId",
                        column: x => x.TripRequestId,
                        principalSchema: "Trip",
                        principalTable: "TripRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EndTripLocations_Trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "Trip",
                        principalTable: "Trips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2024, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified)),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_ApplicationUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_ApplicationUsers_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "User",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "Trip",
                        principalTable: "Trips",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "ApplicationUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DriverId", "Email", "EmailConfirmed", "IsActive", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PassengerId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), 0, "Static-Concurrency-Stamp-Value", null, "support.user@example.com", true, true, false, false, null, "SUPPORT.USER@EXAMPLE.COM", "SUPPORT.USER", null, "AQAAAAEAACcQAAAAE...", "01234567892", false, "STATIC-SECURITY-STAMP-VALUE", false, "Support.user" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Driver", "DRIVER" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Passenger", "PASSENGER" }
                });

            migrationBuilder.InsertData(
                schema: "Sites",
                table: "Cities",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "Latitude", "Longitude", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, false, 0.0, 0.0, "Zurich", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, false, 0.0, 0.0, "Geneva", null, null }
                });

            migrationBuilder.InsertData(
                schema: "Driver",
                table: "Drivers",
                columns: new[] { "Id", "CreatedBy", "DriverLicenseId", "FirstName", "IsAvailable", "IsDeleted", "IsNotifiedForOnlyTripsAboveMyPrice", "LastName", "Rating", "TotalTrips", "UpdatedBy", "UpdatedDate", "VehicleId", "Email", "Start_Address", "Start_Latitude", "Start_Longitude", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, new Guid("11111111-1111-1111-1111-111111111111"), "Mohamed", true, false, true, "Driver", 4.7999999999999998, 120, null, null, null, "mohamed.driver@example.com", "Start Location 1", 1.1000000000000001, 1.1000000000000001, "01234567890" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, new Guid("22222222-2222-2222-2222-222222222222"), "Khaled", false, false, false, "Driver", 4.5, 75, null, null, null, "khaled.driver@example.com", "Start Location 2", 2.2000000000000002, 2.2000000000000002, "01234567891" }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "EmergencyNumbers",
                columns: new[] { "Id", "CreatedBy", "EmergencyNumberType", "IsDeleted", "Name", "PhoneNumber", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, 1, false, "Police", "117", null, null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, 1, false, "Ambulance", "144", null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "Trip",
                table: "Feedbacks",
                columns: new[] { "Id", "Comment", "CreatedBy", "CreatedDate", "FromUserId", "IsDeleted", "Rating", "ToUserId", "TripId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Great trip, very friendly driver!", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("33333333-3333-3333-3333-333333333333"), false, 5, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Smooth ride, but arrived a bit late.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("44444444-4444-4444-4444-444444444444"), false, 4, new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                schema: "Passenger",
                table: "Passengers",
                columns: new[] { "Id", "CreatedBy", "FirstName", "IsDeleted", "LastName", "RideCount", "UpdatedBy", "UpdatedDate", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Ahmed", false, "Passenger", 3, null, null, "ahmed.passenger@example.com", "01234567890" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, "Mostafa", false, "Passenger", 5, null, null, "mostafa.passenger@example.com", "01534567890" }
                });

            migrationBuilder.InsertData(
                schema: "Vehicle",
                table: "VehicleTypes",
                columns: new[] { "Id", "Capacity", "CreatedBy", "Description", "IsDeleted", "Name", "PerKmCharge", "PerMinuteDelayCharge", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 6, null, "Standard Car", false, "Classic", 1.50m, 0.50m, null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 4, null, "Sport Car", false, "Sport Vehicle", 2.00m, 0.75m, null, null }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "ApplicationUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DriverId", "Email", "EmailConfirmed", "IsActive", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PassengerId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, "Static-Concurrency-Stamp-Value", new Guid("11111111-1111-1111-1111-111111111111"), "mohamed.driver@example.com", true, true, false, false, null, "MOHAMED.DRIVER@EXAMPLE.COM", "MOHAMED.DRIVER", null, "AQAAAAEAACcQAAAAE...", "01234567890", false, "STATIC-SECURITY-STAMP-VALUE", false, "Mohamed.driver" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, "Static-Concurrency-Stamp-Value", new Guid("22222222-2222-2222-2222-222222222222"), "khaled.driver@example.com", true, false, false, false, null, "KHALED.DRIVER@EXAMPLE.COM", "KHALED.DRIVER", null, "AQAAAAEAACcQAAAAE...", "01234567891", false, "STATIC-SECURITY-STAMP-VALUE", false, "khaled.driver" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, "Static-Concurrency-Stamp-Value", null, "ahmed.passenger@example.com", true, true, false, false, null, "AHMED.PASSENGER@EXAMPLE.COM", "AHMED.PASSENGER", new Guid("33333333-3333-3333-3333-333333333333"), "AQAAAAEAACcQAAAAE...", "01234567890", false, "STATIC-SECURITY-STAMP-VALUE", false, "ahmed.passenger" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, "Static-Concurrency-Stamp-Value", null, "mostafa.passenger@example.com", true, true, false, false, null, "MOSTAFA.PASSENGER@EXAMPLE.COM", "MOSTAFA.PASSENGER", new Guid("44444444-4444-4444-4444-444444444444"), "AQAAAAEAACcQAAAAE...", "01534567890", false, "STATIC-SECURITY-STAMP-VALUE", false, "Mostafa.passenger" }
                });

            migrationBuilder.InsertData(
                schema: "Driver",
                table: "Earnings",
                columns: new[] { "Id", "Amount", "CreatedBy", "DriverId", "EarnedAt", "IsDeleted", "IsPaid", "PaymentId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 85.50m, null, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, new Guid("11111111-1111-1111-1111-111111111111"), null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 120.00m, null, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Licenses",
                columns: new[] { "Id", "CreatedBy", "DriverId", "ExpiryDate", "IsValid", "IssueDate", "LicenseNumber", "UpdatedBy", "UpdatedDate", "licenseType" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DR123456", null, null, 0 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DR7891012", null, null, 0 }
                });

            migrationBuilder.InsertData(
                schema: "Sites",
                table: "Sectors",
                columns: new[] { "Id", "CityId", "CreatedBy", "IsDeleted", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), null, false, "sector 1", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), null, false, "sector 2", null, null }
                });

            migrationBuilder.InsertData(
                schema: "Vehicle",
                table: "Vehicles",
                columns: new[] { "Id", "CreatedBy", "DriverId", "IsDeleted", "IsVerified", "LicenseId", "LicensePlate", "Model", "Status", "UpdatedBy", "UpdatedDate", "VehicleTypeId", "Year", "kmPrice" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), null, new Guid("11111111-1111-1111-1111-111111111111"), false, true, new Guid("11111111-1111-1111-1111-111111111111"), "ABC123", "Toyota Prius", 1, null, null, new Guid("11111111-1111-1111-1111-111111111111"), 2020, 0.5m });

            migrationBuilder.InsertData(
                schema: "Vehicle",
                table: "Vehicles",
                columns: new[] { "Id", "CreatedBy", "DriverId", "IsDeleted", "LicenseId", "LicensePlate", "Model", "UpdatedBy", "UpdatedDate", "VehicleTypeId", "Year", "kmPrice" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), null, new Guid("22222222-2222-2222-2222-222222222222"), false, new Guid("22222222-2222-2222-2222-222222222222"), "XYZ789", "Honda Civic", null, null, new Guid("22222222-2222-2222-2222-222222222222"), 2019, 0.8m });

            migrationBuilder.InsertData(
                schema: "User",
                table: "Activities",
                columns: new[] { "Id", "CreatedBy", "EndTime", "IsDeleted", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, null, false, null, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, null, false, null, null, new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "EmergencyNumbers",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "Name", "PhoneNumber", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), null, false, "Maged", "01127378619", null, null, new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                schema: "Sites",
                table: "FavoriteLocations",
                columns: new[] { "Id", "CityId", "CreatedBy", "IsDeleted", "Latitude", "Longitude", "Name", "PassengerId", "SectorId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), null, false, 47.376899999999999, 8.5417000000000005, "dsds", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("11111111-1111-1111-1111-111111111111"), null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), null, false, 46.2044, 6.1432000000000002, "sqqq", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                schema: "License",
                table: "LicensePhotos",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "LicenseId", "PhotoUrl", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, false, new Guid("11111111-1111-1111-1111-111111111111"), "https://example.com/photos/vehicle1_main.jpg", null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, false, new Guid("11111111-1111-1111-1111-111111111111"), "https://example.com/photos/vehicle1_side.jpg", null, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, false, new Guid("22222222-2222-2222-2222-222222222222"), "https://example.com/photos/vehicle2_main.jpg", null, null }
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Licenses",
                columns: new[] { "Id", "CreatedBy", "ExpiryDate", "IssueDate", "LicenseNumber", "UpdatedBy", "UpdatedDate", "VehicleId", "licenseType" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), null, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "ABC123", null, null, new Guid("11111111-1111-1111-1111-111111111111"), 1 });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Licenses",
                columns: new[] { "Id", "CreatedBy", "ExpiryDate", "IsValid", "IssueDate", "LicenseNumber", "UpdatedBy", "UpdatedDate", "VehicleId", "licenseType" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), null, new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2024, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "XYZ789", null, null, new Guid("22222222-2222-2222-2222-222222222222"), 1 });

            migrationBuilder.InsertData(
                schema: "Chat",
                table: "Messages",
                columns: new[] { "Id", "Content", "CreatedBy", "ImagePath", "IsDeleted", "IsRead", "MessageType", "RecipientId", "SenderId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "I have an issue the driver did not arrive till now.", null, null, false, true, 1, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("44444444-4444-4444-4444-444444444444"), null, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "I have an issue that I did not receive my earning yet.", null, null, false, true, 1, new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-2222-2222-2222-222222222222"), null, null }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "Notifications",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDeleted", "Message", "NotificationType", "Title", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Your trip has been confirmed by the driver.", 1, "Trip Confirmed", null, null, new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                schema: "User",
                table: "Notifications",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDeleted", "IsRead", "Message", "NotificationType", "Title", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, "Your payment for the trip has been received.", 3, "Payment Received", null, null, new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.InsertData(
                schema: "User",
                table: "Otp",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDeleted", "IsUsed", "OTPGenerationTime", "UpdatedBy", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, new DateTime(2024, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "654321", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, new DateTime(2024, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                schema: "PaymentMethod",
                table: "PaymentMethods",
                columns: new[] { "Id", "Brand", "CreatedBy", "CreatedDate", "Details", "ExpirationMonth", "ExpirationYear", "IsDefault", "IsDeleted", "Token", "UpdatedBy", "UpdatedDate", "UserId", "paymentMethodType" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Visa", null, new DateTime(2024, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "**** **** **** 4242", 12, 2026, true, false, "tok-test-visa-4242", null, null, new Guid("11111111-1111-1111-1111-111111111111"), 0 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Mastercard", null, new DateTime(2024, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "**** **** **** 5555", 11, 2027, false, false, "tok-test-mastercard-5555", null, null, new Guid("22222222-2222-2222-2222-222222222222"), 2 }
                });

            migrationBuilder.InsertData(
                schema: "Vehicle",
                table: "VehiclePhotos",
                columns: new[] { "Id", "CreatedBy", "Description", "IsDeleted", "IsPrimary", "PhotoUrl", "UpdatedBy", "UpdatedDate", "VehicleId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Main photo of Toyota Prius", false, true, "https://example.com/photos/vehicle1_main.jpg", null, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Side view", false, false, "https://example.com/photos/vehicle1_side.jpg", null, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Main photo of Honda Civic", false, true, "https://example.com/photos/vehicle2_main.jpg", null, null, new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                schema: "Trip",
                table: "TripRequests",
                columns: new[] { "Id", "CreatedBy", "Distance", "DriverId", "EstimatedPrice", "IsAnonymous", "IsDeleted", "PassengerId", "PaymentMethodId", "ScheduledAt", "TripId", "UpdatedBy", "UpdatedDate", "VehicleTypeId", "Start_Address", "Start_Latitude", "Start_Longitude" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), null, 0.0, null, 30.00m, false, false, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("55555555-5555-5555-5555-555555555555"), null, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "Start Location 1", 1.1000000000000001, 1.1000000000000001 });

            migrationBuilder.InsertData(
                schema: "Trip",
                table: "TripRequests",
                columns: new[] { "Id", "CreatedBy", "Distance", "DriverId", "EstimatedPrice", "IsAnonymous", "IsDeleted", "PassengerId", "PaymentMethodId", "ScheduledAt", "Status", "TripId", "UpdatedBy", "UpdatedDate", "VehicleTypeId", "Start_Address", "Start_Latitude", "Start_Longitude" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), null, 0.0, new Guid("22222222-2222-2222-2222-222222222222"), 45.50m, false, false, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("55555555-5555-5555-5555-555555555555"), null, 1, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), "Start Location 2", 2.2000000000000002, 2.2000000000000002 });

            migrationBuilder.InsertData(
                schema: "Driver",
                table: "Offers",
                columns: new[] { "Id", "CreatedBy", "DistanceToArriveInMeters", "DriverId", "IsDeleted", "Price", "TimeToArriveInMinutes", "TripRequestId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), null, 1200.5f, new Guid("11111111-1111-1111-1111-111111111111"), false, 25f, 5, new Guid("11111111-1111-1111-1111-111111111111"), null, null });

            migrationBuilder.InsertData(
                schema: "Driver",
                table: "Offers",
                columns: new[] { "Id", "CreatedBy", "DistanceToArriveInMeters", "DriverId", "IsAccepted", "IsDeleted", "Price", "TimeToArriveInMinutes", "TripRequestId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), null, 800f, new Guid("22222222-2222-2222-2222-222222222222"), true, false, 18.5f, 3, new Guid("22222222-2222-2222-2222-222222222222"), null, null });

            migrationBuilder.InsertData(
                schema: "Trip",
                table: "Trips",
                columns: new[] { "Id", "CreatedBy", "DriverId", "EndedAt", "Fare", "FeedbackId", "IsAnonymous", "IsDeleted", "PassengerId", "PaymentId", "PaymentMethodId", "PaymentMethodType", "Rating", "StartedAt", "Status", "TripRequestId", "UpdatedBy", "UpdatedDate", "Start_Address", "Start_Latitude", "Start_Longitude" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), 35.00m, new Guid("11111111-1111-1111-1111-111111111111"), false, false, new Guid("33333333-3333-3333-3333-333333333333"), null, new Guid("55555555-5555-5555-5555-555555555555"), 0, 5, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, new Guid("22222222-2222-2222-2222-222222222222"), null, null, "Start Location 1", 1.1000000000000001, 1.1000000000000001 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), 50.00m, new Guid("22222222-2222-2222-2222-222222222222"), false, false, new Guid("44444444-4444-4444-4444-444444444444"), null, new Guid("55555555-5555-5555-5555-555555555555"), 8, 4, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, new Guid("11111111-1111-1111-1111-111111111111"), null, null, "Start Location 2", 2.2000000000000002, 2.2000000000000002 }
                });

            migrationBuilder.InsertData(
                schema: "Chat",
                table: "Messages",
                columns: new[] { "Id", "Content", "CreatedBy", "ImagePath", "IsDeleted", "IsRead", "MessageType", "RecipientId", "SenderId", "TripId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Hello, I'll be arriving in 5 minutes for our trip.", null, null, false, true, 0, new Guid("33333333-3333-3333-3333-333333333333"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Great, I'm at the pickup point.", null, null, false, true, 0, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("11111111-1111-1111-1111-111111111111"), null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                schema: "User",
                table: "Activities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "User",
                table: "ApplicationUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_DriverId",
                schema: "User",
                table: "ApplicationUsers",
                column: "DriverId",
                unique: true,
                filter: "[DriverId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_PassengerId",
                schema: "User",
                table: "ApplicationUsers",
                column: "PassengerId",
                unique: true,
                filter: "[PassengerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "User",
                table: "ApplicationUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CityVehicleTypes_CityId",
                schema: "Vehicle",
                table: "CityVehicleTypes",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityVehicleTypes_VehicleTypeId",
                schema: "Vehicle",
                table: "CityVehicleTypes",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Earnings_DriverId",
                schema: "Driver",
                table: "Earnings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyNumbers_UserId",
                schema: "User",
                table: "EmergencyNumbers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EndTripLocations_TripId",
                schema: "Trip",
                table: "EndTripLocations",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_EndTripLocations_TripRequestId",
                schema: "Trip",
                table: "EndTripLocations",
                column: "TripRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteLocations_CityId",
                schema: "Sites",
                table: "FavoriteLocations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteLocations_PassengerId",
                schema: "Sites",
                table: "FavoriteLocations",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteLocations_SectorId",
                schema: "Sites",
                table: "FavoriteLocations",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_FromUserId",
                schema: "Trip",
                table: "Feedbacks",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ToUserId",
                schema: "Trip",
                table: "Feedbacks",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LicensePhotos_LicenseId",
                schema: "License",
                table: "LicensePhotos",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_DriverId",
                schema: "Common",
                table: "Licenses",
                column: "DriverId",
                unique: true,
                filter: "[DriverId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_VehicleId",
                schema: "Common",
                table: "Licenses",
                column: "VehicleId",
                unique: true,
                filter: "[VehicleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                schema: "Chat",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                schema: "Chat",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TripId",
                schema: "Chat",
                table: "Messages",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "User",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DriverId",
                schema: "Driver",
                table: "Offers",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_TripRequestId",
                schema: "Driver",
                table: "Offers",
                column: "TripRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Otp_UserId",
                schema: "User",
                table: "Otp",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserId",
                schema: "PaymentMethod",
                table: "PaymentMethods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EarningId",
                schema: "Trip",
                table: "Payments",
                column: "EarningId",
                unique: true,
                filter: "[EarningId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                schema: "Trip",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_CityId",
                schema: "Sites",
                table: "Sectors",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_DriverId",
                schema: "Trip",
                table: "TripRequests",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_PassengerId",
                schema: "Trip",
                table: "TripRequests",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_PaymentMethodId",
                schema: "Trip",
                table: "TripRequests",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRequests_VehicleTypeId",
                schema: "Trip",
                table: "TripRequests",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverId",
                schema: "Trip",
                table: "Trips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_FeedbackId",
                schema: "Trip",
                table: "Trips",
                column: "FeedbackId",
                unique: true,
                filter: "[FeedbackId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PassengerId",
                schema: "Trip",
                table: "Trips",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PaymentId",
                schema: "Trip",
                table: "Trips",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PaymentMethodId",
                schema: "Trip",
                table: "Trips",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripRequestId",
                schema: "Trip",
                table: "Trips",
                column: "TripRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePhotos_VehicleId",
                schema: "Vehicle",
                table: "VehiclePhotos",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                schema: "Vehicle",
                table: "Vehicles",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                schema: "Vehicle",
                table: "Vehicles",
                column: "VehicleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities",
                schema: "User");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CityVehicleTypes",
                schema: "Vehicle");

            migrationBuilder.DropTable(
                name: "EmergencyNumbers",
                schema: "User");

            migrationBuilder.DropTable(
                name: "EndTripLocations",
                schema: "Trip");

            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "FavoriteLocations",
                schema: "Sites");

            migrationBuilder.DropTable(
                name: "LicensePhotos",
                schema: "License");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Chat");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "User");

            migrationBuilder.DropTable(
                name: "Offers",
                schema: "Driver");

            migrationBuilder.DropTable(
                name: "Otp",
                schema: "User");

            migrationBuilder.DropTable(
                name: "VehiclePhotos",
                schema: "Vehicle");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Sectors",
                schema: "Sites");

            migrationBuilder.DropTable(
                name: "Licenses",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Trips",
                schema: "Trip");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "Sites");

            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "Vehicle");

            migrationBuilder.DropTable(
                name: "Feedbacks",
                schema: "Trip");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "Trip");

            migrationBuilder.DropTable(
                name: "TripRequests",
                schema: "Trip");

            migrationBuilder.DropTable(
                name: "Earnings",
                schema: "Driver");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "VehicleTypes",
                schema: "Vehicle");

            migrationBuilder.DropTable(
                name: "ApplicationUsers",
                schema: "User");

            migrationBuilder.DropTable(
                name: "Drivers",
                schema: "Driver");

            migrationBuilder.DropTable(
                name: "Passengers",
                schema: "Passenger");
        }
    }
}
