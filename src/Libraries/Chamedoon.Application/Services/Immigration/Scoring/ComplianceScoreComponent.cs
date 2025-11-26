using System;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class ComplianceScoreComponent : ICountryScoreComponent
    {
        public ComplianceScoreComponent(double weight = 6)
        {
            Weight = weight;
        }

        public string Key => "Compliance";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            var normalized = context.Input.HasCriminalRecord ? 0.25 : 1d;
            return normalized * Weight;
        }
    }
}
