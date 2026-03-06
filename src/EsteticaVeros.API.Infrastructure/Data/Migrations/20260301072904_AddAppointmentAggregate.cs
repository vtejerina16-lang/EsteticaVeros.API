using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsteticaVeros.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    AssignedAssistantId = table.Column<int>(type: "int", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedDuration = table.Column<int>(type: "int", nullable: false),
                    ActualStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDuration = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DepositPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CustomerNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledBy = table.Column<int>(type: "int", nullable: true),
                    NoShowReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReminderSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmedByCustomer = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RecurrencePattern = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RecurrenceEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentAppointmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentServices_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDate",
                table: "Appointments",
                column: "AppointmentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDate_Status",
                table: "Appointments",
                columns: new[] { "AppointmentDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentNumber",
                table: "Appointments",
                column: "AppointmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AssignedAssistantId",
                table: "Appointments",
                column: "AssignedAssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AssignedAssistantId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "AssignedAssistantId", "AppointmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "CustomerId", "AppointmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ParentAppointmentId",
                table: "Appointments",
                column: "ParentAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PaymentStatus",
                table: "Appointments",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Source",
                table: "Appointments",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_AppointmentId",
                table: "AppointmentServices",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_ServiceId",
                table: "AppointmentServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentServices_Status",
                table: "AppointmentServices",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentServices");

            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
