using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CountryPoints;

namespace Chamedoon.Application.Services.Immigration
{
    /// <summary>
    /// Represents the input information provided by a user for scoring immigration eligibility.
    /// </summary>
    public class ImmigrationInput
    {
        private const int DefaultTextLength = 1000;

        public int Age { get; set; }
        public string? JobTitle { get; set; }
        public JobCategoryType JobCategory { get; set; } = JobCategoryType.None;
        public int WorkExperienceYears { get; set; }
        public FieldCategoryType FieldCategory { get; set; }
        public DegreeLevelType DegreeLevel { get; set; }
        public LanguageCertificateType LanguageCertificate { get; set; }
        public VisaType VisaType { get; set; }
        public bool HasCriminalRecord { get; set; }
        public decimal InvestmentAmount { get; set; }
        public MaritalStatusType MaritalStatus { get; set; }
        public bool WillingToStudy { get; set; }
        public PersonalityType MBTIPersonality { get; set; } = PersonalityType.Unknown;

        [EmailAddress]
        [StringLength(256)]
        public string? Email { get; set; }

        [StringLength(DefaultTextLength)]
        public string? Notes { get; set; }

    }

    public class CountryRecommendation
    {
        public CountryType Country { get; set; }
        public double Score { get; set; }
        public string RecommendedVisaType { get; set; } = string.Empty;
        public CountryDataCard Data { get; set; } = new();
    }

    public class CountryDataCard
    {
        public string MaritalStatusImpact { get; set; } = string.Empty;
        public IReadOnlyList<string> IranianMigrationRestrictions { get; set; } = new List<string>();
        public string InvestmentNotes { get; set; } = string.Empty;
        public decimal InvestmentAmount { get; set; }
        public string InvestmentCurrency { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
        public MinimumLivingCosts LivingCosts { get; set; } = new() { Housing = new List<HousingCost>() };
        public MatchedJobCard? Job { get; set; }
        public MatchedEducationCard? Education { get; set; }
        public string? PersonalityReport { get; set; }
        public string? LanguageRequirement { get; set; }
    }

    public class MatchedJobCard
    {
        public string EnteredTitle { get; set; } = string.Empty;
        public JobTitle Job { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ExperienceImpact { get; set; } = string.Empty;
        public int Score { get; set; }
    }

    public class MatchedEducationCard
    {
        public FieldName Field { get; set; }
        public string Description { get; set; } = string.Empty;
        public string LanguageRequirement { get; set; } = string.Empty;
        public int Score { get; set; }
        public DegreeLevelType DegreeLevel { get; set; }
    }

    public class ImmigrationResult
    {
        public List<CountryRecommendation> TopCountries { get; set; } = new();
    }

    internal record CountryProfile(
        CountryType Country,
        Dictionary<int, string> AgeScores,
        List<JobInfo> Jobs,
        List<EducationInfo> Educations,
        decimal InvestmentAmount,
        string InvestmentCurrency,
        string InvestmentNotes,
        string AdditionalInfo,
        string MaritalStatusImpact,
        List<string> IranianMigrationRestrictions,
        MinimumLivingCosts LivingCosts,
        List<PersonalityType> SuitablePersonalities);

    /// <summary>
    /// سرویس جدید محاسبهٔ امتیاز با استفاده از داده‌های کشورها در مسیر Contries.
    /// </summary>
    public class ImmigrationScoringService
    {
        private readonly ICountryDataCache _countryDataCache;
        private readonly IReadOnlyList<CountryProfile> _countries;

        public ImmigrationScoringService(ICountryDataCache countryDataCache)
        {
            _countryDataCache = countryDataCache;
            _countries = BuildDefaultProfiles();
        }

        public async Task<ImmigrationResult> CalculateImmigrationAsync(ImmigrationInput input, CancellationToken cancellationToken)
        {
            var cachedCountries = await _countryDataCache.GetAllAsync(cancellationToken);
            var enrichedCountries = _countries
                .Select(country => ApplyCachedData(country, cachedCountries))
                .ToList();

            var recommendations = enrichedCountries
                .Select(country => BuildRecommendation(country, input, cachedCountries))
                .OrderByDescending(item => item.Score)
                .Take(3)
                .ToList();

            return new ImmigrationResult { TopCountries = recommendations };
        }

        private static IReadOnlyList<CountryProfile> BuildDefaultProfiles()
        {
            var defaultAgeScores = new Dictionary<int, string>
            {
                { 100, "۲۵-۳۲" },
                { 83, "۱۸-۲۴، ۳۳-۳۹" },
                { 50, "۴۰-۴۴" },
                { 0, "۴۵+" }
            };

            List<CountryProfile> CreateProfiles(params CountryType[] countries)
            {
                return countries
                    .Select(country => new CountryProfile(
                        country,
                        defaultAgeScores,
                        new List<JobInfo>(),
                        new List<EducationInfo>(),
                        0m,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        new List<string>(),
                        new MinimumLivingCosts { Housing = new List<HousingCost>() },
                        new List<PersonalityType>()))
                    .ToList();
            }

            return CreateProfiles(
                CountryType.Canada,
                CountryType.Australia,
                CountryType.Germany,
                CountryType.USA,
                CountryType.Netherlands,
                CountryType.Spain,
                CountryType.Sweden,
                CountryType.Italy,
                CountryType.Oman);
        }

