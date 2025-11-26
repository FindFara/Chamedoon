using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Chamedoon.Application.Services.Immigration.Scoring;

namespace Chamedoon.Application.Services.Immigration
{
    /// <summary>
    /// ورودی فرم ارزیابی مهاجرت که به الگوریتم امتیازدهی ارسال می‌شود.
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

        /// <summary>
        /// نوع الگوریتم امتیازدهی. کاربرانی که پلن هوش مصنوعی دارند می‌توانند گزینه AI را انتخاب کنند.
        /// </summary>
        public ScoringAlgorithmType ScoringAlgorithm { get; set; } = ScoringAlgorithmType.Standard;

        public bool WillingToStudy { get; set; }
        public PersonalityType MBTIPersonality { get; set; } = PersonalityType.Unknown;

        [Phone]
        [StringLength(32)]
        public string? PhoneNumber { get; set; }

        [StringLength(DefaultTextLength)]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// خروجی امتیاز هر کشور.
    /// </summary>
    public class CountryRecommendation
    {
        public CountryType Country { get; set; }
        public double Score { get; set; }
        public string RecommendedVisaType { get; set; } = string.Empty;
        public string Conditions { get; set; } = string.Empty;
        public string? PersonalityReport { get; set; }
        public string? JobInfo { get; set; }
        public string? EducationInfo { get; set; }
        public string? EconomyInfo { get; set; }
    }

    /// <summary>
    /// خروجی نهایی شامل ده کشور برتر و گزینه نمایش سه کشور اول.
    /// </summary>
    public class ImmigrationResult
    {
        public List<CountryRecommendation> RankedCountries { get; set; } = new();
        public List<CountryRecommendation> TopCountries => RankedCountries.Take(3).ToList();
        public string? AiPrompt { get; set; }
        public string? AiSummary { get; set; }
        public ScoringAlgorithmType AlgorithmUsed { get; set; } = ScoringAlgorithmType.Standard;
    }

    /// <summary>
    /// سرویس محاسبه امتیاز مهاجرت. هر فاکتور امتیازدهی در کلاس جداگانه پیاده شده است
    /// تا بتوان با داده‌های به‌روز هر کشور وزن‌دهی کرد. دو الگوریتم در دسترس است: استاندارد و نسخه
    /// تقویت شده با هوش مصنوعی که هم‌پوشانی بین فاکتورهای شغلی و تحصیلی را بیشتر در نظر می‌گیرد.
    /// </summary>
    public class ImmigrationScoringService
    {
        private readonly List<ICountryScoreComponent> _standardComponents;
        private readonly List<ICountryScoreComponent> _aiComponents;

        private static readonly Dictionary<PersonalityType, List<CountryType>> PersonalityCountryPreferences = new()
        {
            [PersonalityType.ENFP] = new() { CountryType.Spain, CountryType.Switzerland, CountryType.Oman, CountryType.Canada, CountryType.NewZealand, CountryType.Netherlands },
            [PersonalityType.ENFJ] = new() { CountryType.Switzerland, CountryType.Spain, CountryType.Oman, CountryType.UK, CountryType.Canada },
            [PersonalityType.ESFP] = new() { CountryType.Spain, CountryType.Oman, CountryType.Italy, CountryType.NewZealand },
            [PersonalityType.ESFJ] = new() { CountryType.Spain, CountryType.Oman, CountryType.Italy, CountryType.Canada },
            [PersonalityType.ENTP] = new() { CountryType.USA, CountryType.Germany, CountryType.Netherlands, CountryType.Australia, CountryType.Sweden },
            [PersonalityType.ENTJ] = new() { CountryType.USA, CountryType.Germany, CountryType.Switzerland, CountryType.UK, CountryType.Netherlands },
            [PersonalityType.ESTP] = new() { CountryType.USA, CountryType.Australia, CountryType.Canada, CountryType.Switzerland },
            [PersonalityType.ESTJ] = new() { CountryType.USA, CountryType.UK, CountryType.Switzerland, CountryType.Germany },
            [PersonalityType.INFP] = new() { CountryType.Italy, CountryType.Sweden, CountryType.Switzerland, CountryType.NewZealand },
            [PersonalityType.INFJ] = new() { CountryType.Italy, CountryType.Sweden, CountryType.Switzerland, CountryType.Netherlands },
            [PersonalityType.ISFP] = new() { CountryType.Italy, CountryType.Oman, CountryType.Spain, CountryType.NewZealand },
            [PersonalityType.ISFJ] = new() { CountryType.Italy, CountryType.Spain, CountryType.Oman, CountryType.NewZealand },
            [PersonalityType.INTP] = new() { CountryType.Germany, CountryType.Netherlands, CountryType.Sweden, CountryType.Switzerland },
            [PersonalityType.INTJ] = new() { CountryType.Germany, CountryType.UK, CountryType.Italy, CountryType.Switzerland },
            [PersonalityType.ISTP] = new() { CountryType.Germany, CountryType.Canada, CountryType.Sweden },
            [PersonalityType.ISTJ] = new() { CountryType.Germany, CountryType.Canada, CountryType.UK }
        };

