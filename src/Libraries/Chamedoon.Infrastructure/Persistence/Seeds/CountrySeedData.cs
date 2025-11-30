using System.Collections.Generic;
using System.ComponentModel;
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
            modelBuilder.Entity<CountryJob>().HasData(seed.Jobs);
            modelBuilder.Entity<CountryEducation>().HasData(seed.Educations);
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
                countries.SelectMany(c => c.Restrictions).ToList(),
                countries.SelectMany(c => c.Jobs).ToList(),
                countries.SelectMany(c => c.Educations).ToList());
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

            var restrictions = GetValue(source, nameof(Canada.IranianMigrationRestrictions), new List<string>())
                .Select((text, index) => new CountryRestriction
                {
                    Id = id * 100 + index + 1,
                    CountryId = id,
                    Description = text
                })
                .ToList();

            var livingCosts = MapLivingCosts(id, GetValue(source, nameof(Canada.LivingCosts), new MinimumLivingCosts()));

            var jobs = MapJobs(id, GetValue(source, nameof(Canada.Jobs), new List<JobInfo>()));
            var educations = MapEducations(id, GetValue(source, nameof(Canada.Educations), new List<EducationInfo>()));

            return new CountrySeedRow(country, livingCosts, restrictions, jobs, educations);
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

        private static List<CountryJob> MapJobs(long countryId, IEnumerable<JobInfo> jobs)
        {
            var result = new List<CountryJob>();
            var nextId = countryId * 10000;

            foreach (var job in jobs ?? Enumerable.Empty<JobInfo>())
            {
                result.Add(new CountryJob
                {
                    Id = ++nextId,
                    CountryId = countryId,
                    Title = GetEnumDescription(job.Job),
                    Description = job.Description ?? string.Empty,
                    Score = job.Score,
                    ExperienceImpact = job.ExperienceImpact ?? string.Empty
                });
            }

            return result;
        }

        private static List<CountryEducation> MapEducations(long countryId, IEnumerable<EducationInfo> educations)
        {
            var result = new List<CountryEducation>();
            var nextId = countryId * 20000;

            foreach (var education in educations ?? Enumerable.Empty<EducationInfo>())
            {
                result.Add(new CountryEducation
                {
                    Id = ++nextId,
                    CountryId = countryId,
                    FieldName = GetEnumDescription(education.Field),
                    Description = education.Description ?? string.Empty,
                    Score = education.Score,
                    Level = GetEnumDescription(education.Level),
                    LanguageRequirement = education.LanguageRequirement ?? string.Empty
                });
            }

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

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return value.ToString();
            }

            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return attribute?.Description ?? value.ToString();
        }

        private record CountrySeedRow(
            Country Country,
            List<CountryLivingCost> LivingCosts,
            List<CountryRestriction> Restrictions,
            List<CountryJob> Jobs,
            List<CountryEducation> Educations);

        public record CountrySeedResult(
            List<Country> Countries,
            List<CountryLivingCost> LivingCosts,
            List<CountryRestriction> Restrictions,
            List<CountryJob> Jobs,
            List<CountryEducation> Educations);
    }
}
