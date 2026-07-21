using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.TenantMigrations
{
    /// <inheritdoc />
    public partial class updateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowFollow_UpDate",
                table: "follow_ups");

            migrationBuilder.AddColumn<DateTime>(
                name: "PreferredTimeSlot",
                table: "waitlists",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromotedAppointmentId",
                table: "waitlists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewTimeSlot",
                table: "postponed_services",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "payments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AllowFollowUpDate",
                table: "follow_ups",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledAppointmentId",
                table: "follow_ups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInTime",
                table: "appointments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_waitlists_PromotedAppointmentId",
                table: "waitlists",
                column: "PromotedAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_follow_ups_ScheduledAppointmentId",
                table: "follow_ups",
                column: "ScheduledAppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_follow_ups_appointments_ScheduledAppointmentId",
                table: "follow_ups",
                column: "ScheduledAppointmentId",
                principalTable: "appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_waitlists_appointments_PromotedAppointmentId",
                table: "waitlists",
                column: "PromotedAppointmentId",
                principalTable: "appointments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_follow_ups_appointments_ScheduledAppointmentId",
                table: "follow_ups");

            migrationBuilder.DropForeignKey(
                name: "FK_waitlists_appointments_PromotedAppointmentId",
                table: "waitlists");

            migrationBuilder.DropIndex(
                name: "IX_waitlists_PromotedAppointmentId",
                table: "waitlists");

            migrationBuilder.DropIndex(
                name: "IX_follow_ups_ScheduledAppointmentId",
                table: "follow_ups");

            migrationBuilder.DropColumn(
                name: "PreferredTimeSlot",
                table: "waitlists");

            migrationBuilder.DropColumn(
                name: "PromotedAppointmentId",
                table: "waitlists");

            migrationBuilder.DropColumn(
                name: "NewTimeSlot",
                table: "postponed_services");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "AllowFollowUpDate",
                table: "follow_ups");

            migrationBuilder.DropColumn(
                name: "ScheduledAppointmentId",
                table: "follow_ups");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "appointments");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AllowFollow_UpDate",
                table: "follow_ups",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
