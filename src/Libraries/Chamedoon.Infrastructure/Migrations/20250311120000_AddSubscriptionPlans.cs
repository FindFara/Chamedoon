using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chamedoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DurationLabel = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OriginalPrice = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    EvaluationLimit = table.Column<int>(type: "int", nullable: true),
                    IncludesAI = table.Column<bool>(type: "bit", nullable: false),
                    FeaturesJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SubscriptionPlans",
                columns: new[]
                {
                    "Id",
                    "Title",
                    "DurationLabel",
                    "OriginalPrice",
                    "Price",
                    "EvaluationLimit",
                    "IncludesAI",
                    "FeaturesJson",
                    "IsActive",
                    "SortOrder",
                    "CreatedAtUtc",
                    "UpdatedAtUtc"
                },
                values: new object[,]
                {
                    {
                        "starter",
                        "پلن پایه (۳ استعلام)",
                        "یک ماهه",
                        120000,
                        37000,
                        3,
                        false,
                        "[\"۳ استعلام دقیق ارزیابی مهاجرت\",\"نمایش گزارش کامل در داشبورد\",\"پشتیبانی ایمیلی در تمام مدت اشتراک\"]",
                        true,
                        1,
                        new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                        null
                    },
                    {
                        "unlimited",
                        "پلن حرفه‌ای (نامحدود)",
                        "یک ماهه",
                        170000,
                        49000,
                        null,
                        false,
                        "[\"استعلام نامحدود در دوره اشتراک\",\"به‌روزرسانی لحظه‌ای مسیرهای مهاجرتی\",\"پشتیبانی سریع‌تر در ساعات اداری\"]",
                        true,
                        2,
                        new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                        null
                    },
                    {
                        "ai_pro",
                        "پلن ویژه (هوش مصنوعی)",
                        "یک ماهه",
                        220000,
                        62000,
                        null,
                        true,
                        "[\"استعلام نامحدود با دقت بالا\",\"تحلیل پیشرفته با کمک هوش مصنوعی\",\"اولویت در پردازش و پاسخگویی\"]",
                        true,
                        3,
                        new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                        null
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionPlans");
        }
    }
}
