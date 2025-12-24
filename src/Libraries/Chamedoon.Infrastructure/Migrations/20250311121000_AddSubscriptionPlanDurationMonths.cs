using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chamedoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionPlanDurationMonths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMonths",
                table: "SubscriptionPlans",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: "starter",
                column: "DurationMonths",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: "unlimited",
                column: "DurationMonths",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: "ai_pro",
                column: "DurationMonths",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMonths",
                table: "SubscriptionPlans");
        }
    }
}
