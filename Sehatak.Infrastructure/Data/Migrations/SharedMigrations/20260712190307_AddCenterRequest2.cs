using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.SharedMigrations
{
    /// <inheritdoc />
    public partial class AddCenterRequest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdminFullName",
                table: "Center_Registration_Request",
                newName: "AdminLastName");

            migrationBuilder.AddColumn<string>(
                name: "AdminFirstName",
                table: "Center_Registration_Request",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "PartialRefundPercent",
                table: "Center_Registration_Request",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Center_Registration_Request",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PrepaymentAmount",
                table: "Center_Registration_Request",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RefundPolicyHours",
                table: "Center_Registration_Request",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresPrepayment",
                table: "Center_Registration_Request",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "logo",
                table: "Center_Registration_Request",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminFirstName",
                table: "Center_Registration_Request");

            migrationBuilder.DropColumn(
                name: "PartialRefundPercent",
                table: "Center_Registration_Request");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Center_Registration_Request");

            migrationBuilder.DropColumn(
                name: "PrepaymentAmount",
                table: "Center_Registration_Request");

            migrationBuilder.DropColumn(
                name: "RefundPolicyHours",
                table: "Center_Registration_Request");

            migrationBuilder.DropColumn(
                name: "RequiresPrepayment",
                table: "Center_Registration_Request");

            migrationBuilder.DropColumn(
                name: "logo",
                table: "Center_Registration_Request");

            migrationBuilder.RenameColumn(
                name: "AdminLastName",
                table: "Center_Registration_Request",
                newName: "AdminFullName");
        }
    }
}
