using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Chamedoon.Application.Common.Extensions;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Enums;

namespace ChamedoonWebUI.Areas.Admin.ViewModels;

public class CountriesIndexViewModel
{
    public List<CountryPanelViewModel> Countries { get; set; } = new();
    public CountryEditViewModel NewCountry { get; set; } = new();
}

public class CountryPanelViewModel
{
    public long Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal InvestmentAmount { get; set; }
    public string InvestmentCurrency { get; set; } = string.Empty;
    public string InvestmentNotes { get; set; } = string.Empty;
    public string AdditionalInfo { get; set; } = string.Empty;
    public string MaritalStatusImpact { get; set; } = string.Empty;
    public List<CountryLivingCostViewModel> LivingCosts { get; set; } = new();
    public List<CountryRestrictionViewModel> Restrictions { get; set; } = new();
    public List<CountryJobViewModel> Jobs { get; set; } = new();
    public List<CountryEducationViewModel> Educations { get; set; } = new();

    public CountryEditViewModel EditModel => new()
    {
        Id = Id,
        Key = Key,
        Name = Name,
        InvestmentAmount = InvestmentAmount,
        InvestmentCurrency = InvestmentCurrency,
        InvestmentNotes = InvestmentNotes,
        AdditionalInfo = AdditionalInfo,
        MaritalStatusImpact = MaritalStatusImpact
    };

    public CountryLivingCostEditViewModel NewLivingCost => new() { CountryId = Id };
    public CountryRestrictionEditViewModel NewRestriction => new() { CountryId = Id };
    public CountryJobEditViewModel NewJob => new() { CountryId = Id };
    public CountryEducationEditViewModel NewEducation => new() { CountryId = Id };

    public static CountryPanelViewModel FromDto(AdminCountryDto dto) => new()
    {
        Id = dto.Id,
        Key = dto.Key,
        Name = dto.Name,
        InvestmentAmount = dto.InvestmentAmount,
        InvestmentCurrency = dto.InvestmentCurrency,
        InvestmentNotes = dto.InvestmentNotes,
        AdditionalInfo = dto.AdditionalInfo,
        MaritalStatusImpact = dto.MaritalStatusImpact,
        LivingCosts = dto.LivingCosts
            .OrderBy(cost => cost.Type)
            .Select(CountryLivingCostViewModel.FromDto)
            .ToList(),
        Restrictions = dto.Restrictions
            .OrderBy(restriction => restriction.Id)
            .Select(CountryRestrictionViewModel.FromDto)
            .ToList(),
        Jobs = dto.Jobs
            .OrderByDescending(job => job.Score)
            .ThenBy(job => job.Title)
            .Select(CountryJobViewModel.FromDto)
            .ToList(),
        Educations = dto.Educations
            .OrderByDescending(education => education.Score)
            .ThenBy(education => education.FieldName)
            .Select(CountryEducationViewModel.FromDto)
            .ToList()
    };
}

public class CountryEditViewModel
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "کلید کشور را وارد کنید.")]
    [MaxLength(64, ErrorMessage = "حداکثر ۶۴ کاراکتر")]
    public string Key { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام کشور را وارد کنید.")]
    [MaxLength(128, ErrorMessage = "حداکثر ۱۲۸ کاراکتر")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "مبلغ سرمایه‌گذاری")]
    [Range(0, double.MaxValue, ErrorMessage = "مبلغ سرمایه‌گذاری معتبر نیست.")]
    public decimal InvestmentAmount { get; set; }

    [Display(Name = "واحد پول")]
    [MaxLength(16, ErrorMessage = "حداکثر ۱۶ کاراکتر")]
    public string InvestmentCurrency { get; set; } = "USD";

    [Display(Name = "توضیحات سرمایه‌گذاری")]
    [MaxLength(1024, ErrorMessage = "حداکثر ۱۰۲۴ کاراکتر")]
    public string InvestmentNotes { get; set; } = string.Empty;

    [Display(Name = "اطلاعات تکمیلی")]
    [MaxLength(1024, ErrorMessage = "حداکثر ۱۰۲۴ کاراکتر")]
    public string AdditionalInfo { get; set; } = string.Empty;

    [Display(Name = "تأثیر وضعیت تأهل")]
    [MaxLength(1024, ErrorMessage = "حداکثر ۱۰۲۴ کاراکتر")]
    public string MaritalStatusImpact { get; set; } = string.Empty;

    public AdminCountryInput ToInput()
        => new(
            Id,
            Key ?? string.Empty,
            Name ?? string.Empty,
            InvestmentAmount,
            InvestmentCurrency ?? string.Empty,
            InvestmentNotes ?? string.Empty,
            AdditionalInfo ?? string.Empty,
            MaritalStatusImpact ?? string.Empty);
}

public class CountryLivingCostViewModel
{
    public long Id { get; set; }
    public long CountryId { get; set; }
    public LivingCostType Type { get; set; }
    public string Value { get; set; } = string.Empty;

    public string TypeLabel => Type.GetDescription();
    public CountryLivingCostEditViewModel EditModel => new() { Id = Id, CountryId = CountryId, Type = Type, Value = Value };

    public static CountryLivingCostViewModel FromDto(AdminCountryLivingCostDto dto) => new()
    {
        Id = dto.Id,
        CountryId = dto.CountryId,
        Type = dto.Type,
        Value = dto.Value
    };
}

