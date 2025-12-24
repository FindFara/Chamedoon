using System;
using System.Collections.Generic;
using System.Text.Json;
using Chamedoon.Domin.Entity.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Persistence.Seeds;

public static class SubscriptionPlanSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriptionPlanEntity>().HasData(Build());
    }

    private static List<SubscriptionPlanEntity> Build()
    {
        var createdAtUtc = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Utc);

        return new List<SubscriptionPlanEntity>
        {
            new()
            {
                Id = "starter",
                Title = "پلن پایه (۳ استعلام)",
                DurationLabel = "یک ماهه",
                DurationMonths = 1,
                OriginalPrice = 120_000,
                Price = 37_000,
                EvaluationLimit = 3,
                IncludesAI = false,
                FeaturesJson = SerializeFeatures(new[]
                {
                    "۳ استعلام دقیق ارزیابی مهاجرت",
                    "نمایش گزارش کامل در داشبورد",
                    "پشتیبانی ایمیلی در تمام مدت اشتراک"
                }),
                IsActive = true,
                SortOrder = 1,
                CreatedAtUtc = createdAtUtc
            },
            new()
            {
                Id = "unlimited",
                Title = "پلن حرفه‌ای (نامحدود)",
                DurationLabel = "یک ماهه",
                DurationMonths = 1,
                OriginalPrice = 170_000,
                Price = 49_000,
                EvaluationLimit = null,
                IncludesAI = false,
                FeaturesJson = SerializeFeatures(new[]
                {
                    "استعلام نامحدود در دوره اشتراک",
                    "به‌روزرسانی لحظه‌ای مسیرهای مهاجرتی",
                    "پشتیبانی سریع‌تر در ساعات اداری"
                }),
                IsActive = true,
                SortOrder = 2,
                CreatedAtUtc = createdAtUtc
            },
            new()
            {
                Id = "ai_pro",
                Title = "پلن ویژه (هوش مصنوعی)",
                DurationLabel = "یک ماهه",
                DurationMonths = 1,
                OriginalPrice = 220_000,
                Price = 62_000,
                EvaluationLimit = null,
                IncludesAI = true,
                FeaturesJson = SerializeFeatures(new[]
                {
                    "استعلام نامحدود با دقت بالا",
                    "تحلیل پیشرفته با کمک هوش مصنوعی",
                    "اولویت در پردازش و پاسخگویی"
                }),
                IsActive = true,
                SortOrder = 3,
                CreatedAtUtc = createdAtUtc
            }
        };
    }

    private static string SerializeFeatures(IEnumerable<string> features)
        => JsonSerializer.Serialize(features);
}