        private static readonly Dictionary<CountryType, string> CountryPersonalityFeatures = new()
        {
            [CountryType.Spain] = "فرهنگ گرم و اجتماعی و روحیه جشن و شادی",
            [CountryType.Switzerland] = "نظم و رفاه اجتماعی بالا و طبیعت زیبا",
            [CountryType.Oman] = "مردم مهمان‌نواز و فرصت‌های اقتصادی در حال رشد",
            [CountryType.Canada] = "تنوع فرهنگی و فضای دوستانه",
            [CountryType.NewZealand] = "طبیعت بی‌نظیر و آرامش زندگی",
            [CountryType.Netherlands] = "فرهنگ خلاق و باز و فرصت‌های استارتاپ",
            [CountryType.UK] = "تاریخ و فرهنگ غنی و سیستم آموزشی پیشرفته",
            [CountryType.Germany] = "نظم و ساختار و فرصت‌های مهندسی",
            [CountryType.Australia] = "فضای باز و فرصت‌های متعدد کاری",
            [CountryType.Italy] = "هنر و فرهنگ خانواده‌محور",
            [CountryType.Sweden] = "رفاه اجتماعی و محیط طبیعی آرام",
            [CountryType.Denmark] = "نوآوری و کیفیت زندگی بالا",
            [CountryType.USA] = "تنوع اقتصادی و فرصت‌های بی‌شمار"
        };

        private static readonly Dictionary<CountryType, (string Job, string Education, string Economy)> CountryExtraInfo = new()
        {
            [CountryType.Canada] = ("بازار کار در سلامت، فناوری و مهندسی فعال است.", "دانشگاه‌های معتبر و مسیرهای مهاجرت تحصیلی گسترده دارد.", "درآمد متوسط بالاست و هزینه زندگی متعادل‌تر از اروپای غربی است."),
            [CountryType.Australia] = ("نیاز به نیروی ماهر در فناوری، معدن و مراقبت‌های درمانی.", "دانشگاه‌های سیدنی و ملبورن گزینه‌های محبوب دانشجویان بین‌المللی هستند.", "اقتصاد پایدار و دستمزد مناسب اما شهرهای بزرگ گران هستند."),
            [CountryType.Germany] = ("کمبود نیروی متخصص در مهندسی و IT محسوس است.", "تحصیل ارزان در دانشگاه‌های دولتی با شرط زبان آلمانی.", "اقتصاد قدرتمند اتحادیه اروپا و بازار کار ساختاریافته."),
            [CountryType.USA] = ("بزرگ‌ترین اقتصاد جهان با فرصت‌های فراوان در فناوری و مالی.", "دانشگاه‌های تراز اول ولی شهریه بالا.", "درآمد بالا همراه با هزینه متغیر در ایالت‌ها."),
            [CountryType.UK] = ("تقاضا در سلامت، مهندسی و فناوری همچنان بالاست.", "سیستم آموزشی قدیمی با دانشگاه‌های معتبر.", "بازار کار رقابتی و هزینه زندگی در لندن بالاست."),
            [CountryType.NewZealand] = ("به متخصصین کشاورزی، مهندسی و سلامت نیاز دارد.", "محیط آموزشی دوستانه و انگلیسی‌زبان.", "اقتصاد کوچک ولی پایدار با کیفیت زندگی خوب."),
            [CountryType.Netherlands] = ("فرصت‌های استارتاپ و کشاورزی پیشرفته.", "دوره‌های متعدد به زبان انگلیسی ارائه می‌شود.", "اقتصاد صادرات‌محور و هزینه زندگی متوسط رو به بالا."),
            [CountryType.Spain] = ("گردشگری، IT و انرژی‌های نو فرصت ایجاد کرده‌اند.", "دانشگاه‌های عمومی مقرون‌به‌صرفه برای اروپا.", "هزینه زندگی در جنوب پایین‌تر از مادرید و بارسلون است."),
            [CountryType.Sweden] = ("متخصصان فناوری و محیط زیست مزیت دارند.", "تحصیل انگلیسی‌زبان در مقطع ارشد و PhD رایج است.", "رفاه اجتماعی بالا هزینه‌ها را جبران می‌کند."),
            [CountryType.Denmark] = ("نیازمند مهندسان انرژی و IT است.", "دانشگاه‌های نوآور و برنامه‌های کارآفرینی فعال.", "اقتصاد کوچک اما با درآمد و مالیات بالا."),
            [CountryType.Italy] = ("طراحی، گردشگری و تولید صنعتی مزیت کشور است.", "دانشگاه‌های دولتی شهریه مناسبی دارند.", "هزینه زندگی در شمال بالاتر از جنوب است."),
            [CountryType.Switzerland] = ("مالی، داروسازی و فناوری پیشرفته ستون‌های بازار کار هستند.", "دانشگاه‌های سطح جهانی با شهریه و هزینه زندگی بالا.", "درآمد و کیفیت زندگی بسیار بالا است."),
            [CountryType.Oman] = ("پروژه‌های عمرانی و انرژی در حال گسترش است.", "برنامه‌های دانشگاهی انگلیسی رو به رشد هستند.", "مالیات کم و هزینه زندگی پایین‌تر از اروپا است.")
        };