public class CountryRestrictionViewModel
{
    public long Id { get; set; }
    public long CountryId { get; set; }
    public string Description { get; set; } = string.Empty;

    public CountryRestrictionEditViewModel EditModel => new() { Id = Id, CountryId = CountryId, Description = Description };

    public static CountryRestrictionViewModel FromDto(AdminCountryRestrictionDto dto) => new()
    {
        Id = dto.Id,
        CountryId = dto.CountryId,
        Description = dto.Description
    };
}

public class CountryJobViewModel
{
    public long Id { get; set; }
    public long CountryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Score { get; set; }
    public string ExperienceImpact { get; set; } = string.Empty;

    public CountryJobEditViewModel EditModel => new()
    {
        Id = Id,
        CountryId = CountryId,
        Title = Title,
        Description = Description,
        Score = Score,
        ExperienceImpact = ExperienceImpact
    };

    public static CountryJobViewModel FromDto(AdminCountryJobDto dto) => new()
    {
        Id = dto.Id,
        CountryId = dto.CountryId,
        Title = dto.Title,
        Description = dto.Description,
        Score = dto.Score,
        ExperienceImpact = dto.ExperienceImpact
    };
}

public class CountryEducationViewModel
{
    public long Id { get; set; }
    public long CountryId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Level { get; set; } = string.Empty;
    public string LanguageRequirement { get; set; } = string.Empty;

    public CountryEducationEditViewModel EditModel => new()
    {
        Id = Id,
        CountryId = CountryId,
        FieldName = FieldName,
        Description = Description,
        Score = Score,
        Level = Level,
        LanguageRequirement = LanguageRequirement
    };

    public static CountryEducationViewModel FromDto(AdminCountryEducationDto dto) => new()
    {
        Id = dto.Id,
        CountryId = dto.CountryId,
        FieldName = dto.FieldName,
        Description = dto.Description,
        Score = dto.Score,
        Level = dto.Level,
        LanguageRequirement = dto.LanguageRequirement
    };
}

public class CountryLivingCostEditViewModel
{
    public long? Id { get; set; }

    [Required]
    public long CountryId { get; set; }

    [Display(Name = "نوع هزینه")]
    public LivingCostType Type { get; set; }

    [Required(ErrorMessage = "مقدار هزینه را وارد کنید.")]
    [MaxLength(256, ErrorMessage = "حداکثر ۲۵۶ کاراکتر")]
    public string Value { get; set; } = string.Empty;

    public AdminCountryLivingCostInput ToInput()
        => new(Id, CountryId, Type, Value ?? string.Empty);
}

public class CountryRestrictionEditViewModel
{
    public long? Id { get; set; }

    [Required]
    public long CountryId { get; set; }

    [Required(ErrorMessage = "توضیح محدودیت را وارد کنید.")]
    [MaxLength(512, ErrorMessage = "حداکثر ۵۱۲ کاراکتر")]
    public string Description { get; set; } = string.Empty;

    public AdminCountryRestrictionInput ToInput()
        => new(Id, CountryId, Description ?? string.Empty);
}

public class CountryJobEditViewModel
{
    public long? Id { get; set; }

    [Required]
    public long CountryId { get; set; }

    [Required(ErrorMessage = "عنوان شغل را وارد کنید.")]
    [MaxLength(256, ErrorMessage = "حداکثر ۲۵۶ کاراکتر")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "توضیحات شغل را وارد کنید.")]
    [MaxLength(1024, ErrorMessage = "حداکثر ۱۰۲۴ کاراکتر")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "امتیاز")]
    [Range(0, 100, ErrorMessage = "امتیاز باید بین ۰ تا ۱۰۰ باشد.")]
    public int Score { get; set; }

    [Display(Name = "تأثیر سابقه کار")]
    [MaxLength(512, ErrorMessage = "حداکثر ۵۱۲ کاراکتر")]
    public string ExperienceImpact { get; set; } = string.Empty;

    public AdminCountryJobInput ToInput()
        => new(Id, CountryId, Title ?? string.Empty, Description ?? string.Empty, Score, ExperienceImpact ?? string.Empty);
}

public class CountryEducationEditViewModel
{
    public long? Id { get; set; }

    [Required]
    public long CountryId { get; set; }

    [Required(ErrorMessage = "عنوان رشته را وارد کنید.")]
    [MaxLength(256, ErrorMessage = "حداکثر ۲۵۶ کاراکتر")]
    public string FieldName { get; set; } = string.Empty;

    [Required(ErrorMessage = "توضیحات رشته را وارد کنید.")]
    [MaxLength(1024, ErrorMessage = "حداکثر ۱۰۲۴ کاراکتر")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "امتیاز")]
    [Range(0, 100, ErrorMessage = "امتیاز باید بین ۰ تا ۱۰۰ باشد.")]
    public int Score { get; set; }

    [Display(Name = "مقطع/سطح")]
    [MaxLength(256, ErrorMessage = "حداکثر ۲۵۶ کاراکتر")]
    public string Level { get; set; } = string.Empty;

    [Display(Name = "مدرک زبان مورد نیاز")]
    [MaxLength(256, ErrorMessage = "حداکثر ۲۵۶ کاراکتر")]
    public string LanguageRequirement { get; set; } = string.Empty;

    public AdminCountryEducationInput ToInput()
        => new(Id, CountryId, FieldName ?? string.Empty, Description ?? string.Empty, Score, Level ?? string.Empty, LanguageRequirement ?? string.Empty);
}
