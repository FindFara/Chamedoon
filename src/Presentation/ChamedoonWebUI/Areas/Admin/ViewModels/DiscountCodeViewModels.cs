using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Enums;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class DiscountCodeIndexViewModel
{
    public List<DiscountCodeListItemViewModel> Codes { get; set; } = new();
    public string? SearchTerm { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}

public class DiscountCodeListItemViewModel
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }

    public string TypeLabel => Type == DiscountType.Percentage ? "درصدی" : "مبلغی";
    public string ValueLabel => Type == DiscountType.Percentage ? $"{Value}%" : Value.ToString("N0");

    public static DiscountCodeListItemViewModel FromDto(AdminDiscountCodeDto dto) => new()
    {
        Id = dto.Id,
        Code = dto.Code,
        Type = dto.Type,
        Value = dto.Value,
        IsActive = dto.IsActive,
        CreatedAtUtc = dto.CreatedAtUtc,
        ExpiresAtUtc = dto.ExpiresAtUtc
    };
}

public class DiscountCodeEditViewModel
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "کد تخفیف را وارد کنید.")]
    [MaxLength(64, ErrorMessage = "حداکثر ۶۴ کاراکتر")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "نوع تخفیف")]
    public DiscountType Type { get; set; }

    [Range(1, 100000000, ErrorMessage = "مقدار تخفیف معتبر نیست.")]
    public int Value { get; set; }

    [Display(Name = "فعال باشد؟")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "تاریخ انقضا (UTC)")]
    public DateTime? ExpiresAtUtc { get; set; }

    [MaxLength(512)]
    [Display(Name = "توضیحات")]
    public string? Description { get; set; }

    public AdminDiscountCodeInput ToInput()
        => new(Id, Code, Type, Value, IsActive, ExpiresAtUtc, Description);

    public static DiscountCodeEditViewModel FromDto(AdminDiscountCodeDto dto) => new()
    {
        Id = dto.Id,
        Code = dto.Code,
        Type = dto.Type,
        Value = dto.Value,
        IsActive = dto.IsActive,
        ExpiresAtUtc = dto.ExpiresAtUtc,
        Description = dto.Description
    };
}
