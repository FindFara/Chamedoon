using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Entity.Countries;
using Chamedoon.Domin.Enums;
using CountryPoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Chamedoon.Application.Services.Immigration
{
    public interface ICountryDataCache
    {
        Task<IReadOnlyDictionary<string, CountryDataSnapshot>> GetAllAsync(CancellationToken cancellationToken);
        void Invalidate();
    }

    public class CountryDataCache : ICountryDataCache
    {
        private const string CacheKey = "country-data-cache";
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CountryDataCache(IApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<IReadOnlyDictionary<string, CountryDataSnapshot>> GetAllAsync(CancellationToken cancellationToken)
        {
            if (_memoryCache.TryGetValue(CacheKey, out IReadOnlyDictionary<string, CountryDataSnapshot>? cached) && cached is not null)
            {
                return cached;
            }

            var countries = await _context.Countries
                .AsNoTracking()
                .AsSplitQuery()
                .Include(c => c.LivingCosts)
                .Include(c => c.Restrictions)
                .Include(c => c.Jobs)
                .Include(c => c.Educations)
                .ToListAsync(cancellationToken);

            var result = countries.ToDictionary(
                c => c.Key,
                c => new CountryDataSnapshot
                {
                    Key = c.Key,
                    Name = c.Name,
                    MaritalStatusImpact = c.MaritalStatusImpact,
                    InvestmentAmount = c.InvestmentAmount,
                    InvestmentCurrency = c.InvestmentCurrency,
                    InvestmentNotes = c.InvestmentNotes,
                    AdditionalInfo = c.AdditionalInfo,
                    LivingCosts = BuildLivingCosts(c.LivingCosts),
                    Restrictions = c.Restrictions.Select(r => r.Description).ToList(),
                    Jobs = c.Jobs
                        .Select(j => new CountryJobSnapshot
                        {
                            Title = j.Title,
                            Description = j.Description,
                            Score = j.Score,
                            ExperienceImpact = j.ExperienceImpact
                        })
                        .ToList(),
                    Educations = c.Educations
                        .Select(e => new CountryEducationSnapshot
                        {
                            FieldName = e.FieldName,
                            Description = e.Description,
                            Score = e.Score,
                            Level = e.Level,
                            LanguageRequirement = e.LanguageRequirement
                        })
                        .ToList()
                },
                StringComparer.OrdinalIgnoreCase);

            _memoryCache.Set(CacheKey, result, TimeSpan.FromHours(6));
            return result;
        }

        public void Invalidate()
        {
            _memoryCache.Remove(CacheKey);
        }

        private static MinimumLivingCosts BuildLivingCosts(IEnumerable<CountryLivingCost> costs)
        {
            var grouped = costs.ToDictionary(cost => cost.Type, cost => cost.Value);
            return new MinimumLivingCosts
            {
                Housing = new List<HousingCost>(),
                FamilyMonthly = GetCost(grouped, LivingCostType.FamilyMonthly),
                SingleMonthly = GetCost(grouped, LivingCostType.SingleMonthly),
                Transport = GetCost(grouped, LivingCostType.Transport),
                Utilities = GetCost(grouped, LivingCostType.Utilities),
                Recreation = GetCost(grouped, LivingCostType.Recreation),
                RentOneBedroom = GetCost(grouped, LivingCostType.RentOneBedroom),
                RentThreeBedroom = GetCost(grouped, LivingCostType.RentThreeBedroom),
                Internet = GetCost(grouped, LivingCostType.Internet)
            };
        }

        private static string GetCost(IReadOnlyDictionary<LivingCostType, string> costs, LivingCostType type)
        {
            return costs.TryGetValue(type, out var value) ? value : string.Empty;
        }
    }

    public class CountryDataSnapshot
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MaritalStatusImpact { get; set; } = string.Empty;
        public string InvestmentNotes { get; set; } = string.Empty;
        public decimal InvestmentAmount { get; set; }
        public string InvestmentCurrency { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
        public MinimumLivingCosts LivingCosts { get; set; } = new() { Housing = new List<HousingCost>() };
        public IReadOnlyList<string> Restrictions { get; set; } = new List<string>();
        public IReadOnlyList<CountryJobSnapshot> Jobs { get; set; } = new List<CountryJobSnapshot>();
        public IReadOnlyList<CountryEducationSnapshot> Educations { get; set; } = new List<CountryEducationSnapshot>();
    }

    public class CountryJobSnapshot
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Score { get; set; }
        public string ExperienceImpact { get; set; } = string.Empty;
    }

    public class CountryEducationSnapshot
    {
        public string FieldName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Level { get; set; } = string.Empty;
        public string LanguageRequirement { get; set; } = string.Empty;
    }
}
