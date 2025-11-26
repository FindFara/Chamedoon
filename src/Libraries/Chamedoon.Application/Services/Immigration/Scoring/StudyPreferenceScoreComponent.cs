using System;
using System.Collections.Generic;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class StudyPreferenceScoreComponent : ICountryScoreComponent
    {
        private readonly bool _boostIfWilling;
        private readonly HashSet<CountryType> _studyFriendly = new()
        {
            CountryType.Canada,
            CountryType.Australia,
            CountryType.Germany,
            CountryType.UK,
            CountryType.Netherlands,
            CountryType.Sweden,
            CountryType.Italy,
            CountryType.Spain,
            CountryType.NewZealand
        };

        public StudyPreferenceScoreComponent(double weight = 6, bool boostIfWilling = false)
        {
            Weight = weight;
            _boostIfWilling = boostIfWilling;
        }

        public string Key => "Study";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            if (!context.Input.WillingToStudy)
            {
                return Weight * 0.25;
            }

            var normalized = _studyFriendly.Contains(country) ? 1d : 0.6;
            if (_boostIfWilling && context.Input.Age <= 35)
            {
                normalized += 0.1;
            }

            return Math.Clamp(normalized, 0, 1) * Weight;
        }
    }
}
