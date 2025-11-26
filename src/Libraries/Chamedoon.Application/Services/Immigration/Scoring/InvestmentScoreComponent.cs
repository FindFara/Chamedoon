using System;
using System.Collections.Generic;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class InvestmentScoreComponent : ICountryScoreComponent
    {
        private readonly Dictionary<CountryType, decimal> _thresholds = new()
        {
            [CountryType.Canada] = 100_000m,
            [CountryType.Australia] = 125_000m,
            [CountryType.Germany] = 75_000m,
            [CountryType.UK] = 150_000m,
            [CountryType.USA] = 200_000m,
            [CountryType.Netherlands] = 60_000m,
            [CountryType.NewZealand] = 80_000m,
            [CountryType.Sweden] = 70_000m,
            [CountryType.Denmark] = 85_000m,
            [CountryType.Spain] = 50_000m,
            [CountryType.Italy] = 40_000m,
            [CountryType.Switzerland] = 200_000m,
            [CountryType.Oman] = 30_000m
        };

        public InvestmentScoreComponent(double weight = 8)
        {
            Weight = weight;
        }

        public string Key => "Investment";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            var threshold = _thresholds.TryGetValue(country, out var value) ? value : 60_000m;
            var normalized = Math.Clamp((double)(context.Input.InvestmentAmount / threshold), 0, 1);
            if (context.Input.InvestmentAmount <= 0)
            {
                normalized *= 0.3;
            }

            return normalized * Weight;
        }
    }
}
