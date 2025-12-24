using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class SubscriptionPlanIndexViewModel
{
    public List<SubscriptionPlanListItemViewModel> Plans { get; set; } = new();
    public string? SearchTerm { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}

public class SubscriptionPlanListItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string DurationLabel { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public int OriginalPrice { get; set; }
    public int Price { get; set; }
    public int? EvaluationLimit { get; set; }
    public bool IncludesAI { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }

    public string PriceLabel => Price.ToString("N0");
    public string OriginalPriceLabel => OriginalPrice > 0 ? OriginalPrice.ToString("N0") : "-";
    public string EvaluationLabel => EvaluationLimit.HasValue ? EvaluationLimit.Value.ToString() : "نامحدود";
    public string AiLabel => IncludesAI ? "فعال" : "غیرفعال";

    public static SubscriptionPlanListItemViewModel FromDto(AdminSubscriptionPlanDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        DurationLabel = dto.DurationLabel,
        DurationMonths = dto.DurationMonths,
        OriginalPrice = dto.OriginalPrice,
        Price = dto.Price,
        EvaluationLimit = dto.EvaluationLimit,
        IncludesAI = dto.IncludesAI,
        IsActive = dto.IsActive,
        SortOrder = dto.SortOrder
    };
}

public class SubscriptionPlanEditViewModel
{
    [MaxLength(64, ErrorMessage = "حداکثر ۶۴ کاراکتر")]
    [Display(Name = "شناسه پلن")]
    public string? Id { get; set; }

    [Required(ErrorMessage = "عنوان پلن را وارد کنید.")]
    [MaxLength(200, ErrorMessage = "حداکثر ۲۰۰ کاراکتر")]
    [Display(Name = "عنوان پلن")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "مدت اشتراک")]
    public int DurationMonths { get; set; } = 1;

    [Range(0, 100000000, ErrorMessage = "مبلغ اصلی معتبر نیست.")]
    [Display(Name = "قیمت اصلی")]
    public int OriginalPrice { get; set; }

    [Range(0, 100000000, ErrorMessage = "قیمت نهایی معتبر نیست.")]
    [Display(Name = "قیمت نهایی")]
    public int Price { get; set; }

    [Display(Name = "محدودیت استعلام")]
    public int? EvaluationLimit { get; set; }

    [Display(Name = "هوش مصنوعی")]
    public bool IncludesAI { get; set; }

    [Display(Name = "ترتیب نمایش")]
    public int SortOrder { get; set; }

    [Display(Name = "فعال باشد؟")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "ویژگی‌ها (هر خط یک مورد)")]
    public string FeaturesText { get; set; } = string.Empty;

    public AdminSubscriptionPlanInput ToInput()
        => new(
            Id?.Trim() ?? string.Empty,
            Title,
            string.Empty,
            DurationMonths,
            OriginalPrice,
            Price,
            EvaluationLimit,
            IncludesAI,
            ParseFeatures(FeaturesText),
            IsActive,
            SortOrder);

    public static SubscriptionPlanEditViewModel FromDto(AdminSubscriptionPlanDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        DurationMonths = dto.DurationMonths,
        OriginalPrice = dto.OriginalPrice,
        Price = dto.Price,
        EvaluationLimit = dto.EvaluationLimit,
        IncludesAI = dto.IncludesAI,
        IsActive = dto.IsActive,
        SortOrder = dto.SortOrder,
        FeaturesText = string.Join(Environment.NewLine, dto.Features)
    };

    private static IReadOnlyList<string> ParseFeatures(string? text)
        => (text ?? string.Empty)
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToList();
}
