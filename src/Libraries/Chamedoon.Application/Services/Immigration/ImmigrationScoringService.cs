using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
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

        [Phone]
        [StringLength(32)]
        public string? PhoneNumber { get; set; }

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
        private readonly IReadOnlyList<CountryProfile> _countries;

        public ImmigrationScoringService()
        {
            _countries = new List<CountryProfile>
            {
                BuildCountryProfile(CountryType.Canada, new Canada()),
                BuildCountryProfile(CountryType.Australia, new Australia()),
                BuildCountryProfile(CountryType.Germany, new Germany()),
                BuildCountryProfile(CountryType.USA, new USA()),
                BuildCountryProfile(CountryType.Netherlands, new Netherlands()),
                BuildCountryProfile(CountryType.Spain, new Spain()),
                BuildCountryProfile(CountryType.Sweden, new Sweden()),
                BuildCountryProfile(CountryType.Italy, new Italy()),
                BuildCountryProfile(CountryType.Oman, new Oman())
            };
        }

        public ImmigrationResult CalculateImmigration(ImmigrationInput input)
        {
            var recommendations = _countries
                .Select(country => BuildRecommendation(country, input))
                .OrderByDescending(item => item.Score)
                .Take(3)
                .ToList();

            return new ImmigrationResult { TopCountries = recommendations };
        }

        private static CountryProfile BuildCountryProfile(CountryType country, object source)
        {
            return new CountryProfile(
                country,
                GetPropertyValue(source, nameof(Canada.AgeScores), new Dictionary<int, string>()),
                GetPropertyValue(source, nameof(Canada.Jobs), new List<JobInfo>()),
                GetPropertyValue(source, nameof(Canada.Educations), new List<EducationInfo>()),
                GetPropertyValue(source, nameof(Canada.InvestmentAmount), 0m),
                GetPropertyValue(source, nameof(Canada.InvestmentNotes), string.Empty),
                GetPropertyValue(source, nameof(Canada.AdditionalInfo), string.Empty),
                GetPropertyValue(source, nameof(Canada.MaritalStatusImpact), string.Empty),
                GetPropertyValue(source, nameof(Canada.IranianMigrationRestrictions), new List<string>()),
                GetPropertyValue(source, nameof(Canada.LivingCosts), new MinimumLivingCosts { Housing = new List<HousingCost>() }),
                GetPropertyValue(source, nameof(Canada.SuitablePersonalities), new List<PersonalityType>()));
        }

        private CountryRecommendation BuildRecommendation(CountryProfile country, ImmigrationInput input)
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

            return new CountryRecommendation
            {
                Country = country.Country,
                Score = Math.Clamp(Math.Round(total, 2), 0, 100),
                RecommendedVisaType = recommendedVisa,
                Data = new CountryDataCard
                {
                    MaritalStatusImpact = country.MaritalStatusImpact,
                    IranianMigrationRestrictions = country.IranianMigrationRestrictions,
                    InvestmentNotes = country.InvestmentNotes,
                    InvestmentAmount = country.InvestmentAmount,
                    AdditionalInfo = country.AdditionalInfo,
                    LivingCosts = country.LivingCosts,
                    Job = BuildJobCard(jobMatch, input),
                    Education = educationCard,
                    PersonalityReport = BuildPersonalityReport(input.MBTIPersonality, country),
                    LanguageRequirement = educationCard?.LanguageRequirement
                }
            };
        }

        private static T GetPropertyValue<T>(object source, string propertyName, T @default)
        {
            var value = source.GetType().GetProperty(propertyName)?.GetValue(source);
            return value is T typed ? typed : @default;
        }

        private static string ChooseVisa(CountryProfile country, ImmigrationInput input, double investmentScore)
        {
            if (input.WillingToStudy || input.VisaType == VisaType.Study)
            {
                return "ویزای تحصیلی";
            }

            if (country.InvestmentAmount > 0 && investmentScore >= 1)
            {
                return "ویزای سرمایه‌گذاری یا استارتاپ";
            }

            if (input.VisaType == VisaType.Family && input.MaritalStatus == MaritalStatusType.Married)
            {
                return "ویزای خانواده";
            }

            if (input.VisaType == VisaType.Residence)
            {
                return "اقامت یا اقامت دائم";
            }

            if (input.WorkExperienceYears > 0)
            {
                return "ویزای کاری";
            }

            return "ویزای توریستی یا کوتاه‌مدت";
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
