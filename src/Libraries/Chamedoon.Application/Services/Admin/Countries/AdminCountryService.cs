using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Application.Services.Immigration;
using Chamedoon.Domin.Entity.Countries;

namespace Chamedoon.Application.Services.Admin.Countries;

public class AdminCountryService : IAdminCountryService
{
    private readonly IAdminCountryRepository _repository;
    private readonly ICountryDataCache _countryDataCache;

    public AdminCountryService(IAdminCountryRepository repository, ICountryDataCache countryDataCache)
    {
        _repository = repository;
        _countryDataCache = countryDataCache;
    }

    public async Task<OperationResult<IReadOnlyList<AdminCountryDto>>> GetCountriesAsync(CancellationToken cancellationToken)
    {
        var countries = await _repository.GetCountriesWithDetailsAsync(cancellationToken);
        var mapped = countries.Select(country => country.ToAdminCountryDto()).ToList();
        return OperationResult<IReadOnlyList<AdminCountryDto>>.Success(mapped);
    }

    public async Task<OperationResult<AdminCountryDto>> SaveCountryAsync(AdminCountryInput input, CancellationToken cancellationToken)
    {
        var normalized = NormalizeCountry(input);
        Country? persisted;

        if (normalized.Id is null)
        {
            var entity = new Country
            {
                Key = normalized.Key,
                Name = normalized.Name,
                InvestmentAmount = normalized.InvestmentAmount,
                InvestmentCurrency = normalized.InvestmentCurrency,
                InvestmentNotes = normalized.InvestmentNotes,
                AdditionalInfo = normalized.AdditionalInfo,
                MaritalStatusImpact = normalized.MaritalStatusImpact
            };

            persisted = await _repository.AddCountryAsync(entity, cancellationToken);
        }
        else
        {
            var entity = new Country
            {
                Id = normalized.Id.Value,
                Key = normalized.Key,
                Name = normalized.Name,
                InvestmentAmount = normalized.InvestmentAmount,
                InvestmentCurrency = normalized.InvestmentCurrency,
                InvestmentNotes = normalized.InvestmentNotes,
                AdditionalInfo = normalized.AdditionalInfo,
                MaritalStatusImpact = normalized.MaritalStatusImpact
            };

            persisted = await _repository.UpdateCountryAsync(entity, cancellationToken);
            if (persisted is null)
            {
                return OperationResult<AdminCountryDto>.Fail("کشور مورد نظر یافت نشد.");
            }
        }

        var withDetails = await _repository.GetCountryWithDetailsAsync(persisted.Id, cancellationToken) ?? persisted;
        ClearCountryCache();
        return OperationResult<AdminCountryDto>.Success(withDetails.ToAdminCountryDto());
    }

    public async Task<OperationResult<AdminCountryLivingCostDto>> SaveLivingCostAsync(AdminCountryLivingCostInput input, CancellationToken cancellationToken)
    {
        if (!await CountryExistsAsync(input.CountryId, cancellationToken))
        {
            return OperationResult<AdminCountryLivingCostDto>.Fail("کشور مورد نظر یافت نشد.");
        }

        var entity = await GetOrCreateLivingCostAsync(input, cancellationToken);
        if (entity is null)
        {
            return OperationResult<AdminCountryLivingCostDto>.Fail("رکورد هزینه زندگی یافت نشد.");
        }

        entity.CountryId = input.CountryId;
        entity.Type = input.Type;
        entity.Value = input.Value?.Trim() ?? string.Empty;

        var saved = await _repository.SaveLivingCostAsync(entity, cancellationToken);
        ClearCountryCache();
        return OperationResult<AdminCountryLivingCostDto>.Success(saved.ToAdminCountryLivingCostDto());
    }

    public async Task<OperationResult<AdminCountryRestrictionDto>> SaveRestrictionAsync(AdminCountryRestrictionInput input, CancellationToken cancellationToken)
    {
        if (!await CountryExistsAsync(input.CountryId, cancellationToken))
        {
            return OperationResult<AdminCountryRestrictionDto>.Fail("کشور مورد نظر یافت نشد.");
        }

        var entity = await GetOrCreateRestrictionAsync(input, cancellationToken);
        if (entity is null)
        {
            return OperationResult<AdminCountryRestrictionDto>.Fail("محدودیت انتخابی یافت نشد.");
        }

        entity.CountryId = input.CountryId;
        entity.Description = input.Description?.Trim() ?? string.Empty;

        var saved = await _repository.SaveRestrictionAsync(entity, cancellationToken);
        ClearCountryCache();
        return OperationResult<AdminCountryRestrictionDto>.Success(saved.ToAdminCountryRestrictionDto());
    }

