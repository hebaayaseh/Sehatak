using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.TenantMigrations
{
    /// <inheritdoc />
    public partial class UpdatePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_staff_shifts_users_Id",
                table: "staff_shifts");

            migrationBuilder.DropColumn(
                name: "LabPayment",
                table: "lab_results");

            migrationBuilder.DropColumn(
                name: "consultationCost",
                table: "consultations");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "staff_shifts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "ConsultationId",
                table: "payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LabResultId",
                table: "payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubPatientName",
                table: "patients",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "lab_results",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "consultations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_staff_shifts_staffId",
                table: "staff_shifts",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_lab_results_PaymentId",
                table: "lab_results",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consultations_PaymentId",
                table: "consultations",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_consultations_payments_PaymentId",
                table: "consultations",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_lab_results_payments_PaymentId",
                table: "lab_results",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_staff_shifts_users_staffId",
                table: "staff_shifts",
                column: "staffId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_consultations_payments_PaymentId",
                table: "consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_lab_results_payments_PaymentId",
                table: "lab_results");

            migrationBuilder.DropForeignKey(
                name: "FK_staff_shifts_users_staffId",
                table: "staff_shifts");

            migrationBuilder.DropIndex(
                name: "IX_staff_shifts_staffId",
                table: "staff_shifts");

            migrationBuilder.DropIndex(
                name: "IX_lab_results_PaymentId",
                table: "lab_results");

            migrationBuilder.DropIndex(
                name: "IX_consultations_PaymentId",
                table: "consultations");

            migrationBuilder.DropColumn(
                name: "ConsultationId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "LabResultId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "SubPatientName",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "lab_results");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "consultations");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "staff_shifts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "LabPayment",
                table: "lab_results",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "consultationCost",
                table: "consultations",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_staff_shifts_users_Id",
                table: "staff_shifts",
                column: "Id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
