using System.Collections.Generic;
using System.Linq;
using Chamedoon.Domin.Entity.Countries;
using Chamedoon.Domin.Enums;
using CountryPoints;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Persistence.Seeds
{
    public static class CountrySeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var seed = Build();

            modelBuilder.Entity<Country>().HasData(seed.Countries);
            modelBuilder.Entity<CountryLivingCost>().HasData(seed.LivingCosts);
            modelBuilder.Entity<CountryRestriction>().HasData(seed.Restrictions);
        }

        public static CountrySeedResult Build()
        {
            var countries = new List<CountrySeedRow>
            {
                BuildSeed(1, "Canada", "کانادا", new Canada()),
                BuildSeed(2, "Australia", "استرالیا", new Australia()),
                BuildSeed(3, "Germany", "آلمان", new Germany()),
                BuildSeed(4, "USA", "ایالات متحده آمریکا", new USA()),
                BuildSeed(5, "Netherlands", "هلند", new Netherlands()),
                BuildSeed(6, "Spain", "اسپانیا", new Spain()),
                BuildSeed(7, "Sweden", "سوئد", new Sweden()),
                BuildSeed(8, "Italy", "ایتالیا", new Italy()),
                BuildSeed(9, "Oman", "عمان", new Oman()),
                BuildSeed(10, "India", "هند", new India())
            };

            return new CountrySeedResult(
                countries.Select(c => c.Country).ToList(),
                countries.SelectMany(c => c.LivingCosts).ToList(),
                countries.SelectMany(c => c.Restrictions).ToList());
        }

        private static CountrySeedRow BuildSeed(long id, string key, string name, dynamic source)
        {
            var country = new Country
            {
                Id = id,
                Key = key,
                Name = name,
                InvestmentAmount = GetValue(source, nameof(Canada.InvestmentAmount), 0m),
                InvestmentCurrency = GetValue(source, nameof(Canada.InvestmentCurrency), string.Empty),
                InvestmentNotes = GetValue(source, nameof(Canada.InvestmentNotes), string.Empty),
                AdditionalInfo = GetValue(source, nameof(Canada.AdditionalInfo), string.Empty),
                MaritalStatusImpact = GetValue(source, nameof(Canada.MaritalStatusImpact), string.Empty)
            };
            var restrictionTexts = GetValue(
                source,
                nameof(Canada.IranianMigrationRestrictions),
                new List<string>()
            ) as IEnumerable<string> ?? Enumerable.Empty<string>();
            
            var restrictions = restrictionTexts
                .Select((text, index) => new CountryRestriction
                {
                    Id = id * 100 + index + 1,
                    CountryId = id,
                    Description = text
                })
                .ToList();

            var livingCosts = MapLivingCosts(id, GetValue(source, nameof(Canada.LivingCosts), new MinimumLivingCosts()));

            return new CountrySeedRow(country, livingCosts, restrictions);
        }

        private static List<CountryLivingCost> MapLivingCosts(long countryId, MinimumLivingCosts costs)
        {
            var result = new List<CountryLivingCost>();
            long nextId = countryId * 1000;

            void AddCost(LivingCostType type, string? value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    result.Add(new CountryLivingCost
                    {
                        Id = ++nextId,
                        CountryId = countryId,
                        Type = type,
                        Value = value
                    });
                }
            }

            AddCost(LivingCostType.FamilyMonthly, costs.FamilyMonthly);
            AddCost(LivingCostType.SingleMonthly, costs.SingleMonthly);
            AddCost(LivingCostType.Transport, costs.Transport);
            AddCost(LivingCostType.Utilities, costs.Utilities);
            AddCost(LivingCostType.Recreation, costs.Recreation);
            AddCost(LivingCostType.RentOneBedroom, costs.RentOneBedroom);
            AddCost(LivingCostType.RentThreeBedroom, costs.RentThreeBedroom);
            AddCost(LivingCostType.Internet, costs.Internet);

            return result;
        }

        private static T GetValue<T>(dynamic source, string propertyName, T defaultValue)
        {
            try
            {
                var value = source.GetType().GetProperty(propertyName)?.GetValue(source);
                return value is T typed ? typed : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private record CountrySeedRow(Country Country, List<CountryLivingCost> LivingCosts, List<CountryRestriction> Restrictions);

        public record CountrySeedResult(List<Country> Countries, List<CountryLivingCost> LivingCosts, List<CountryRestriction> Restrictions);
    }
}