        public ImmigrationScoringService()
        {
            _standardComponents = new List<ICountryScoreComponent>
            {
                new AgeScoreComponent(12),
                new EducationScoreComponent(18),
                new ExperienceScoreComponent(12),
                new JobMarketFitScoreComponent(12),
                new LanguageScoreComponent(14),
                new InvestmentScoreComponent(8),
                new StudyPreferenceScoreComponent(6),
                new VisaPreferenceScoreComponent(6),
                new PersonalityScoreComponent(PersonalityCountryPreferences, 6),
                new ComplianceScoreComponent(6)
            };

            _aiComponents = new List<ICountryScoreComponent>
            {
                new AgeScoreComponent(10),
                new EducationScoreComponent(19, emphasizeHighTech: true),
                new ExperienceScoreComponent(14, rewardDepth: true),
                new JobMarketFitScoreComponent(15),
                new LanguageScoreComponent(15),
                new InvestmentScoreComponent(7),
                new StudyPreferenceScoreComponent(5, boostIfWilling: true),
                new VisaPreferenceScoreComponent(5, flexible: true),
                new PersonalityScoreComponent(PersonalityCountryPreferences, 5),
                new ComplianceScoreComponent(5)
            };
        }

        public ImmigrationResult CalculateImmigration(ImmigrationInput input)
        {
            var context = new CountryScoreContext(input);
            var components = input.ScoringAlgorithm == ScoringAlgorithmType.AiEnhanced ? _aiComponents : _standardComponents;

            var recommendations = new List<CountryRecommendation>();
            foreach (var country in Enum.GetValues(typeof(CountryType)).Cast<CountryType>())
            {
                double score = 0;
                foreach (var component in components)
                {
                    score += component.CalculateScore(country, context);
                }

                score = Math.Round(Math.Min(score, 100), 1);
                var extra = GetCountryExtraInfo(country);
                recommendations.Add(new CountryRecommendation
                {
                    Country = country,
                    Score = score,
                    RecommendedVisaType = GetVisaRecommendation(country, input.VisaType),
                    Conditions = GetCountryConditions(country),
                    PersonalityReport = GetPersonalityExplanation(input.MBTIPersonality, country),
                    JobInfo = extra.Job,
                    EducationInfo = extra.Education,
                    EconomyInfo = extra.Economy
                });
            }

            var ranked = recommendations
                .OrderByDescending(r => r.Score)
                .ThenBy(r => r.Country.ToString())
                .Take(10)
                .ToList();

            return new ImmigrationResult
            {
                RankedCountries = ranked,
                AiPrompt = input.ScoringAlgorithm == ScoringAlgorithmType.AiEnhanced ? BuildAiPrompt(input) : null,
                AiSummary = input.ScoringAlgorithm == ScoringAlgorithmType.AiEnhanced ? BuildAiSummary(ranked) : null,
                AlgorithmUsed = input.ScoringAlgorithm
            };
        }

