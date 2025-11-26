using System;
using System.Collections.Generic;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class AgeScoreComponent : ICountryScoreComponent
    {
        private readonly AgeProfile _defaultProfile = new(20, 50, 23, 35);
        private readonly Dictionary<CountryType, AgeProfile> _profiles;

        public AgeScoreComponent(double weight = 12)
        {
            Weight = weight;
            _profiles = new Dictionary<CountryType, AgeProfile>
            {
                [CountryType.Canada] = new AgeProfile(18, 45, 22, 35),
                [CountryType.Australia] = new AgeProfile(18, 45, 23, 33),
                [CountryType.Germany] = new AgeProfile(18, 45, 22, 32),
                [CountryType.UK] = new AgeProfile(18, 50, 23, 37),
                [CountryType.USA] = new AgeProfile(18, 50, 24, 38),
                [CountryType.Netherlands] = new AgeProfile(18, 45, 24, 35),
                [CountryType.NewZealand] = new AgeProfile(18, 55, 24, 39),
                [CountryType.Sweden] = new AgeProfile(18, 45, 23, 35),
                [CountryType.Denmark] = new AgeProfile(18, 45, 23, 36),
                [CountryType.Spain] = new AgeProfile(18, 50, 24, 40),
                [CountryType.Italy] = new AgeProfile(18, 50, 24, 40),
                [CountryType.Switzerland] = new AgeProfile(18, 45, 24, 37),
                [CountryType.Oman] = new AgeProfile(18, 55, 25, 45),
            };
        }

        public string Key => "Age";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            var profile = _profiles.TryGetValue(country, out var custom) ? custom : _defaultProfile;
            var normalized = Score(profile, context.Input.Age);
            return Weight * normalized;
        }

        private static double Score(AgeProfile profile, int age)
        {
            if (age <= 0)
            {
                return 0;
            }

            if (age >= profile.IdealMin && age <= profile.IdealMax)
            {
                return 1d;
            }

            if (age < profile.Min || age > profile.Max)
            {
                return 0.25;
            }

            var distance = age < profile.IdealMin
                ? profile.IdealMin - age
                : age - profile.IdealMax;

            var tolerance = Math.Max(profile.IdealMin - profile.Min, profile.Max - profile.IdealMax);
            var decay = 1 - (distance / (double)tolerance);
            return Math.Clamp(decay, 0.25, 0.9);
        }

        private record AgeProfile(int Min, int Max, int IdealMin, int IdealMax);
    }
}
