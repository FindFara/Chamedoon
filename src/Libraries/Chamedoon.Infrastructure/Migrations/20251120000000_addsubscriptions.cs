using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chamedoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addsubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionEndDateUtc",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionPlanId",
                table: "Customers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionStartDateUtc",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsedEvaluations",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionEndDateUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SubscriptionStartDateUtc",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UsedEvaluations",
                table: "Customers");
        }
    }
}