        private CountryRecommendation BuildRecommendation(
            CountryProfile country,
            ImmigrationInput input,
            IReadOnlyDictionary<string, CountryDataSnapshot> cachedCountries)
        {
            var ageScore = CalculateAgeComponent(input.Age, country.AgeScores);
            var (jobMatch, keywordMatched) = FindBestJob(country, input);
            var jobScore = CalculateJobComponent(jobMatch, input, keywordMatched);
            var educationMatch = FindBestEducation(country, input);
            var educationScore = CalculateEducationComponent(educationMatch, input.DegreeLevel);
            var languageScore = CalculateLanguageComponent(input.LanguageCertificate, country.Country, educationMatch);
            var investmentScore = CalculateInvestmentComponent(input.InvestmentAmount, country.InvestmentAmount);
            var personalityScore = CalculatePersonalityComponent(input.MBTIPersonality, country);
            var maritalScore = input.MaritalStatus == MaritalStatusType.Married ? 1 : 0.85;
            var studyScore = input.WillingToStudy || input.VisaType == VisaType.Study ? 1 : 0.2;
            var criminalPenalty = input.HasCriminalRecord ? -15 : 0;

            var total =
                ageScore * 20 +
                jobScore * 25 +
                educationScore * 20 +
                languageScore * 12 +
                investmentScore * 8 +
                personalityScore * 5 +
                maritalScore * 5 +
                studyScore * 5 +
                criminalPenalty;

            var recommendedVisa = ChooseVisa(country, input, investmentScore);
            var educationCard = BuildEducationCard(educationMatch, input);

            var recommendation = new CountryRecommendation
            {
                Country = country.Country,
                Score = Math.Clamp(Math.Round(total, 2), 0, 100),
                RecommendedVisaType = recommendedVisa,
                Data = BuildDataCard(country, input, jobMatch, educationCard, cachedCountries)
            };

            return recommendation;
        }

        private static CountryProfile ApplyCachedData(
            CountryProfile profile,
            IReadOnlyDictionary<string, CountryDataSnapshot> cachedCountries)
        {
            if (!cachedCountries.TryGetValue(profile.Country.ToString(), out var cached))
            {
                return profile;
            }

            var jobs = cached.Jobs
                .Select(TryCreateJobInfo)
                .Where(job => job is not null)
                .Cast<JobInfo>()
                .ToList();

            var educations = cached.Educations
                .Select(TryCreateEducationInfo)
                .Where(education => education is not null)
                .Cast<EducationInfo>()
                .ToList();

            return new CountryProfile(
                profile.Country,
                profile.AgeScores,
                jobs.Count > 0 ? jobs : profile.Jobs,
                educations.Count > 0 ? educations : profile.Educations,
                cached.InvestmentAmount > 0 ? cached.InvestmentAmount : profile.InvestmentAmount,
                string.IsNullOrWhiteSpace(cached.InvestmentCurrency)
                    ? profile.InvestmentCurrency
                    : cached.InvestmentCurrency,
                string.IsNullOrWhiteSpace(cached.InvestmentNotes) ? profile.InvestmentNotes : cached.InvestmentNotes,
                string.IsNullOrWhiteSpace(cached.AdditionalInfo) ? profile.AdditionalInfo : cached.AdditionalInfo,
                string.IsNullOrWhiteSpace(cached.MaritalStatusImpact)
                    ? profile.MaritalStatusImpact
                    : cached.MaritalStatusImpact,
                cached.Restrictions.Count > 0 ? cached.Restrictions.ToList() : profile.IranianMigrationRestrictions,
                cached.LivingCosts?.Housing is not null ? cached.LivingCosts : profile.LivingCosts,
                profile.SuitablePersonalities);
        }

