using System;
using System.Collections.Generic;
using System.Linq;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class VisaPreferenceScoreComponent : ICountryScoreComponent
    {
        private readonly bool _flexible;
        private readonly Dictionary<CountryType, HashSet<VisaType>> _visaAlignment = new()
        {
            [CountryType.Canada] = new HashSet<VisaType> { VisaType.Work, VisaType.Startup, VisaType.Residence, VisaType.Study },
            [CountryType.Australia] = new HashSet<VisaType> { VisaType.Work, VisaType.Study, VisaType.Residence },
            [CountryType.Germany] = new HashSet<VisaType> { VisaType.Work, VisaType.Study, VisaType.Residence, VisaType.Startup },
            [CountryType.UK] = new HashSet<VisaType> { VisaType.Work, VisaType.Study, VisaType.Family, VisaType.Residence },
            [CountryType.USA] = new HashSet<VisaType> { VisaType.Work, VisaType.Residence, VisaType.Startup, VisaType.Research },
            [CountryType.Netherlands] = new HashSet<VisaType> { VisaType.Work, VisaType.Startup, VisaType.Study, VisaType.Residence },
            [CountryType.NewZealand] = new HashSet<VisaType> { VisaType.Work, VisaType.Study, VisaType.Residence },
            [CountryType.Sweden] = new HashSet<VisaType> { VisaType.Work, VisaType.Startup, VisaType.Study, VisaType.Residence },
            [CountryType.Denmark] = new HashSet<VisaType> { VisaType.Work, VisaType.Startup, VisaType.Study },
            [CountryType.Spain] = new HashSet<VisaType> { VisaType.Work, VisaType.Study, VisaType.Investment, VisaType.DigitalNomad },
            [CountryType.Italy] = new HashSet<VisaType> { VisaType.Work, VisaType.Study, VisaType.Investment },
            [CountryType.Switzerland] = new HashSet<VisaType> { VisaType.Work, VisaType.Study, VisaType.Investment },
            [CountryType.Oman] = new HashSet<VisaType> { VisaType.Work, VisaType.Investment, VisaType.Family }
        };

        public VisaPreferenceScoreComponent(double weight = 6, bool flexible = false)
        {
            Weight = weight;
            _flexible = flexible;
        }

        public string Key => "Visa";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            if (!_visaAlignment.TryGetValue(country, out var aligned))
            {
                return Weight * 0.5;
            }

            if (aligned.Contains(context.Input.VisaType))
            {
                return Weight;
            }

            var normalized = _flexible ? 0.65 : 0.45;
            if (context.Input.VisaType == VisaType.Other)
            {
                normalized -= 0.1;
            }

            return Math.Clamp(normalized, 0, 1) * Weight;
        }
    }
}
