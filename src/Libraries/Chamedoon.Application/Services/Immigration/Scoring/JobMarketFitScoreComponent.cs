using System;
using System.Collections.Generic;
using System.Linq;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class JobMarketFitScoreComponent : ICountryScoreComponent
    {
        private readonly Dictionary<CountryType, HashSet<FieldCategoryType>> _fieldDemand = new()
        {
            [CountryType.Canada] = new HashSet<FieldCategoryType> { FieldCategoryType.IT, FieldCategoryType.Engineering, FieldCategoryType.Medicine, FieldCategoryType.Business, FieldCategoryType.Science },
            [CountryType.Australia] = new HashSet<FieldCategoryType> { FieldCategoryType.IT, FieldCategoryType.Engineering, FieldCategoryType.Medicine, FieldCategoryType.Business },
            [CountryType.Germany] = new HashSet<FieldCategoryType> { FieldCategoryType.Engineering, FieldCategoryType.IT, FieldCategoryType.Medicine, FieldCategoryType.Science },
            [CountryType.UK] = new HashSet<FieldCategoryType> { FieldCategoryType.Medicine, FieldCategoryType.IT, FieldCategoryType.Business },
            [CountryType.USA] = new HashSet<FieldCategoryType> { FieldCategoryType.IT, FieldCategoryType.Engineering, FieldCategoryType.Business, FieldCategoryType.Science, FieldCategoryType.Medicine },
            [CountryType.Netherlands] = new HashSet<FieldCategoryType> { FieldCategoryType.IT, FieldCategoryType.Engineering, FieldCategoryType.Business },
            [CountryType.NewZealand] = new HashSet<FieldCategoryType> { FieldCategoryType.Medicine, FieldCategoryType.Engineering, FieldCategoryType.Agriculture, FieldCategoryType.IT },
            [CountryType.Sweden] = new HashSet<FieldCategoryType> { FieldCategoryType.IT, FieldCategoryType.Engineering, FieldCategoryType.Science },
            [CountryType.Denmark] = new HashSet<FieldCategoryType> { FieldCategoryType.Engineering, FieldCategoryType.IT, FieldCategoryType.Science },
            [CountryType.Spain] = new HashSet<FieldCategoryType> { FieldCategoryType.IT, FieldCategoryType.Engineering, FieldCategoryType.Business, FieldCategoryType.Arts },
            [CountryType.Italy] = new HashSet<FieldCategoryType> { FieldCategoryType.Engineering, FieldCategoryType.Arts, FieldCategoryType.Business },
            [CountryType.Switzerland] = new HashSet<FieldCategoryType> { FieldCategoryType.Medicine, FieldCategoryType.Engineering, FieldCategoryType.IT, FieldCategoryType.Business },
            [CountryType.Oman] = new HashSet<FieldCategoryType> { FieldCategoryType.Engineering, FieldCategoryType.Medicine, FieldCategoryType.Business }
        };

        public JobMarketFitScoreComponent(double weight = 12)
        {
            Weight = weight;
        }

        public string Key => "JobMarket";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            var alignment = _fieldDemand.TryGetValue(country, out var set) && set.Contains(context.Input.FieldCategory) ? 1 : 0.6;
            var relevantJob = context.IsTechProfile || context.IsMedicalProfile || context.IsBusinessProfile;
            var synergy = relevantJob && alignment > 0.6 ? 0.1 : 0;
            var normalized = Math.Clamp(context.NormalizedExperience * alignment + synergy, 0, 1);
            return normalized * Weight;
        }
    }
}
