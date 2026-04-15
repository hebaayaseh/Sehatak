using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.TenantMigrations
{
    /// <inheritdoc />
    public partial class AddServicePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "lab_results");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "lab_results");

            migrationBuilder.AlterColumn<string>(
                name: "cancellationReason",
                table: "appointments",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "lab_results",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "lab_results",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "appointments",
                keyColumn: "cancellationReason",
                keyValue: null,
                column: "cancellationReason",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "cancellationReason",
                table: "appointments",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