        private CountryDataCard BuildDataCard(
            CountryProfile country,
            ImmigrationInput input,
            JobInfo? jobMatch,
            MatchedEducationCard? educationCard,
            IReadOnlyDictionary<string, CountryDataSnapshot> cachedCountries)
        {
            var card = new CountryDataCard
            {
                MaritalStatusImpact = country.MaritalStatusImpact,
                IranianMigrationRestrictions = country.IranianMigrationRestrictions,
                InvestmentNotes = country.InvestmentNotes,
                InvestmentAmount = country.InvestmentAmount,
                InvestmentCurrency = country.InvestmentCurrency,
                AdditionalInfo = country.AdditionalInfo,
                LivingCosts = country.LivingCosts,
                Job = BuildJobCard(jobMatch, input),
                Education = educationCard,
                PersonalityReport = BuildPersonalityReport(input.MBTIPersonality, country),
                LanguageRequirement = educationCard?.LanguageRequirement
            };

            if (cachedCountries.TryGetValue(country.Country.ToString(), out var cached))
            {
                card.MaritalStatusImpact = string.IsNullOrWhiteSpace(cached.MaritalStatusImpact)
                    ? card.MaritalStatusImpact
                    : cached.MaritalStatusImpact;
                card.IranianMigrationRestrictions = cached.Restrictions.Any()
                    ? cached.Restrictions
                    : card.IranianMigrationRestrictions;
                card.InvestmentNotes = string.IsNullOrWhiteSpace(cached.InvestmentNotes)
                    ? card.InvestmentNotes
                    : cached.InvestmentNotes;
                card.InvestmentAmount = cached.InvestmentAmount > 0 ? cached.InvestmentAmount : card.InvestmentAmount;
                card.InvestmentCurrency = string.IsNullOrWhiteSpace(cached.InvestmentCurrency)
                    ? card.InvestmentCurrency
                    : cached.InvestmentCurrency;
                card.AdditionalInfo = string.IsNullOrWhiteSpace(cached.AdditionalInfo)
                    ? card.AdditionalInfo
                    : cached.AdditionalInfo;
                card.LivingCosts = cached.LivingCosts?.Housing is not null ? cached.LivingCosts : card.LivingCosts;
                card.Job = MergeJobCard(card.Job, cached);
                card.Education = MergeEducationCard(card.Education, cached);
            }

            card.LanguageRequirement ??= educationCard?.LanguageRequirement;
            return card;
        }

        private static string ChooseVisa(CountryProfile country, ImmigrationInput input, double investmentScore)
        {
            var requested = ResolveRequestedVisa(country, input, investmentScore);
            return MapVisaTitle(country.Country, requested);
        }

        private static VisaType ResolveRequestedVisa(CountryProfile country, ImmigrationInput input, double investmentScore)
        {
            if (input.WillingToStudy || input.VisaType == VisaType.Study)
            {
                return VisaType.Study;
            }

            if (country.InvestmentAmount > 0 && investmentScore >= 1)
            {
                return VisaType.Investment;
            }

            if (input.VisaType == VisaType.Investment && investmentScore >= 0.7)
            {
                return VisaType.Investment;
            }

            if (input.VisaType == VisaType.Family && input.MaritalStatus == MaritalStatusType.Married)
            {
                return VisaType.Family;
            }

            if (input.VisaType == VisaType.Residence)
            {
                return VisaType.Residence;
            }

            if (input.VisaType is VisaType.DigitalNomad or VisaType.Freelancer)
            {
                return input.VisaType;
            }

            if (input.VisaType is VisaType.Startup or VisaType.Research or VisaType.Retirement
                or VisaType.Cultural or VisaType.Humanitarian or VisaType.Asylum)
            {
                return input.VisaType;
            }

            if (input.VisaType == VisaType.Tourist)
            {
                return VisaType.Tourist;
            }

            if (input.WorkExperienceYears > 0 || input.JobCategory != JobCategoryType.None)
            {
                return VisaType.Work;
            }

            return VisaType.Tourist;
        }

