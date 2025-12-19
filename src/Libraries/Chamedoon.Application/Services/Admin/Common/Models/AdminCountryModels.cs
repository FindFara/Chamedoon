using System.Collections.Generic;
using Chamedoon.Domin.Enums;

namespace Chamedoon.Application.Services.Admin.Common.Models;

public record AdminCountryDto(
    long Id,
    string Key,
    string Name,
    decimal InvestmentAmount,
    string InvestmentCurrency,
    string InvestmentNotes,
    string AdditionalInfo,
    string MaritalStatusImpact,
    IReadOnlyList<AdminCountryLivingCostDto> LivingCosts,
    IReadOnlyList<AdminCountryRestrictionDto> Restrictions,
    IReadOnlyList<AdminCountryJobDto> Jobs,
    IReadOnlyList<AdminCountryEducationDto> Educations);

public record AdminCountryLivingCostDto(
    long Id,
    long CountryId,
    LivingCostType Type,
    string Value);

public record AdminCountryRestrictionDto(
    long Id,
    long CountryId,
    string Description);

public record AdminCountryJobDto(
    long Id,
    long CountryId,
    string Title,
    string Description,
    int Score,
    string ExperienceImpact);

public record AdminCountryEducationDto(
    long Id,
    long CountryId,
    string FieldName,
    string Description,
    int Score,
    string Level,
    string LanguageRequirement);

public record AdminCountryInput(
    long? Id,
    string Key,
    string Name,
    decimal InvestmentAmount,
    string InvestmentCurrency,
    string InvestmentNotes,
    string AdditionalInfo,
    string MaritalStatusImpact);

public record AdminCountryLivingCostInput(
    long? Id,
    long CountryId,
    LivingCostType Type,
    string Value);

public record AdminCountryRestrictionInput(
    long? Id,
    long CountryId,
    string Description);

public record AdminCountryJobInput(
    long? Id,
    long CountryId,
    string Title,
    string Description,
    int Score,
    string ExperienceImpact);

public record AdminCountryEducationInput(
    long? Id,
    long CountryId,
    string FieldName,
    string Description,
    int Score,
    string Level,
    string LanguageRequirement);
