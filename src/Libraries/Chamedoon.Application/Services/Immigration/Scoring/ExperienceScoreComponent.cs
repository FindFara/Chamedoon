using System;
using System.Collections.Generic;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class ExperienceScoreComponent : ICountryScoreComponent
    {
        private readonly bool _rewardDepth;
        private readonly Dictionary<JobCategoryType, double> _baseCategoryWeight = new()
        {
            [JobCategoryType.IT] = 1.0,
            [JobCategoryType.Engineering] = 1.0,
            [JobCategoryType.Healthcare] = 0.95,
            [JobCategoryType.Business] = 0.9,
            [JobCategoryType.Finance] = 0.9,
            [JobCategoryType.Science] = 0.85,
            [JobCategoryType.Telecommunications] = 0.9,
            [JobCategoryType.Energy] = 0.85,
            [JobCategoryType.Education] = 0.8,
            [JobCategoryType.Legal] = 0.75,
            [JobCategoryType.Manufacturing] = 0.75,
            [JobCategoryType.Agriculture] = 0.7,
            [JobCategoryType.Services] = 0.65,
            [JobCategoryType.Hospitality] = 0.65,
            [JobCategoryType.Sales] = 0.7,
            [JobCategoryType.RealEstate] = 0.65,
            [JobCategoryType.Transportation] = 0.65,
            [JobCategoryType.Logistics] = 0.7,
            [JobCategoryType.Arts] = 0.6,
            [JobCategoryType.None] = 0.2
        };

        private readonly Dictionary<CountryType, HashSet<JobCategoryType>> _priorityJobs = new()
        {
            [CountryType.Canada] = new HashSet<JobCategoryType> { JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Healthcare, JobCategoryType.Business, JobCategoryType.Manufacturing, JobCategoryType.Logistics },
            [CountryType.Australia] = new HashSet<JobCategoryType> { JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Healthcare, JobCategoryType.Education, JobCategoryType.Manufacturing },
            [CountryType.Germany] = new HashSet<JobCategoryType> { JobCategoryType.Engineering, JobCategoryType.IT, JobCategoryType.Healthcare, JobCategoryType.Manufacturing, JobCategoryType.Science },
            [CountryType.UK] = new HashSet<JobCategoryType> { JobCategoryType.Healthcare, JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Business, JobCategoryType.Education },
            [CountryType.USA] = new HashSet<JobCategoryType> { JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Healthcare, JobCategoryType.Business, JobCategoryType.Science },
            [CountryType.Netherlands] = new HashSet<JobCategoryType> { JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Business, JobCategoryType.Logistics },
            [CountryType.NewZealand] = new HashSet<JobCategoryType> { JobCategoryType.Healthcare, JobCategoryType.Engineering, JobCategoryType.Agriculture, JobCategoryType.IT },
            [CountryType.Sweden] = new HashSet<JobCategoryType> { JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Science },
            [CountryType.Denmark] = new HashSet<JobCategoryType> { JobCategoryType.Engineering, JobCategoryType.IT, JobCategoryType.Energy },
            [CountryType.Spain] = new HashSet<JobCategoryType> { JobCategoryType.Hospitality, JobCategoryType.IT, JobCategoryType.Agriculture, JobCategoryType.Engineering },
            [CountryType.Italy] = new HashSet<JobCategoryType> { JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Arts, JobCategoryType.Agriculture, JobCategoryType.Manufacturing },
            [CountryType.Switzerland] = new HashSet<JobCategoryType> { JobCategoryType.Healthcare, JobCategoryType.Engineering, JobCategoryType.IT, JobCategoryType.Finance },
            [CountryType.Oman] = new HashSet<JobCategoryType> { JobCategoryType.Engineering, JobCategoryType.Energy, JobCategoryType.Logistics, JobCategoryType.Manufacturing }
        };

        public ExperienceScoreComponent(double weight = 12, bool rewardDepth = false)
        {
            Weight = weight;
            _rewardDepth = rewardDepth;
        }

        public string Key => "Experience";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            var normalizedYears = Math.Clamp(context.Input.WorkExperienceYears / 10d, 0, 1);
            var baseWeight = _baseCategoryWeight.TryGetValue(context.Input.JobCategory, out var categoryScore)
                ? categoryScore
                : 0.5;

            var bonus = _priorityJobs.TryGetValue(country, out var set) && set.Contains(context.Input.JobCategory)
                ? 0.15
                : 0;

            var depth = _rewardDepth && context.Input.WorkExperienceYears >= 8 ? 0.1 : 0;
            var normalized = Math.Clamp(normalizedYears * baseWeight + bonus + depth, 0, 1);
            return normalized * Weight;
        }
    }
}
