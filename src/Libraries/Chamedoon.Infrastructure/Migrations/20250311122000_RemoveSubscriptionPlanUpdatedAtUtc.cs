using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chamedoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSubscriptionPlanUpdatedAtUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "SubscriptionPlans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "SubscriptionPlans",
                type: "datetime2",
                nullable: true);
        }
    }
}