        private string? GetPersonalityExplanation(PersonalityType type, CountryType country)
        {
            if (type == PersonalityType.Unknown)
            {
                return null;
            }

            if (!CountryPersonalityFeatures.TryGetValue(country, out var feature))
            {
                return null;
            }

            return $"تیپ شخصیتی {type} معمولاً با ویژگی‌های {feature} در {country.GetDescription()} هماهنگ است.";
        }

        private (string Job, string Education, string Economy) GetCountryExtraInfo(CountryType country)
        {
            return CountryExtraInfo.TryGetValue(country, out var info)
                ? info
                : ("اطلاعات بازار کار در دسترس نیست.", "اطلاعات تحصیلی در دسترس نیست.", "اطلاعات اقتصادی در دسترس نیست.");
        }

        private string GetVisaRecommendation(CountryType country, VisaType requested)
        {
            return country switch
            {
                CountryType.Canada => requested switch
                {
                    VisaType.Study => "ویزای تحصیلی کانادا",
                    VisaType.Investment => "برنامه سرمایه‌گذاری/استارتاپ",
                    VisaType.Residence => "اکسپرس اینتری یا PNP",
                    VisaType.Family => "ویزای الحاق خانواده",
                    _ => "ویزای کاری (Express Entry)"
                },
                CountryType.Australia => requested switch
                {
                    VisaType.Study => "ویزای دانشجویی استرالیا",
                    VisaType.Residence => "ویزای مهارتی 189",
                    _ => "ویزای کار مهارتی"
                },
                CountryType.Germany => requested switch
                {
                    VisaType.Study => "ویزای تحصیلی یا زبان آلمانی",
                    VisaType.Residence => "کارت فرصت آلمان",
                    VisaType.Startup => "ویزای استارتاپ/نوآفرینی",
                    _ => "ویزای جستجوی کار"
                },
                CountryType.UK => requested switch
                {
                    VisaType.Study => "ویزای دانشجویی",
                    VisaType.Family => "ویزای پیوست خانواده",
                    _ => "Skilled Worker Visa"
                },
                CountryType.USA => requested switch
                {
                    VisaType.Research => "ویزای J1/F1",
                    VisaType.Startup => "ویزای استارتاپ/سرمایه‌گذاری",
                    _ => "ویزای تخصصی H-1B"
                },
                CountryType.Netherlands => requested switch
                {
                    VisaType.Startup => "ویزای استارتاپ",
                    VisaType.DigitalNomad => "ویزای کار از راه دور",
                    _ => "ویزای نیروی متخصص"
                },
                CountryType.NewZealand => requested switch
                {
                    VisaType.Study => "ویزای دانشجویی",
                    VisaType.Residence => "Skilled Migrant",
                    _ => "ویزای کار موقت"
                },
                CountryType.Sweden => requested switch
                {
                    VisaType.Startup => "ویزای استارتاپ سوئد",
                    VisaType.Study => "ویزای تحصیلی",
                    _ => "ویزای نیروی متخصص"
                },
                CountryType.Denmark => requested switch
                {
                    VisaType.Startup => "Start-up Denmark",
                    _ => "ویزای کاری/تحصیلی"
                },
                CountryType.Spain => requested switch
                {
                    VisaType.DigitalNomad => "ویزای دیجیتال نوماد اسپانیا",
                    VisaType.Investment => "ویزای طلایی اسپانیا",
                    _ => "ویزای کار/تحصیل"
                },
                CountryType.Italy => requested switch
                {
                    VisaType.Investment => "ویزای طلایی ایتالیا",
                    VisaType.Study => "ویزای تحصیلی",
                    _ => "ویزای کار ایتالیا"
                },
                CountryType.Switzerland => requested switch
                {
                    VisaType.Investment => "ویزای سرمایه‌گذاری یا اقامت مالیاتی",
                    _ => "اجازه کار (Permit L/B)"
                },
                CountryType.Oman => requested switch
                {
                    VisaType.Work => "ویزای کار عمان",
                    VisaType.Investment => "ویزای سرمایه‌گذاری عمان",
                    _ => "ویزای خانواده/دانشجویی"
                },
                _ => "ویزای کاری"
            };
        }

