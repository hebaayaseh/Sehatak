using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.SharedMigrations
{
    /// <inheritdoc />
    public partial class addFeatureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "subscription_plans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "platformFeatureId",
                table: "subscription_plans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "platform_features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NameOfFeature = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_platform_features_subscription_plans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "subscription_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "center_features",
                columns: table => new
                {
                    CenterId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_center_features", x => x.CenterId);
                    table.ForeignKey(
                        name: "FK_center_features_medical_centers_CenterId",
                        column: x => x.CenterId,
                        principalTable: "medical_centers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_center_features_platform_features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "platform_features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_center_features_FeatureId",
                table: "center_features",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_platform_features_SubscriptionPlanId",
                table: "platform_features",
                column: "SubscriptionPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "center_features");

            migrationBuilder.DropTable(
                name: "platform_features");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "subscription_plans");

            migrationBuilder.DropColumn(
                name: "platformFeatureId",
                table: "subscription_plans");
        }
    }
}