        private static string MapVisaTitle(CountryType country, VisaType requested)
        {
            // برای سادگی، نام فارسی و انگلیسی در پرانتز آمده‌اند.
            if (country == CountryType.Canada)
            {
                return requested switch
                {
                    VisaType.Study => "تحصیلی (Study Permit)",
                    VisaType.Investment => "برنامه سرمایه‌گذاری یا کارآفرینی",
                    VisaType.Work => "اکسپرس اینتری (Express Entry)",
                    VisaType.Family => "ویزای اسپانسرشیپ خانواده",
                    VisaType.Tourist => "ویزای توریستی (Visitor Visa)",
                    VisaType.Startup => "ویزای استارتاپ (Start-Up Visa)",
                    VisaType.Residence => "اقامت دائم (Permanent Residency)",
                    VisaType.DigitalNomad => "ویزای کار از راه دور کانادا",
                    VisaType.Freelancer => "ویزای فریلنسری یا خوداشتغالی",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "اکسپرس اینتری (Express Entry)"
                };
            }
            if (country == CountryType.Australia)
            {
                return requested switch
                {
                    VisaType.Study => "دانشجویی (Student Visa)",
                    VisaType.Investment => "ویزای سرمایه‌گذاری (Business Innovation)",
                    VisaType.Work => "ویزای نیروی ماهر 189 (Skilled Independent)",
                    VisaType.Family => "ویزای خانواده یا Partner",
                    VisaType.Tourist => "ویزای توریستی (Visitor Visa)",
                    VisaType.Startup => "ویزای نوآوری کسب‌وکار (Business Innovation)",
                    VisaType.Residence => "اقامت دائم (Permanent Residency)",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری",
                    VisaType.Research => "ویزای تحقیقاتی یا پژوهشی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "ویزای نیروی ماهر 189 (Skilled Independent)"
                };
            }
            if (country == CountryType.Germany)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای دانشجویی",
                    VisaType.Investment => "ویزای سرمایه‌گذاری یا کارآفرینی",
                    VisaType.Work => "فرصت کارت (Opportunity Card)",
                    VisaType.Family => "ویزای پیوست خانواده",
                    VisaType.Tourist => "ویزای شینگن (Tourist)",
                    VisaType.Startup => "ویزای استارتاپ آلمان",
                    VisaType.Residence => "اقامت دائم (Permanent Residence)",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری یا فریلنس آزاد",
                    VisaType.Research => "ویزای تحقیقاتی یا پژوهشی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی آلمان",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای بشر‌دوستانه",
                    _ => "فرصت کارت (Opportunity Card)"
                };
            }
            // USA
            if (country == CountryType.USA)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای تحصیلی (F‑1)",
                    VisaType.Investment => "برنامه سرمایه‌گذاری EB‑5",
                    VisaType.Work => "ویزای کار تخصصی (H‑1B/EB)",
                    VisaType.Family => "ویزای خانوادگی (Family Green Card)",
                    VisaType.Tourist => "ویزای توریستی (B1/B2)",
                    VisaType.Startup => "ویزای کارآفرینی (E‑2/EB‑2)",
                    VisaType.Residence => "گرین کارت (Permanent Resident)",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری",
                    VisaType.Research => "ویزای تحقیقاتی J‑1/H‑1B",
                    VisaType.Retirement => "ویزای بازنشستگی (Retiree Visa)",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی (O‑1/P)",
                    VisaType.Humanitarian => "ویزای بشر‌دوستانه (Humanitarian Parole)",
                    _ => "ویزای کار تخصصی (H‑1B/EB)"
                };
            }
            // UK
            if (country == CountryType.UK)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای دانشجویی (Student Visa)",
                    VisaType.Investment => "ویزای سرمایه‌گذاری یا Innovator",
                    VisaType.Work => "ویزای کار ماهر (Skilled Worker)",
                    VisaType.Family => "ویزای خانوادگی (Family Visa)",
                    VisaType.Tourist => "ویزای توریستی (Visitor Visa)",
                    VisaType.Startup => "ویزای استارتاپ یا Innovator",
                    VisaType.Residence => "اقامت دائم (Indefinite Leave)",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری/Global Talent",
                    VisaType.Research => "ویزای تحقیقاتی (Global Talent)",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی (Temporary Work—Creative)",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "ویزای کار ماهر (Skilled Worker)"
                };
            }
            // New Zealand
            if (country == CountryType.NewZealand)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای تحصیلی",
                    VisaType.Investment => "ویزای سرمایه‌گذاری (Investor Visa)",
                    VisaType.Work => "دسته مهاجرت ماهر (Skilled Migrant)",
                    VisaType.Family => "ویزای خانواده یا همراه",
                    VisaType.Tourist => "ویزای توریستی",
                    VisaType.Startup => "ویزای کارآفرینی (Entrepreneur Work Visa)",
                    VisaType.Residence => "ویزای اقامت (Resident Visa)",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "دسته مهاجرت ماهر (Skilled Migrant)"
                };
            }
            // Netherlands
            if (country == CountryType.Netherlands)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای دانشجویی",
                    VisaType.Investment => "ویزای استارتاپ یا خوداشتغالی",
                    VisaType.Work => "ویزای نیروی متخصص (Highly Skilled Migrant)",
                    VisaType.Family => "ویزای پیوست خانواده",
                    VisaType.Tourist => "ویزای توریستی (Schengen)",
                    VisaType.Startup => "ویزای استارتاپ (Start‑Up Visa)",
                    VisaType.Residence => "اقامت دائم (Permanent Residence Permit)",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری یا خوداشتغالی",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "ویزای نیروی متخصص (Highly Skilled Migrant)"
                };
            }
            // Spain
            if (country == CountryType.Spain)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای تحصیلی",
                    VisaType.Investment => "ویزای طلایی (Golden Visa)",
                    VisaType.Work => "ویزای کار",
                    VisaType.Family => "ویزای پیوست خانواده",
                    VisaType.Tourist => "ویزای توریستی",
                    VisaType.Startup => "ویزای استارتاپ اسپانیا",
                    VisaType.Residence => "اقامت دائم",
                    VisaType.DigitalNomad => "ویزای دیجیتال نوماد اسپانیا",
                    VisaType.Freelancer => "ویزای خوداشتغالی",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "ویزای کار"
                };
            }
            // Sweden
            if (country == CountryType.Sweden)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای تحصیلی",
                    VisaType.Investment => "ویزای استارتاپ یا خوداشتغالی",
                    VisaType.Work => "اجازه کار",
                    VisaType.Family => "ویزای پیوست خانواده",
                    VisaType.Tourist => "ویزای توریستی",
                    VisaType.Startup => "ویزای استارتاپ سوئد",
                    VisaType.Residence => "اقامت دائم",
                    VisaType.DigitalNomad => "ویزای دیجیتال نوماد",
                    VisaType.Freelancer => "ویزای فریلنسری/خوداشتغالی",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "اجازه کار"
                };
            }
            // Denmark
            if (country == CountryType.Denmark)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای تحصیلی",
                    VisaType.Investment => "برنامه استارتاپ دانمارک",
                    VisaType.Work => "طرح مثبت لیست یا Pay Limit",
                    VisaType.Family => "ویزای پیوست خانواده",
                    VisaType.Tourist => "ویزای توریستی",
                    VisaType.Startup => "ویزای استارتاپ دانمارک",
                    VisaType.Residence => "اقامت دائم",
                    VisaType.DigitalNomad => "ویزای دیجیتال نوماد",
                    VisaType.Freelancer => "ویزای فریلنسری",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی دانمارک",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "طرح مثبت لیست یا Pay Limit"
                };
            }

            // Italy
            if (country == CountryType.Italy)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای تحصیلی ایتالیا",
                    VisaType.Investment => "ویزای سرمایه‌گذاری (Investor Visa)",
                    VisaType.Work => "ویزای کار ایتالیا",
                    VisaType.Family => "ویزای پیوست خانواده",
                    VisaType.Tourist => "ویزای توریستی",
                    VisaType.Startup => "ویزای استارتاپ ایتالیا",
                    VisaType.Residence => "اقامت ایتالیا",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری/خوداشتغالی",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی ایتالیا",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای بشر‌دوستانه",
                    _ => "ویزای کار ایتالیا"
                };
            }

            // Switzerland
            if (country == CountryType.Switzerland)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای تحصیلی",
                    VisaType.Investment => "ویزای سرمایه‌گذاری یا اقامت مالیاتی",
                    VisaType.Work => "اجازه کار (Permit L/B)",
                    VisaType.Family => "ویزای پیوست خانواده",
                    VisaType.Tourist => "ویزای توریستی (شینگن)",
                    VisaType.Startup => "ویزای کارآفرینی",
                    VisaType.Residence => "اجازه اقامت C",
                    VisaType.DigitalNomad => "ویزای دیجیتال نوماد سوئیس",
                    VisaType.Freelancer => "ویزای فریلنسری یا خوداشتغالی",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی سوئیس",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای بشر‌دوستانه",
                    _ => "اجازه کار (Permit L/B)"
                };
            }

            // Oman
            if (country == CountryType.Oman)
            {
                return requested switch
                {
                    VisaType.Study => "ویزای دانشجویی",
                    VisaType.Investment => "ویزای سرمایه‌گذاری",
                    VisaType.Work => "ویزای کار",
                    VisaType.Family => "ویزای خانواده",
                    VisaType.Tourist => "ویزای توریستی",
                    VisaType.Startup => "ویزای کارآفرینی",
                    VisaType.Residence => "اقامت طولانی مدت",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    VisaType.Freelancer => "ویزای فریلنسری",
                    VisaType.Research => "ویزای تحقیقاتی",
                    VisaType.Retirement => "ویزای بازنشستگی",
                    VisaType.Asylum => "پناهندگی",
                    VisaType.Cultural => "ویزای فرهنگی",
                    VisaType.Humanitarian => "ویزای انسان‌دوستانه",
                    _ => "ویزای کار"
                };
            }

            return "ویزای متناسب با شرایط";
        }

        private static JobInfo? TryCreateJobInfo(CountryJobSnapshot snapshot)
        {
            if (!TryParseEnumByDescription(snapshot.Title, out JobTitle title))
            {
                return null;
            }

            return new JobInfo
            {
                Job = title,
                Description = snapshot.Description ?? string.Empty,
                Score = snapshot.Score,
                ExperienceImpact = snapshot.ExperienceImpact ?? string.Empty
            };
        }

        private static EducationInfo? TryCreateEducationInfo(CountryEducationSnapshot snapshot)
        {
            if (!TryParseEnumByDescription(snapshot.FieldName, out FieldName field))
            {
                return null;
            }

            var level = TryParseEnumByDescription(snapshot.Level, out EducationLevel parsedLevel)
                ? parsedLevel
                : EducationLevel.Bachelor;

            return new EducationInfo
            {
                Field = field,
                Description = snapshot.Description ?? string.Empty,
                Score = snapshot.Score,
                Level = level,
                LanguageRequirement = snapshot.LanguageRequirement ?? string.Empty
            };
        }

        private static MatchedJobCard? BuildJobCard(JobInfo? match, ImmigrationInput input)
        {
            if (match == null)
            {
                return null;
            }

            return new MatchedJobCard
            {
                EnteredTitle = input.JobTitle ?? string.Empty,
                Job = match.Job,
                Description = match.Description,
                ExperienceImpact = match.ExperienceImpact,
                Score = match.Score
            };
        }

        private static MatchedEducationCard? BuildEducationCard(EducationInfo? match, ImmigrationInput input)
        {
            if (match == null)
            {
                return null;
            }

            return new MatchedEducationCard
            {
                Field = match.Field,
                Description = match.Description,
                LanguageRequirement = match.LanguageRequirement,
                Score = match.Score,
                DegreeLevel = input.DegreeLevel
            };
        }

        private static MatchedJobCard? MergeJobCard(MatchedJobCard? jobCard, CountryDataSnapshot cached)
        {
            if (jobCard == null || cached.Jobs.Count == 0)
            {
                return jobCard;
            }

            var title = GetEnumDescription(jobCard.Job);
            var match = cached.Jobs.FirstOrDefault(j => string.Equals(j.Title, title, StringComparison.OrdinalIgnoreCase));
            if (match == null)
            {
                return jobCard;
            }

            jobCard.Description = string.IsNullOrWhiteSpace(match.Description) ? jobCard.Description : match.Description;
            jobCard.ExperienceImpact = string.IsNullOrWhiteSpace(match.ExperienceImpact)
                ? jobCard.ExperienceImpact
                : match.ExperienceImpact;
            jobCard.Score = match.Score > 0 ? match.Score : jobCard.Score;
            return jobCard;
        }

        private static MatchedEducationCard? MergeEducationCard(MatchedEducationCard? educationCard, CountryDataSnapshot cached)
        {
            if (educationCard == null || cached.Educations.Count == 0)
            {
                return educationCard;
            }

            var fieldName = GetEnumDescription(educationCard.Field);
            var match = cached.Educations.FirstOrDefault(e => string.Equals(e.FieldName, fieldName, StringComparison.OrdinalIgnoreCase));
            if (match == null)
            {
                return educationCard;
            }

            educationCard.Description = string.IsNullOrWhiteSpace(match.Description)
                ? educationCard.Description
                : match.Description;
            educationCard.LanguageRequirement = string.IsNullOrWhiteSpace(match.LanguageRequirement)
                ? educationCard.LanguageRequirement
                : match.LanguageRequirement;
            educationCard.Score = match.Score > 0 ? match.Score : educationCard.Score;
            return educationCard;
        }

        private static bool TryParseEnumByDescription<TEnum>(string? value, out TEnum result)
            where TEnum : struct, Enum
        {
            result = default;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            foreach (var candidate in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                var description = GetEnumDescription((Enum)(object)candidate);

                if (string.Equals(description, value, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(candidate.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    result = candidate;
                    return true;
                }
            }

            return false;
        }

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return value.ToString();
            }

            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return attribute?.Description ?? value.ToString();
        }

        private static double CalculateAgeComponent(int age, Dictionary<int, string> ageScores)
        {
            if (age <= 0 || ageScores.Count == 0)
            {
                return 0.3;
            }

            double bestScore = 0.2;
            double bestDistance = double.MaxValue;

            foreach (var entry in ageScores)
            {
                var range = ParseRange(entry.Value);
                var normalized = entry.Key / 100d;
                if (IsInRange(age, range))
                {
                    bestScore = Math.Max(bestScore, normalized);
                    bestDistance = 0;
                }
                else
                {
                    var distance = RangeDistance(age, range);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestScore = Math.Max(0.2, normalized * Math.Max(0.4, 1 - distance / 30));
                    }
                }
            }

            return Math.Clamp(bestScore, 0, 1);
        }

        private static (int? Start, int? End) ParseRange(string value)
        {
            var numbers = new List<int>();
            var buffer = string.Empty;
            foreach (var ch in value)
            {
                if (char.IsDigit(ch))
                {
                    buffer += ((int)char.GetNumericValue(ch)).ToString(CultureInfo.InvariantCulture);
                }
                else if (buffer.Length > 0)
                {
                    numbers.Add(int.Parse(buffer, CultureInfo.InvariantCulture));
                    buffer = string.Empty;
                }
            }

            if (buffer.Length > 0)
            {
                numbers.Add(int.Parse(buffer, CultureInfo.InvariantCulture));
            }

            var hasPlus = value.Contains('+');
            var lessThan = value.Contains("زیر", StringComparison.OrdinalIgnoreCase) || value.Contains("کمتر", StringComparison.OrdinalIgnoreCase);

            if (numbers.Count >= 2)
            {
                return (numbers[0], numbers[1]);
            }

            if (numbers.Count == 1)
            {
                if (hasPlus)
                {
                    return (numbers[0], null);
                }

                if (lessThan)
                {
                    return (null, numbers[0] - 1);
                }

                return (numbers[0], numbers[0]);
            }

            return (null, null);
        }

        private static bool IsInRange(int age, (int? Start, int? End) range)
        {
            if (range.Start.HasValue && range.End.HasValue)
            {
                return age >= range.Start.Value && age <= range.End.Value;
            }

            if (range.Start.HasValue && !range.End.HasValue)
            {
                return age >= range.Start.Value;
            }

            if (!range.Start.HasValue && range.End.HasValue)
            {
                return age < range.End.Value;
            }

            return false;
        }

        private static double RangeDistance(int age, (int? Start, int? End) range)
        {
            if (range.Start.HasValue && range.End.HasValue)
            {
                var midpoint = (range.Start.Value + range.End.Value) / 2d;
                return Math.Abs(age - midpoint);
            }

            if (range.Start.HasValue && !range.End.HasValue)
            {
                return Math.Max(0, range.Start.Value - age);
            }

            if (!range.Start.HasValue && range.End.HasValue)
            {
                return Math.Max(0, age - range.End.Value);
            }

            return 100;
        }

        private static double CalculateEducationComponent(EducationInfo? match, DegreeLevelType degree)
        {
            double baseScore = degree switch
            {
                DegreeLevelType.HighSchool => 0.35,
                DegreeLevelType.Diploma => 0.45,
                DegreeLevelType.Associate => 0.55,
                DegreeLevelType.Bachelor => 0.7,
                DegreeLevelType.Master => 0.85,
                DegreeLevelType.Doctorate => 1,
                _ => 0.5
            };

            var matchScore = match?.Score / 100d ?? 0.5;
            return Math.Clamp((baseScore * 0.5) + (matchScore * 0.5), 0, 1);
        }

        private static double CalculateJobComponent(JobInfo? match, ImmigrationInput input, bool keywordBoost)
        {
            double score;

            if (match != null)
            {
                score = match.Score / 100d;
            }
            else if (input.JobCategory == JobCategoryType.None)
            {
                score = 0.25;
            }
            else
            {
                score = 0.5;
            }

            if (input.WorkExperienceYears >= 5)
            {
                score += 0.1;
            }
            else if (input.WorkExperienceYears >= 2)
            {
                score += 0.05;
            }

            if (keywordBoost)
            {
                score += 0.05;
            }

            return Math.Clamp(score, 0, 1);
        }

        private static (JobInfo? Job, bool KeywordMatched) FindBestJob(CountryProfile country, ImmigrationInput input)
        {
            if (country.Jobs.Count == 0)
            {
                return (null, false);
            }

            var preferredTitles = GetRelevantJobTitles(input.JobCategory, input.FieldCategory).ToHashSet();
            var match = country.Jobs
                .Where(job => preferredTitles.Contains(job.Job))
                .OrderByDescending(job => job.Score)
                .FirstOrDefault();

            if (match != null)
            {
                return (match, false);
            }

            if (!string.IsNullOrWhiteSpace(input.JobTitle))
            {
                var normalized = input.JobTitle.ToLowerInvariant();
                var keywordMap = new Dictionary<string, JobTitle>
                {
                    { "نرم افزار", JobTitle.SoftwareDeveloper },
                    { "برنامه", JobTitle.SoftwareDeveloper },
                    { "developer", JobTitle.SoftwareDeveloper },
                    { "مهندس", JobTitle.MechanicalEngineer },
                    { "civil", JobTitle.CivilEngineer },
                    { "برق", JobTitle.ElectricalEngineer },
                    { "data", JobTitle.DataScientist },
                    { "تحلیل", JobTitle.BusinessAnalyst },
                    { "مدیر پروژه", JobTitle.ProjectManager },
                    { "پروژه", JobTitle.ProjectManager },
                    { "پرستار", JobTitle.Nurse },
                    { "معلم", JobTitle.Teacher },
                    { "chef", JobTitle.Chef },
                    { "آشپز", JobTitle.Chef }
                };

                foreach (var kvp in keywordMap)
                {
                    if (normalized.Contains(kvp.Key))
                    {
                        var keywordMatch = country.Jobs.FirstOrDefault(job => job.Job == kvp.Value);
                        return (keywordMatch, keywordMatch != null);
                    }
                }
            }

            return (null, false);
        }

        private static IEnumerable<JobTitle> GetRelevantJobTitles(JobCategoryType category, FieldCategoryType field)
        {
            var titles = new List<JobTitle>();

            switch (category)
            {
                case JobCategoryType.IT:
                    titles.AddRange(new[] { JobTitle.SoftwareDeveloper, JobTitle.NetworkSecuritySpecialist, JobTitle.DataScientist });
                    break;
                case JobCategoryType.Engineering:
                    titles.AddRange(new[] { JobTitle.MechanicalEngineer, JobTitle.ElectricalEngineer, JobTitle.CivilEngineer, JobTitle.ProjectManager });
                    break;
                case JobCategoryType.Healthcare:
                    titles.Add(JobTitle.Nurse);
                    break;
                case JobCategoryType.Education:
                    titles.Add(JobTitle.Teacher);
                    break;
                case JobCategoryType.Finance:
                case JobCategoryType.Business:
                    titles.AddRange(new[] { JobTitle.Accountant, JobTitle.BusinessAnalyst, JobTitle.ProjectManager, JobTitle.HumanResourcesManager, JobTitle.MarketingSpecialist });
                    break;
                case JobCategoryType.Science:
                    titles.AddRange(new[] { JobTitle.DataScientist, JobTitle.Researcher });
                    break;
                case JobCategoryType.Arts:
                    titles.Add(JobTitle.Chef);
                    break;
                case JobCategoryType.Hospitality:
                    titles.Add(JobTitle.Chef);
                    break;
                case JobCategoryType.Legal:
                    titles.Add(JobTitle.BusinessAnalyst);
                    break;
            }

            if (titles.Count == 0)
            {
                titles.Add(JobTitle.ProjectManager);
            }

            if (field == FieldCategoryType.IT && !titles.Contains(JobTitle.SoftwareDeveloper))
            {
                titles.Add(JobTitle.SoftwareDeveloper);
            }

            if (field == FieldCategoryType.Engineering)
            {
                titles.Add(JobTitle.MechanicalEngineer);
                titles.Add(JobTitle.CivilEngineer);
                titles.Add(JobTitle.ElectricalEngineer);
            }

            if (field == FieldCategoryType.Medicine)
            {
                titles.Add(JobTitle.Nurse);
            }

            return titles;
        }

        private static EducationInfo? FindBestEducation(CountryProfile country, ImmigrationInput input)
        {
            if (country.Educations.Count == 0)
            {
                return null;
            }

            var preferredFields = GetRelevantFields(input.FieldCategory);
            var match = country.Educations
                .Where(edu => preferredFields.Contains(edu.Field))
                .OrderByDescending(edu => edu.Score)
                .FirstOrDefault();

            return match ?? country.Educations.OrderByDescending(edu => edu.Score).FirstOrDefault();
        }

        private static HashSet<FieldName> GetRelevantFields(FieldCategoryType field)
        {
            var fields = new HashSet<FieldName>();

            switch (field)
            {
                case FieldCategoryType.IT:
                    fields.UnionWith(new[] { FieldName.SoftwareEngineering, FieldName.DataScience });
                    break;
                case FieldCategoryType.Engineering:
                    fields.UnionWith(new[]
                    {
                        FieldName.ElectricalAndElectronicEngineering,
                        FieldName.MechanicalEngineering,
                        FieldName.CivilEngineering,
                        FieldName.ProjectManagement
                    });
                    break;
                case FieldCategoryType.Medicine:
                    fields.Add(FieldName.HealthSciencesAndNursing);
                    break;
                case FieldCategoryType.Science:
                    fields.UnionWith(new[] { FieldName.DataScience, FieldName.PureSciencesAndResearch });
                    break;
                case FieldCategoryType.Business:
                    fields.UnionWith(new[]
                    {
                        FieldName.BusinessAnalysisAndManagement,
                        FieldName.ProjectManagement,
                        FieldName.AccountingAndFinance,
                        FieldName.MarketingAndDigitalMarketing,
                        FieldName.HumanResourceManagement
                    });
                    break;
                case FieldCategoryType.Arts:
                    fields.UnionWith(new[] { FieldName.ArtsAndDesign, FieldName.TourismAndHospitality });
                    break;
            }

            return fields.Count == 0
                ? new HashSet<FieldName>(Enum.GetValues(typeof(FieldName)).Cast<FieldName>())
                : fields;
        }

        private static double CalculateLanguageComponent(LanguageCertificateType certificate, CountryType country, EducationInfo? education)
        {
            double baseScore = certificate switch
            {
                LanguageCertificateType.IELTS => 0.9,
                LanguageCertificateType.TOEFL => 0.85,
                LanguageCertificateType.GermanC2 => 1,
                LanguageCertificateType.GermanC1 => 0.95,
                LanguageCertificateType.GermanB2 => 0.85,
                LanguageCertificateType.GermanB1 => 0.7,
                LanguageCertificateType.GermanA2 => 0.5,
                LanguageCertificateType.None => 0.3,
                _ => 0.6
            };

            if (country == CountryType.Germany)
            {
                baseScore = certificate switch
                {
                    LanguageCertificateType.GermanC2 => 1,
                    LanguageCertificateType.GermanC1 => 0.95,
                    LanguageCertificateType.GermanB2 => 0.9,
                    LanguageCertificateType.GermanB1 => 0.75,
                    _ => baseScore * 0.8
                };
            }

            if (education != null && !string.IsNullOrWhiteSpace(education.LanguageRequirement))
            {
                baseScore = Math.Min(1, baseScore + 0.05);
            }

            return Math.Clamp(baseScore, 0, 1);
        }

        private static double CalculateInvestmentComponent(decimal amount, decimal countryMinimum)
        {
            if (amount <= 0)
            {
                return 0;
            }

            if (countryMinimum > 0)
            {
                return Math.Clamp((double)(amount / countryMinimum), 0, 1);
            }

            if (amount < 50000)
            {
                return 0.35;
            }

            if (amount < 100000)
            {
                return 0.6;
            }

            return 1;
        }

        private static double CalculatePersonalityComponent(PersonalityType personality, CountryProfile country)
        {
            if (personality == PersonalityType.Unknown || country.SuitablePersonalities.Count == 0)
            {
                return 0.45;
            }

            return country.SuitablePersonalities.Contains(personality) ? 1 : 0.65;
        }

        private static string? BuildPersonalityReport(PersonalityType personality, CountryProfile country)
        {
            if (personality == PersonalityType.Unknown || country.SuitablePersonalities.Count == 0)
            {
                return null;
            }

            return country.SuitablePersonalities.Contains(personality)
                ? "تیپ شخصیتی شما با فرهنگ و سبک زندگی این کشور همخوانی دارد."
                : "تیپ شخصیتی شما متفاوت است اما می‌توانید با آمادگی فرهنگی، موفق باشید.";
        }
    }
}
