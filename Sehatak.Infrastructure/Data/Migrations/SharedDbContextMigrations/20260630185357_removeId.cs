using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.SharedDbContextMigrations
{
    /// <inheritdoc />
    public partial class removeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "platformFeatureId",
                table: "subscription_plans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "platformFeatureId",
                table: "subscription_plans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
