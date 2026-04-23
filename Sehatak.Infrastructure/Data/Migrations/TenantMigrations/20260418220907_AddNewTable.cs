using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.TenantMigrations
{
    /// <inheritdoc />
    public partial class AddNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ① أول شي احذف الـ FK
            migrationBuilder.DropForeignKey(
                name: "FK_lab_results_patients_PatientId",
                table: "lab_results");

            // ② بعدها احذف الـ Index القديم
            migrationBuilder.DropIndex(
                name: "IX_lab_results_PatientId",
                table: "lab_results");

            // ③ باقي الكود زي ما هو
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "service_prices",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "lab_results",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ConsultationCostId",
                table: "doctors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationCost",
                table: "appointments",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemsTotal",
                table: "appointments",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "appointment_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    ServicePriceId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appointment_items_appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointment_items_service_prices_ServicePriceId",
                        column: x => x.ServicePriceId,
                        principalTable: "service_prices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "follow_ups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OriginalAppointmentId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    ReceptionistId = table.Column<int>(type: "int", nullable: false),
                    AllowFollow_UpDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow_ups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_follow_ups_appointments_OriginalAppointmentId",
                        column: x => x.OriginalAppointmentId,
                        principalTable: "appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_follow_ups_doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_follow_ups_patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patients",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_follow_ups_users_ReceptionistId",
                        column: x => x.ReceptionistId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "postponed_services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ReceptionistId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AppointmentId = table.Column<int>(type: "int", nullable: true),
                    ConsultationId = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postponed_services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_postponed_services_appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_postponed_services_consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_postponed_services_patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patients",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_postponed_services_users_ReceptionistId",
                        column: x => x.ReceptionistId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // ④ أنشئ الـ Index الجديد المدمج بدل القديم
            migrationBuilder.CreateIndex(
                name: "IX_lab_results_PatientId_Status",
                table: "lab_results",
                columns: new[] { "PatientId", "Status" });

            // ⑤ أعد الـ FK على الـ Index الجديد
            migrationBuilder.AddForeignKey(
                name: "FK_lab_results_patients_PatientId",
                table: "lab_results",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "patientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
                name: "IX_service_prices_Type",
                table: "service_prices",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_doctors_ConsultationCostId",
                table: "doctors",
                column: "ConsultationCostId");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_items_AppointmentId",
                table: "appointment_items",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_items_ServicePriceId",
                table: "appointment_items",
                column: "ServicePriceId");

            migrationBuilder.CreateIndex(
                name: "IX_follow_ups_DoctorId",
                table: "follow_ups",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_follow_ups_OriginalAppointmentId",
                table: "follow_ups",
                column: "OriginalAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_follow_ups_PatientId",
                table: "follow_ups",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_follow_ups_ReceptionistId",
                table: "follow_ups",
                column: "ReceptionistId");

            migrationBuilder.CreateIndex(
                name: "IX_postponed_services_AppointmentId",
                table: "postponed_services",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_postponed_services_ConsultationId",
                table: "postponed_services",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_postponed_services_PatientId",
                table: "postponed_services",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_postponed_services_ReceptionistId",
                table: "postponed_services",
                column: "ReceptionistId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_service_prices_ConsultationCostId",
                table: "doctors",
                column: "ConsultationCostId",
                principalTable: "service_prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_service_prices_ConsultationCostId",
                table: "doctors");

            // ① احذف الـ FK الجديد أولاً
            migrationBuilder.DropForeignKey(
                name: "FK_lab_results_patients_PatientId",
                table: "lab_results");

            migrationBuilder.DropTable(name: "appointment_items");
            migrationBuilder.DropTable(name: "follow_ups");
            migrationBuilder.DropTable(name: "postponed_services");

            migrationBuilder.DropIndex(
                name: "IX_service_prices_Type",
                table: "service_prices");

            // ② احذف الـ Index المدمج
            migrationBuilder.DropIndex(
                name: "IX_lab_results_PatientId_Status",
                table: "lab_results");

            migrationBuilder.DropIndex(
                name: "IX_doctors_ConsultationCostId",
                table: "doctors");

            migrationBuilder.DropColumn(name: "Type", table: "service_prices");
            migrationBuilder.DropColumn(name: "Status", table: "lab_results");
            migrationBuilder.DropColumn(name: "ConsultationCostId", table: "doctors");
            migrationBuilder.DropColumn(name: "ConsultationCost", table: "appointments");
            migrationBuilder.DropColumn(name: "ItemsTotal", table: "appointments");

            // ③ أعد الـ Index القديم البسيط
            migrationBuilder.CreateIndex(
                name: "IX_lab_results_PatientId",
                table: "lab_results",
                column: "PatientId");

            // ④ أعد الـ FK على الـ Index القديم
            migrationBuilder.AddForeignKey(
                name: "FK_lab_results_patients_PatientId",
                table: "lab_results",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "patientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}