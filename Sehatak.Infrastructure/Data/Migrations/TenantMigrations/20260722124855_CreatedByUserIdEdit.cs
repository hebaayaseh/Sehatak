using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.TenantMigrations
{
    /// <inheritdoc />
    public partial class CreatedByUserIdEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE `postponed_services` CHANGE COLUMN `ReceptionistId` `CreatedByUserId` int(11) NOT NULL;");

            migrationBuilder.CreateIndex(
                name: "IX_postponed_services_CreatedByUserId",
                table: "postponed_services",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_postponed_services_users_CreatedByUserId",
                table: "postponed_services",
                column: "CreatedByUserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postponed_services_users_CreatedByUserId",
                table: "postponed_services");

            migrationBuilder.DropIndex(
                name: "IX_postponed_services_CreatedByUserId",
                table: "postponed_services");

            migrationBuilder.Sql(
                "ALTER TABLE `postponed_services` CHANGE COLUMN `CreatedByUserId` `ReceptionistId` int(11) NOT NULL;");

            migrationBuilder.AddForeignKey(
                name: "FK_postponed_services_users_ReceptionistId",
                table: "postponed_services",
                column: "ReceptionistId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
