using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sehatak.Infrastructure.Data.Migrations.SharedMigrations
{
    /// <inheritdoc />
    public partial class UpdateFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_platform_features_subscription_plans_SubscriptionPlanId",
                table: "platform_features");

            migrationBuilder.DropIndex(
                name: "IX_platform_features_SubscriptionPlanId",
                table: "platform_features");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "platform_features");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "platform_features",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "plan_features",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_features", x => new { x.PlanId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_plan_features_platform_features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "platform_features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plan_features_subscription_plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "subscription_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_plan_features_FeatureId",
                table: "plan_features",
                column: "FeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "plan_features");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "platform_features");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanId",
                table: "platform_features",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_platform_features_SubscriptionPlanId",
                table: "platform_features",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_platform_features_subscription_plans_SubscriptionPlanId",
                table: "platform_features",
                column: "SubscriptionPlanId",
                principalTable: "subscription_plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