    public async Task<OperationResult<AdminCountryJobDto>> SaveJobAsync(AdminCountryJobInput input, CancellationToken cancellationToken)
    {
        if (!await CountryExistsAsync(input.CountryId, cancellationToken))
        {
            return OperationResult<AdminCountryJobDto>.Fail("کشور مورد نظر یافت نشد.");
        }

        var entity = await GetOrCreateJobAsync(input, cancellationToken);
        if (entity is null)
        {
            return OperationResult<AdminCountryJobDto>.Fail("شغل مورد نظر یافت نشد.");
        }

        entity.CountryId = input.CountryId;
        entity.Title = input.Title?.Trim() ?? string.Empty;
        entity.Description = input.Description?.Trim() ?? string.Empty;
        entity.Score = input.Score;
        entity.ExperienceImpact = input.ExperienceImpact?.Trim() ?? string.Empty;

        var saved = await _repository.SaveJobAsync(entity, cancellationToken);
        ClearCountryCache();
        return OperationResult<AdminCountryJobDto>.Success(saved.ToAdminCountryJobDto());
    }

    public async Task<OperationResult<AdminCountryEducationDto>> SaveEducationAsync(AdminCountryEducationInput input, CancellationToken cancellationToken)
    {
        if (!await CountryExistsAsync(input.CountryId, cancellationToken))
        {
            return OperationResult<AdminCountryEducationDto>.Fail("کشور مورد نظر یافت نشد.");
        }

        var entity = await GetOrCreateEducationAsync(input, cancellationToken);
        if (entity is null)
        {
            return OperationResult<AdminCountryEducationDto>.Fail("رشته تحصیلی انتخابی یافت نشد.");
        }

        entity.CountryId = input.CountryId;
        entity.FieldName = input.FieldName?.Trim() ?? string.Empty;
        entity.Description = input.Description?.Trim() ?? string.Empty;
        entity.Score = input.Score;
        entity.Level = input.Level?.Trim() ?? string.Empty;
        entity.LanguageRequirement = input.LanguageRequirement?.Trim() ?? string.Empty;

        var saved = await _repository.SaveEducationAsync(entity, cancellationToken);
        ClearCountryCache();
        return OperationResult<AdminCountryEducationDto>.Success(saved.ToAdminCountryEducationDto());
    }

    private static AdminCountryInput NormalizeCountry(AdminCountryInput input)
        => new(
            input.Id,
            input.Key?.Trim() ?? string.Empty,
            input.Name?.Trim() ?? string.Empty,
            input.InvestmentAmount,
            input.InvestmentCurrency?.Trim() ?? string.Empty,
            input.InvestmentNotes?.Trim() ?? string.Empty,
            input.AdditionalInfo?.Trim() ?? string.Empty,
            input.MaritalStatusImpact?.Trim() ?? string.Empty);

    private async Task<bool> CountryExistsAsync(long id, CancellationToken cancellationToken)
        => await _repository.GetCountryAsync(id, cancellationToken) is not null;

    private async Task<CountryLivingCost?> GetOrCreateLivingCostAsync(AdminCountryLivingCostInput input, CancellationToken cancellationToken)
    {
        if (input.Id is null)
        {
            return new CountryLivingCost { CountryId = input.CountryId };
        }

        var existing = await _repository.GetLivingCostAsync(input.Id.Value, cancellationToken);
        return existing?.CountryId == input.CountryId ? existing : null;
    }

    private async Task<CountryRestriction?> GetOrCreateRestrictionAsync(AdminCountryRestrictionInput input, CancellationToken cancellationToken)
    {
        if (input.Id is null)
        {
            return new CountryRestriction { CountryId = input.CountryId };
        }

        var existing = await _repository.GetRestrictionAsync(input.Id.Value, cancellationToken);
        return existing?.CountryId == input.CountryId ? existing : null;
    }

    private async Task<CountryJob?> GetOrCreateJobAsync(AdminCountryJobInput input, CancellationToken cancellationToken)
    {
        if (input.Id is null)
        {
            return new CountryJob { CountryId = input.CountryId };
        }

        var existing = await _repository.GetJobAsync(input.Id.Value, cancellationToken);
        return existing?.CountryId == input.CountryId ? existing : null;
    }

    private async Task<CountryEducation?> GetOrCreateEducationAsync(AdminCountryEducationInput input, CancellationToken cancellationToken)
    {
        if (input.Id is null)
        {
            return new CountryEducation { CountryId = input.CountryId };
        }

        var existing = await _repository.GetEducationAsync(input.Id.Value, cancellationToken);
        return existing?.CountryId == input.CountryId ? existing : null;
    }

    private void ClearCountryCache() => _countryDataCache.Invalidate();
}
