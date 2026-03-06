using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EsteticaVeros.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessHoursAndSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlotDuration = table.Column<int>(type: "int", nullable: false),
                    BufferTime = table.Column<int>(type: "int", nullable: false),
                    CancellationWindowHours = table.Column<int>(type: "int", nullable: false),
                    NoShowPenaltyPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    RequireDepositForCustomServices = table.Column<bool>(type: "bit", nullable: false),
                    DepositPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MaxConcurrentAppointments = table.Column<int>(type: "int", nullable: false),
                    EnableRecurringAppointments = table.Column<bool>(type: "bit", nullable: false),
                    ReminderHoursBefore = table.Column<int>(type: "int", nullable: false),
                    MaxAdvanceBookingDays = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    IsWorkingDay = table.Column<bool>(type: "bit", nullable: false),
                    MorningStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    MorningEndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    AfternoonStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    AfternoonEndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessHours", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppointmentSettings",
                columns: new[] { "Id", "BufferTime", "CancellationWindowHours", "CreatedAt", "DepositPercentage", "EnableRecurringAppointments", "MaxAdvanceBookingDays", "MaxConcurrentAppointments", "NoShowPenaltyPercentage", "ReminderHoursBefore", "RequireDepositForCustomServices", "SlotDuration", "UpdatedAt" },
                values: new object[] { 1, 15, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20m, true, 60, 2, 10m, 24, true, 30, null });

            migrationBuilder.InsertData(
                table: "BusinessHours",
                columns: new[] { "Id", "AfternoonEndTime", "AfternoonStartTime", "CreatedAt", "DayOfWeek", "IsActive", "IsWorkingDay", "MorningEndTime", "MorningStartTime", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, true, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), null },
                    { 2, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, true, true, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), null },
                    { 3, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, true, true, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), null },
                    { 4, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, true, true, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), null },
                    { 5, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, true, true, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), null },
                    { 6, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, true, true, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), null },
                    { 7, null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, true, false, null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_DayOfWeek",
                table: "BusinessHours",
                column: "DayOfWeek",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentSettings");

            migrationBuilder.DropTable(
                name: "BusinessHours");
        }
    }
}
