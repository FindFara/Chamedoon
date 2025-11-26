using System;
using System.Collections.Generic;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class EducationScoreComponent : ICountryScoreComponent
    {
        private readonly bool _emphasizeHighTech;
        private readonly Dictionary<DegreeLevelType, double> _degreeBase = new()
        {
            [DegreeLevelType.HighSchool] = 0.25,
            [DegreeLevelType.Diploma] = 0.4,
            [DegreeLevelType.Associate] = 0.6,
            [DegreeLevelType.Bachelor] = 0.8,
            [DegreeLevelType.Master] = 0.9,
            [DegreeLevelType.Doctorate] = 1.0
        };

        private readonly Dictionary<CountryType, Dictionary<FieldCategoryType, double>> _demandByField;

        public EducationScoreComponent(double weight = 18, bool emphasizeHighTech = false)
        {
            Weight = weight;
            _emphasizeHighTech = emphasizeHighTech;
            _demandByField = new Dictionary<CountryType, Dictionary<FieldCategoryType, double>>
            {
                [CountryType.Canada] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.IT] = 1.0,
                    [FieldCategoryType.Engineering] = 1.0,
                    [FieldCategoryType.Medicine] = 0.95,
                    [FieldCategoryType.Business] = 0.85,
                    [FieldCategoryType.Science] = 0.85,
                    [FieldCategoryType.Arts] = 0.6,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.Australia] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.IT] = 1.0,
                    [FieldCategoryType.Engineering] = 0.95,
                    [FieldCategoryType.Medicine] = 0.95,
                    [FieldCategoryType.Business] = 0.8,
                    [FieldCategoryType.Science] = 0.8,
                    [FieldCategoryType.Arts] = 0.55,
                    [FieldCategoryType.Other] = 0.55
                },
                [CountryType.Germany] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.Engineering] = 1.0,
                    [FieldCategoryType.IT] = 0.95,
                    [FieldCategoryType.Medicine] = 0.95,
                    [FieldCategoryType.Science] = 0.85,
                    [FieldCategoryType.Business] = 0.75,
                    [FieldCategoryType.Arts] = 0.55,
                    [FieldCategoryType.Other] = 0.55
                },
                [CountryType.UK] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.Medicine] = 1.0,
                    [FieldCategoryType.IT] = 0.95,
                    [FieldCategoryType.Engineering] = 0.9,
                    [FieldCategoryType.Business] = 0.85,
                    [FieldCategoryType.Science] = 0.85,
                    [FieldCategoryType.Arts] = 0.6,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.USA] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.IT] = 1.0,
                    [FieldCategoryType.Engineering] = 1.0,
                    [FieldCategoryType.Medicine] = 0.95,
                    [FieldCategoryType.Business] = 0.9,
                    [FieldCategoryType.Science] = 0.9,
                    [FieldCategoryType.Arts] = 0.65,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.Netherlands] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.IT] = 0.95,
                    [FieldCategoryType.Engineering] = 0.9,
                    [FieldCategoryType.Business] = 0.85,
                    [FieldCategoryType.Medicine] = 0.75,
                    [FieldCategoryType.Science] = 0.8,
                    [FieldCategoryType.Arts] = 0.65,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.NewZealand] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.IT] = 0.9,
                    [FieldCategoryType.Engineering] = 0.9,
                    [FieldCategoryType.Medicine] = 0.95,
                    [FieldCategoryType.Business] = 0.85,
                    [FieldCategoryType.Science] = 0.8,
                    [FieldCategoryType.Arts] = 0.6,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.Sweden] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.IT] = 0.95,
                    [FieldCategoryType.Engineering] = 0.9,
                    [FieldCategoryType.Business] = 0.8,
                    [FieldCategoryType.Medicine] = 0.8,
                    [FieldCategoryType.Science] = 0.85,
                    [FieldCategoryType.Arts] = 0.65,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.Denmark] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.Engineering] = 0.95,
                    [FieldCategoryType.IT] = 0.9,
                    [FieldCategoryType.Medicine] = 0.85,
                    [FieldCategoryType.Business] = 0.8,
                    [FieldCategoryType.Science] = 0.85,
                    [FieldCategoryType.Arts] = 0.6,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.Spain] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.IT] = 0.85,
                    [FieldCategoryType.Engineering] = 0.85,
                    [FieldCategoryType.Business] = 0.8,
                    [FieldCategoryType.Medicine] = 0.75,
                    [FieldCategoryType.Science] = 0.75,
                    [FieldCategoryType.Arts] = 0.7,
                    [FieldCategoryType.Other] = 0.55
                },
                [CountryType.Italy] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.Engineering] = 0.85,
                    [FieldCategoryType.IT] = 0.85,
                    [FieldCategoryType.Medicine] = 0.8,
                    [FieldCategoryType.Business] = 0.75,
                    [FieldCategoryType.Science] = 0.75,
                    [FieldCategoryType.Arts] = 0.8,
                    [FieldCategoryType.Other] = 0.55
                },
                [CountryType.Switzerland] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.Engineering] = 0.95,
                    [FieldCategoryType.IT] = 0.9,
                    [FieldCategoryType.Medicine] = 0.9,
                    [FieldCategoryType.Business] = 0.85,
                    [FieldCategoryType.Science] = 0.9,
                    [FieldCategoryType.Arts] = 0.7,
                    [FieldCategoryType.Other] = 0.6
                },
                [CountryType.Oman] = new Dictionary<FieldCategoryType, double>
                {
                    [FieldCategoryType.Engineering] = 0.85,
                    [FieldCategoryType.IT] = 0.85,
                    [FieldCategoryType.Business] = 0.8,
                    [FieldCategoryType.Medicine] = 0.9,
                    [FieldCategoryType.Science] = 0.7,
                    [FieldCategoryType.Arts] = 0.55,
                    [FieldCategoryType.Other] = 0.55
                }
            };
        }

        public string Key => "Education";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            if (!_degreeBase.TryGetValue(context.Input.DegreeLevel, out var baseScore))
            {
                return 0;
            }

            var fieldMultiplier = LookupFieldDemand(country, context.Input.FieldCategory);
            var emphasisBoost = _emphasizeHighTech && (context.IsTechProfile || context.IsMedicalProfile) ? 0.05 : 0;
            var normalized = Math.Clamp(baseScore * fieldMultiplier + emphasisBoost, 0, 1);
            return normalized * Weight;
        }

        private double LookupFieldDemand(CountryType country, FieldCategoryType field)
        {
            if (_demandByField.TryGetValue(country, out var map) && map.TryGetValue(field, out var value))
            {
                return value;
            }

            return 0.6;
        }
    }
}
