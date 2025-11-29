using System;
using System.Collections.Generic;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class PersonalityScoreComponent : ICountryScoreComponent
    {
        private readonly IReadOnlyDictionary<PersonalityType, List<CountryType>> _preferences;

        public PersonalityScoreComponent(IReadOnlyDictionary<PersonalityType, List<CountryType>> preferences, double weight = 6)
        {
            _preferences = preferences;
            Weight = weight;
        }

        public string Key => "Personality";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            if (context.Input.MBTIPersonality == PersonalityType.Unknown)
            {
                return 0;
            }

            if (_preferences.TryGetValue(context.Input.MBTIPersonality, out var list) && list.Contains(country))
            {
                return Weight;
            }

            return Weight * 0.45;
        }
    }
}