        private string GetCountryConditions(CountryType country)
        {
            return country switch
            {
                CountryType.Canada => "سن زیر ۴۵ سال، مهارت زبان انگلیسی/فرانسوی و سابقه کار تخصصی امتیاز اصلی را تشکیل می‌دهد.",
                CountryType.Australia => "امتیازدهی بر پایه سن زیر ۴۵ سال، مهارت زبان و سابقه مرتبط است.",
                CountryType.Germany => "کارت فرصت آلمان به سن زیر ۳۵، تجربه کاری و مدرک دانشگاهی امتیاز می‌دهد.",
                CountryType.UK => "پیشنهاد شغلی تایید شده، سطح حقوق و مهارت زبان برای سیستم امتیازدهی الزامی است.",
                CountryType.USA => "پیشنهاد کار معتبر یا سرمایه‌گذاری قابل توجه مسیر اصلی دریافت ویزاست.",
                CountryType.Netherlands => "نیروی متخصص با حقوق حداقلی مشخص یا طرح استارتاپ اولویت دارد.",
                CountryType.NewZealand => "سن زیر ۵۵ سال و سابقه مرتبط برای ویزای مهارتی نیاز است.",
                CountryType.Sweden => "قرارداد کاری یا پذیرش دانشگاهی و منابع مالی ثابت لازم است.",
                CountryType.Denmark => "پیشنهاد کار در لیست مشاغل موردنیاز یا طرح استارتاپی نوآورانه امتیاز می‌گیرد.",
                CountryType.Spain => "برای ویزای طلایی سرمایه‌گذاری و برای ویزای کار پیشنهاد شغلی و زبان لازم است.",
                CountryType.Italy => "پیشنهاد شغلی معتبر یا پذیرش تحصیلی و اثبات تمکن مالی الزامی است.",
                CountryType.Switzerland => "بازار کار رقابتی است و پیشنهاد شغلی یا سرمایه کافی نیاز دارد.",
                CountryType.Oman => "اسپانسر شغلی عمانی یا سرمایه برای راه‌اندازی کسب‌وکار اهمیت دارد.",
                _ => string.Empty
            };
        }

        private string BuildAiPrompt(ImmigrationInput input)
        {
            var builder = new StringBuilder();
            builder.AppendLine("الگوریتم هوش مصنوعی ارزیابی مهاجرت");
            builder.AppendLine($"سن: {input.Age}");
            builder.AppendLine($"شغل: {input.JobCategory} / {input.JobTitle}");
            builder.AppendLine($"سابقه کاری: {input.WorkExperienceYears} سال");
            builder.AppendLine($"تحصیلات: {input.DegreeLevel} در حوزه {input.FieldCategory}");
            builder.AppendLine($"مدرک زبان: {input.LanguageCertificate}");
            builder.AppendLine($"سرمایه: {input.InvestmentAmount}");
            builder.AppendLine($"تمایل به تحصیل: {input.WillingToStudy}");
            builder.AppendLine($"تیپ شخصیتی: {input.MBTIPersonality}");
            builder.AppendLine($"نوع ویزای مدنظر: {input.VisaType}");
            builder.Append("براساس داده تقاضای کشورها، ده کشور را از ۱۰۰ امتیازدهی کن و سه گزینه اول را برجسته کن.");
            return builder.ToString();
        }

        private string? BuildAiSummary(IReadOnlyList<CountryRecommendation> ranked)
        {
            if (ranked.Count == 0)
            {
                return null;
            }

            var top = ranked.Take(3).Select(item => $"{item.Country.GetDescription()} ({item.Score}%)");
            var runnerUps = ranked.Skip(3).Select(item => item.Country.GetDescription()).ToList();
            var summary = new StringBuilder();
            summary.AppendLine("خلاصه خروجی هوش مصنوعی:");
            summary.AppendLine($"گزینه‌های اول: {string.Join("، ", top)}");
            if (runnerUps.Any())
            {
                summary.AppendLine($"گزینه‌های بعدی برای بررسی: {string.Join("، ", runnerUps)}");
            }

            return summary.ToString();
        }
    }
}
