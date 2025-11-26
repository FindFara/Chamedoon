using System;
using System.Collections.Generic;

namespace Chamedoon.Application.Services.Immigration.Scoring
{
    public class LanguageScoreComponent : ICountryScoreComponent
    {
        private readonly Dictionary<LanguageCertificateType, double> _englishStrength = new()
        {
            [LanguageCertificateType.IELTS] = 1.0,
            [LanguageCertificateType.TOEFL] = 0.95,
            [LanguageCertificateType.Other] = 0.6,
            [LanguageCertificateType.None] = 0.1,
            [LanguageCertificateType.GermanA2] = 0.4,
            [LanguageCertificateType.GermanB1] = 0.45,
            [LanguageCertificateType.GermanB2] = 0.5,
            [LanguageCertificateType.GermanC1] = 0.55,
            [LanguageCertificateType.GermanC2] = 0.6
        };

        private readonly Dictionary<LanguageCertificateType, double> _germanStrength = new()
        {
            [LanguageCertificateType.GermanA2] = 0.35,
            [LanguageCertificateType.GermanB1] = 0.55,
            [LanguageCertificateType.GermanB2] = 0.75,
            [LanguageCertificateType.GermanC1] = 0.9,
            [LanguageCertificateType.GermanC2] = 1.0,
            [LanguageCertificateType.IELTS] = 0.65,
            [LanguageCertificateType.TOEFL] = 0.65,
            [LanguageCertificateType.Other] = 0.5,
            [LanguageCertificateType.None] = 0.1
        };

        private readonly HashSet<CountryType> _englishCountries = new()
        {
            CountryType.Canada,
            CountryType.Australia,
            CountryType.UK,
            CountryType.USA,
            CountryType.NewZealand,
            CountryType.Netherlands,
            CountryType.Denmark,
            CountryType.Sweden,
            CountryType.Spain
        };

        private readonly HashSet<CountryType> _germanCountries = new()
        {
            CountryType.Germany,
            CountryType.Switzerland
        };

        public LanguageScoreComponent(double weight = 14)
        {
            Weight = weight;
        }

        public string Key => "Language";

        public double Weight { get; }

        public double CalculateScore(CountryType country, CountryScoreContext context)
        {
            double normalized = 0;
            if (_germanCountries.Contains(country))
            {
                normalized = Lookup(_germanStrength, context.Input.LanguageCertificate);
            }
            else if (_englishCountries.Contains(country))
            {
                normalized = Lookup(_englishStrength, context.Input.LanguageCertificate);
            }
            else
            {
                var best = Math.Max(Lookup(_englishStrength, context.Input.LanguageCertificate), Lookup(_germanStrength, context.Input.LanguageCertificate));
                normalized = Math.Max(0.5, best);
            }

            if (context.Input.LanguageCertificate == LanguageCertificateType.None)
            {
                normalized -= 0.15;
            }

            return Math.Clamp(normalized, 0, 1) * Weight;
        }

        private static double Lookup(Dictionary<LanguageCertificateType, double> map, LanguageCertificateType certificate)
        {
            return map.TryGetValue(certificate, out var value) ? value : 0.5;
        }
    }
}
