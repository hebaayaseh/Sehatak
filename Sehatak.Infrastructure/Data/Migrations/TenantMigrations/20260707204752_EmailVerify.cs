using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.TenantMigrations
{
    /// <inheritdoc />
    public partial class EmailVerify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PendingValue",
                table: "email_verification_codes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "email_verification_codes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingValue",
                table: "email_verification_codes");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "email_verification_codes");
        }
    }
}
