using System;
using System.Collections.Generic;
using System.Linq;

namespace Chamedoon.Application.Services.Immigration
{
    /// <summary>
    /// Represents the input information provided by a user for scoring immigration
    /// eligibility. The properties mirror typical questions asked by online
    /// immigration portals. All string properties are optional and should be
    /// provided in Persian to match the rest of the website.
    /// </summary>
    public class ImmigrationInput
    {
        /// <summary>
        /// سن متقاضی (به سال). مقدار باید عددی مثبت باشد.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// عنوان شغل فعلی متقاضی. این مقدار اختیاری است و می‌تواند جزئیات دقیق‌تر
        /// مانند عنوان پوزیشن یا تخصص را شامل شود. اگر شغل ندارد این فیلد خالی بماند.
        /// </summary>
        public string? JobTitle { get; set; }

        /// <summary>
        /// دسته‌بندی کلی شغل. این فیلد باید از Enum انتخاب شود و یک گزینه برای
        /// افراد فاقد شغل (None) دارد.
        /// </summary>
        public JobCategoryType JobCategory { get; set; } = JobCategoryType.None;

        /// <summary>
        /// سابقه کاری به صورت تعداد سال. صفر برای افراد بدون سابقه.
        /// سابقه کاری تأثیر مستقیمی در امتیاز کشورهایی دارد که تجربه را
        /// می‌سنجند.
        /// </summary>
        public int WorkExperienceYears { get; set; }

        /// <summary>
        /// دسته‌بندی رشته یا زمینه تحصیل/شغل. به جای رشته آزاد، از این
        /// دسته‌بندی‌های استاندارد استفاده می‌شود تا با الگوریتم امتیازدهی
        /// هم‌خوان باشد.
        /// </summary>
        public FieldCategoryType FieldCategory { get; set; }

        /// <summary>
        /// مقطع تحصیلی. از Enum استفاده می‌شود تا از خطاهای ورودی جلوگیری
        /// شود.
        /// </summary>
        public DegreeLevelType DegreeLevel { get; set; }

        /// <summary>
        /// نوع مدرک یا سطح زبان. سطح زبان در امتیاز کشورها به ویژه آلمان
        /// بسیار تأثیرگذار است.
        /// </summary>
        public LanguageCertificateType LanguageCertificate { get; set; }

        /// <summary>
        /// نوع ویزای درخواستی. این گزینه تعیین کننده اولویت‌ها در
        /// پیشنهاد نوع ویزا برای هر کشور است.
        /// </summary>
        public VisaType VisaType { get; set; }

        /// <summary>
        /// سابقه کیفری: اگر سابقه دارید مقدار true قرار دهید.
        /// سابقه کیفری باعث کاهش امتیاز می‌شود.
        /// </summary>
        public bool HasCriminalRecord { get; set; }

        /// <summary>
        /// میزان سرمایه بر حسب دلار یا یورو. صفر در صورت عدم سرمایه‌گذاری.
        /// سرمایه بالا می‌تواند مسیرهای سرمایه‌گذاری را باز کند.
        /// </summary>
        public decimal InvestmentAmount { get; set; }

        /// <summary>
        /// وضعیت تاهل. افراد متأهل در برخی کشورها امتیاز بیشتری دریافت می‌کنند.
        /// </summary>
        public MaritalStatusType MaritalStatus { get; set; }

        /// <summary>
        /// آیا متقاضی تمایل به ادامه تحصیل دارد؟ این فیلد در پیشنهاد
        /// ویزای تحصیلی مؤثر است.
        /// </summary>
        public bool WillingToStudy { get; set; }

        /// <summary>
        /// تیپ شخصیتی MBTI متقاضی. این مقدار اختیاری است و در صورت مشخص شدن،
        /// برای ایجاد تطبیق فرهنگی و توصیه‌های شخصیتی استفاده می‌شود. پیش‌فرض Unknown است.
        /// </summary>
        public PersonalityType MBTIPersonality { get; set; } = PersonalityType.Unknown;
    }

    /// <summary>
    /// یک توصیه برای کشور مقصد شامل امتیاز نهایی، نوع ویزای پیشنهادی و خلاصه‌ای از شرایط.
    /// </summary>
    public class CountryRecommendation
    {
        public CountryType Country { get; set; }
        public double Score { get; set; }
        public string RecommendedVisaType { get; set; } = string.Empty;
        public string Conditions { get; set; } = string.Empty;

        /// <summary>
        /// توضیحی درباره اینکه چرا این کشور برای تیپ شخصیتی متقاضی مناسب است.
        /// در صورتی که تیپ شخصیتی انتخاب نشده باشد، این فیلد می‌تواند خالی بماند.
        /// </summary>
        public string? PersonalityReport { get; set; }

        /// <summary>
        /// توضیحی در مورد بازار کار، صنایع پررونق و فرصت‌های شغلی این کشور. این متن
        /// برای نمایش در گزارش نهایی استفاده می‌شود.
        /// </summary>
        public string? JobInfo { get; set; }

        /// <summary>
        /// توضیحی در مورد سیستم آموزشی، کیفیت دانشگاه‌ها و شرایط زندگی و تحصیل
        /// در این کشور.
        /// </summary>
        public string? EducationInfo { get; set; }

        /// <summary>
        /// توضیحی در مورد وضعیت اقتصادی، متوسط درآمد و هزینه‌های زندگی این کشور.
        /// </summary>
        public string? EconomyInfo { get; set; }
    }

    /// <summary>
    /// خروجی محاسبه امتیاز که شامل سه کشور برتر می‌باشد.
    /// </summary>
    public class ImmigrationResult
    {
        public List<CountryRecommendation> TopCountries { get; set; } = new();
    }

    /// <summary>
    /// سرویس محاسبه امتیاز مهاجرت. این کلاس بر اساس فاکتورهای مختلف امتیازدهی
    /// می‌کند و سه کشور مناسب (کانادا، استرالیا و آلمان) را پیشنهاد می‌دهد. وزن‌ها
    /// و ضرایب از سیستم‌های امتیازدهی رسمی این کشورها الهام گرفته شده‌اند اما برای
    /// استفاده عمومی ساده‌سازی شده‌اند.
    /// منابع: کانادا معیارهایی نظیر سن، تحصیلات، تجربه کاری و زبان را
    /// در سیستم امتیازدهی CRS در نظر می‌گیرد【778440806576490†L120-L160】. در استرالیا،
    /// سن، مهارت زبان انگلیسی و سابقه کاری در سیستم ویزای Subclass 189 ارزشمند
    /// هستند【994435611753471†L140-L176】. آلمان برای کارت فرصت (Chancenkarte) فاکتورهایی مانند
    /// سن، شناخت جزئی مدارک تحصیلی، تجربه کاری و مهارت زبانی را امتیازدهی می‌کند【214254149584883†L234-L259】.
    /// </summary>
    public class ImmigrationScoringService
    {
        /// <summary>
        /// نگاشت تیپ‌های شخصیتی به کشورهایی که از نظر فرهنگی و شخصیتی برای آن تیپ مناسب‌تر هستند.
        /// این داده‌ها برای تخصیص امتیاز اضافه در محاسبه نهایی استفاده می‌شوند. نوع CountryType
        /// استفاده می‌شود تا از خطاهای ناشی از نوشتار جلوگیری گردد.
        /// </summary>
        private static readonly Dictionary<PersonalityType, List<CountryType>> PersonalityCountryPreferences = new Dictionary<PersonalityType, List<CountryType>>
        {
            [PersonalityType.ENFP] = new List<CountryType> { CountryType.Spain, CountryType.Switzerland, CountryType.Oman, CountryType.Canada, CountryType.NewZealand, CountryType.Netherlands },
            [PersonalityType.ENFJ] = new List<CountryType> { CountryType.Switzerland, CountryType.Spain, CountryType.Oman, CountryType.UK, CountryType.Canada },
            [PersonalityType.ESFP] = new List<CountryType> { CountryType.Spain, CountryType.Oman, CountryType.Italy, CountryType.NewZealand },
            [PersonalityType.ESFJ] = new List<CountryType> { CountryType.Spain, CountryType.Oman, CountryType.Italy, CountryType.Canada },
            [PersonalityType.ENTP] = new List<CountryType> { CountryType.USA, CountryType.Germany, CountryType.Netherlands, CountryType.Australia, CountryType.Sweden },
            [PersonalityType.ENTJ] = new List<CountryType> { CountryType.USA, CountryType.Germany, CountryType.Switzerland, CountryType.UK, CountryType.Netherlands },
            [PersonalityType.ESTP] = new List<CountryType> { CountryType.USA, CountryType.Australia, CountryType.Canada, CountryType.Switzerland },
            [PersonalityType.ESTJ] = new List<CountryType> { CountryType.USA, CountryType.UK, CountryType.Switzerland, CountryType.Germany },
            [PersonalityType.INFP] = new List<CountryType> { CountryType.Italy, CountryType.Sweden, CountryType.Switzerland, CountryType.NewZealand },
            [PersonalityType.INFJ] = new List<CountryType> { CountryType.Italy, CountryType.Sweden, CountryType.Switzerland, CountryType.Netherlands },
            [PersonalityType.ISFP] = new List<CountryType> { CountryType.Italy, CountryType.Oman, CountryType.Spain, CountryType.NewZealand },
            [PersonalityType.ISFJ] = new List<CountryType> { CountryType.Italy, CountryType.Spain, CountryType.Oman, CountryType.NewZealand },
            [PersonalityType.INTP] = new List<CountryType> { CountryType.Germany, CountryType.Netherlands, CountryType.Sweden, CountryType.Switzerland },
            [PersonalityType.INTJ] = new List<CountryType> { CountryType.Germany, CountryType.UK, CountryType.Italy, CountryType.Switzerland },
            [PersonalityType.ISTP] = new List<CountryType> { CountryType.Germany, CountryType.Canada, CountryType.Sweden },
            [PersonalityType.ISTJ] = new List<CountryType> { CountryType.Germany, CountryType.Canada, CountryType.UK }
        };

        /// <summary>
        /// مقدار امتیاز اضافی برای کشوری که با تیپ شخصیتی متقاضی تطابق دارد را بازمی‌گرداند.
        /// </summary>
        private double GetPersonalityCountryBonus(PersonalityType type, CountryType country)
        {
            if (type == PersonalityType.Unknown)
                return 0;
            if (PersonalityCountryPreferences.TryGetValue(type, out var countries))
            {
                return countries.Contains(country) ? 2 : 0;
            }
            return 0;
        }

        /// <summary>
        /// ویژگی‌های عمومی کشورها به زبان فارسی. از این ویژگی‌ها در تولید توضیح شخصیتی استفاده می‌شود.
        /// </summary>
        private static readonly Dictionary<CountryType, string> CountryPersonalityFeatures = new Dictionary<CountryType, string>
        {
            [CountryType.Spain] = "فرهنگ گرم و اجتماعی، موسیقی و هنر و روحیه جشن و شادی",
            [CountryType.Switzerland] = "نظم و رفاه اجتماعی بالا، تعادل کار و زندگی و طبیعت زیبا",
            [CountryType.Oman] = "مردم مهمان‌نواز، فرهنگ سنتی و فرصت‌های اقتصادی در حال رشد",
            [CountryType.Canada] = "تنوع فرهنگی، طبیعت بکر و فضای دوستانه",
            [CountryType.NewZealand] = "طبیعت بی‌نظیر، رویکرد دوستانه و آرامش زندگی",
            [CountryType.Netherlands] = "فرهنگ خلاق و باز، فرصت‌های استارتاپ و نوآوری",
            [CountryType.UK] = "تاریخ و فرهنگ غنی، سیستم آموزشی پیشرفته و فرصت‌های کاری",
            [CountryType.Germany] = "نظم و ساختار، فرصت‌های مهندسی و تحقیقاتی و اقتصاد قوی",
            [CountryType.Australia] = "فضای باز و طبیعت متنوع، فرهنگ غیررسمی و فرصت‌های متعدد",
            [CountryType.Italy] = "هنر و معماری غنی، غذا و فرهنگ خانواده‌محور",
            [CountryType.Sweden] = "جامعه مترقی و رفاه اجتماعی، محیط طبیعی و آرام",
            [CountryType.Denmark] = "نظام رفاه و آموزش پیشرفته، نوآوری و کیفیت زندگی بالا",
            [CountryType.USA] = "فرصت‌های بی‌شمار شغلی و تحصیلی و تنوع فرهنگی"
        };

        /// <summary>
        /// اطلاعات تکمیلی درباره هر کشور شامل بازار کار، وضعیت تحصیل و اقتصاد. این متن‌ها به زبان فارسی
        /// نوشته شده‌اند و بر اساس منابع عمومی و داده‌های آماری تهیه شده‌اند. اعداد درآمد و شاخص هزینه
        /// زندگی از منابع معتبر مانند WorldData و Numbeo استخراج شده‌اند【838636707243709†L70-L74】【325730539419572†L118-L121】.
        /// </summary>
        private static readonly Dictionary<CountryType, (string Job, string Education, string Economy)> CountryExtraInfo = new()
        {
            [CountryType.Canada] = (
                Job: "بازار کار کانادا به نیروی متخصص در حوزه‌های بهداشت و درمان، فناوری اطلاعات، مهندسی و کشاورزی نیاز دارد؛ برنامه‌های اکسپرس انتری برای این دسته‌ها اولویت می‌دهد【947379369221691†L74-L131】.",
                Education: "سیستم آموزشی کانادا کیفیت بالا دارد و دانشگاه‌های معتبری مانند تورنتو، مک‌گیل و بریتیش کلمبیا شهریه‌ای نسبتاً بالا برای دانشجویان بین‌المللی دریافت می‌کنند.",
                Economy: "متوسط درآمد سالانه در کانادا حدود ۵۳٬۳۴۰ دلار آمریکا است【838636707243709†L72-L74】 و شاخص هزینه زندگی ۶۰٫۷ است【325730539419572†L118-L121】؛ خانواده چهار نفره ماهانه حدود ۳٬۲۳۰ یورو بدون اجاره هزینه دارد【411000377881144†L98-L104】."
            ),
            [CountryType.Australia] = (
                Job: "استرالیا تقاضای بالایی برای نیروهای ماهر در صنایع معدن، مهندسی، فناوری و مراقبت‌های بهداشتی دارد؛ ویزای نیروی ماهر Subclass 189 بر این پایه شکل گرفته است【994435611753471†L140-L176】.",
                Education: "دانشگاه‌های استرالیا مانند ملبورن و سیدنی کیفیت جهانی دارند ولی شهریه‌ها برای دانشجویان خارجی بالاست.",
                Economy: "متوسط درآمد سالانه حدود ۶۲٬۵۵۰ دلار آمریکا است【838636707243709†L66-L67】 و شاخص هزینه زندگی ۶۳٫۰ است【325730539419572†L116-L117】؛ شهرهای بزرگ مانند سیدنی هزینهٔ زندگی بالایی دارند."
            ),
            [CountryType.Germany] = (
                Job: "آلمان با کمبود نیروی کار در حوزه‌های مهندسی، فناوری اطلاعات، مراقبت‌های پزشکی و پرستاری روبه‌رو است؛ کارت فرصت آلمان بر این اساس ایجاد شده و به متقاضیان زیر ۳۵ سال با تجربه و مدرک دانشگاهی امتیاز می‌دهد【214254149584883†L234-L259】.",
                Education: "تحصیل در دانشگاه‌های دولتی آلمان برای بسیاری از رشته‌ها رایگان یا بسیار ارزان است؛ اما نیاز به مهارت زبان آلمانی دارید.",
                Economy: "متوسط درآمد سالانه ۵۴٬۹۶۰ دلار آمریکا است【838636707243709†L70-L71】 و شاخص هزینه زندگی ۶۴٫۷ است【325730539419572†L111-L112】؛ هزینه زندگی در شهرهای بزرگ مانند مونیخ بالا است ولی کیفیت زندگی و زیرساخت‌ها عالی هستند."
            ),
            [CountryType.USA] = (
                Job: "آمریکا بزرگ‌ترین اقتصاد جهان است و فرصت‌های شغلی متنوعی در فناوری، مالی، بهداشت و مهندسی دارد؛ ویزای H‑1B برای نیروی کار تخصصی استفاده می‌شود【964062238638662†L70-L116】.",
                Education: "دانشگاه‌های ایالات متحده مانند هاروارد و MIT از معتبرترین دانشگاه‌های جهان هستند اما شهریه‌ها بسیار بالا و رقابتی است.",
                Economy: "متوسط درآمد سالانه حدود ۸۳٬۶۶۰ دلار است【838636707243709†L56-L57】 و شاخص هزینه زندگی ۶۴٫۸ است【325730539419572†L110-L111】؛ تفاوت دستمزد و هزینه میان ایالت‌ها بسیار زیاد است."
            ),
            [CountryType.UK] = (
                Job: "بریتانیا نیازمند نیروهای ماهر در حوزه‌های بهداشت، مهندسی، فناوری و آموزش است؛ سیستم امتیازدهی ۷۰ امتیازی به پیشنهاد کار، سطح حقوق و زبان اهمیت می‌دهد【796060067679459†L245-L260】.",
                Education: "دانشگاه‌های بریتانیا مانند آکسفورد و کمبریج شهرت جهانی دارند ولی شهریه دانشجویان خارجی بالاست.",
                Economy: "متوسط درآمد سالانه حدود ۴۸٬۶۱۰ دلار است【838636707243709†L77-L78】 و شاخص هزینه زندگی ۶۴٫۲ است【325730539419572†L114-L115】؛ لندن از گران‌ترین شهرهای جهان است."
            ),
            [CountryType.NewZealand] = (
                Job: "نیوزیلند به نیروی متخصص در بخش‌های کشاورزی، مهندسی، فناوری، ساخت‌وساز و مراقبت‌های بهداشتی نیاز دارد؛ دسته مهاجرت ماهر Skilled Migrant بر این اساس تدوین شده.",
                Education: "دانشگاه‌های نیوزیلند کیفیت بالایی دارند و شهرهایی مانند اوکلند و ولینگتون میزبان دانشجویان بین‌المللی هستند.",
                Economy: "متوسط درآمد سالانه ۴۶٬۲۸۰ دلار آمریکا است【838636707243709†L78-L79】 و شاخص هزینه زندگی ۵۹٫۳ است【325730539419572†L118-L121】؛ هزینه زندگی در شهرها بالا اما محیط طبیعی زیبا و ایمن است."
            ),
            [CountryType.Netherlands] = (
                Job: "هلند به خاطر فرهنگ نوآورانه و استارتاپ‌ها، فرصت‌های زیادی در فناوری، انرژی‌های پاک و کشاورزی پیشرفته دارد.",
                Education: "بسیاری از دوره‌های دانشگاهی به زبان انگلیسی ارائه می‌شوند؛ تحصیل برای شهروندان اتحادیه اروپا ارزان‌تر است.",
                Economy: "متوسط درآمد سالانه ۶۲٬۸۴۰ دلار است【838636707243709†L65-L66】 و شاخص هزینه زندگی ۶۸٫۱ است【325730539419572†L106-L107】؛ کیفیت زندگی و توازن کار و زندگی در شهرهایی مانند آمستردام خوب است."
            ),
            [CountryType.Spain] = (
                Job: "اسپانیا بازار کار نسبتا رقابتی دارد اما فرصت‌هایی در گردشگری، کشاورزی و فناوری‌های نوظهور وجود دارد.",
                Education: "هزینه دانشگاه‌های عمومی برای شهروندان اتحادیه اروپا مناسب است و برنامه‌های جذاب برای دانشجویان خارجی وجود دارد.",
                Economy: "متوسط درآمد سالانه ۳۳٬۴۱۰ دلار است【838636707243709†L85-L86】 و شاخص هزینه زندگی ۴۸٫۶ است【325730539419572†L139-L140】؛ هزینه زندگی در جنوب و مناطق روستایی کمتر از مادرید و بارسلون است."
            ),
            [CountryType.Sweden] = (
                Job: "سوئد در حوزه‌های فناوری اطلاعات، مهندسی، محیط زیست و طراحی صنعتی به نیروی متخصص نیاز دارد.",
                Education: "تحصیل برای شهروندان اتحادیه اروپا رایگان است؛ دانشگاه‌های سوئد به زبان انگلیسی در مقطع کارشناسی ارشد ارائه می‌دهند.",
                Economy: "متوسط درآمد سالانه ۵۸٬۸۲۰ دلار است【838636707243709†L68-L69】 و شاخص هزینه زندگی ۶۲٫۸ است【325730539419572†L117-L118】؛ سیستم رفاهی سخاوتمندانه هزینه‌های زندگی بالا را جبران می‌کند."
            ),
            [CountryType.Denmark] = (
                Job: "دانمارک در صنایع فناوری، انرژی‌های تجدیدپذیر، داروسازی و طراحی به نیرو نیاز دارد؛ طرح مثبت لیست مشاغل موردنیاز را اعلام می‌کند.",
                Education: "تحصیل برای شهروندان اتحادیه اروپا و افراد خاص رایگان است و دانشگاه‌ها کیفیت بالایی دارند.",
                Economy: "متوسط درآمد سالانه ۷۳٬۷۹۰ دلار【838636707243709†L62-L63】 و شاخص هزینه زندگی ۷۴٫۱ است【325730539419572†L100-L101】؛ مالیات بالا ولی خدمات اجتماعی گسترده است."
            ),
            [CountryType.Italy] = (
                Job: "بازار کار ایتالیا با نرخ بیکاری بالا مواجه است اما فرصت‌هایی در صنایع گردشگری، طراحی، مد و تولید وجود دارد.",
                Education: "تحصیل در دانشگاه‌های دولتی هزینهٔ نسبتا پایینی دارد و رشته‌های هنر و معماری مشهورند.",
                Economy: "متوسط درآمد سالانه ۳۸٬۲۹۰ دلار【838636707243709†L80-L81】 و شاخص هزینه زندگی ۵۷٫۲ است【325730539419572†L119-L121】؛ هزینه زندگی در شمال بالاتر از جنوب است."
            ),
            [CountryType.Switzerland] = (
                Job: "سوئیس به نیروهای متخصص در بخش‌های مالی، داروسازی، مهندسی و فناوری‌های پیشرفته نیاز دارد؛ دریافت مجوز کار دشوار است.",
                Education: "دانشگاه‌های سوئیس مانند ETH زوریخ جزو بهترین‌های جهان هستند اما هزینه تحصیل و زندگی بالاست.",
                Economy: "متوسط درآمد سالانه ۹۵٬۹۰۰ دلار【838636707243709†L54-L55】 و شاخص هزینه زندگی ۱۰۶٫۸ است【325730539419572†L94-L96】؛ هزینه‌ها بسیار بالا اما کیفیت زندگی، خدمات و درآمد نیز بالا است."
            ),
            [CountryType.Oman] = (
                Job: "اقتصاد عمان وابسته به نفت و گاز است اما پروژه‌های عمرانی، لجستیک و خدمات در حال رشد هستند.",
                Education: "دانشگاه‌های عمان در حال توسعه‌اند و برنامه‌های انگلیسی دارند.",
                Economy: "متوسط درآمد ماهانه حدود ۱٬۶۰۷ دلار آمریکا است【903737609278324†L49-L56】 و شاخص هزینه زندگی ۳۹٫۳ است【325730539419572†L169-L170】؛ مالیات بر درآمد صفر و هزینه‌ها نسبت به اروپا کمتر است."
            )
        };

        /// <summary>
        /// بازیابی اطلاعات تکمیلی برای یک کشور مشخص. اگر اطلاعات یافت نشود، مقادیر خالی برگردانده می‌شود.
        /// </summary>
        private (string Job, string Education, string Economy) GetCountryExtraInfo(CountryType country)
        {
            return CountryExtraInfo.TryGetValue(country, out var info) ? info : (string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// تولید توضیح شخصیتی برای یک کشور و تیپ شخصیتی.
        /// </summary>
        private string GetPersonalityExplanation(PersonalityType type, CountryType country)
        {
            if (type == PersonalityType.Unknown)
                return string.Empty;
            // استخراج حروف تیپ برای ویژگی‌ها
            string code = type.ToString();
            var traits = new List<string>();
            traits.Add(code.StartsWith("E") ? "برون‌گرا" : "درون‌گرا");
            traits.Add(code[1] == 'N' ? "شهودی" : "حسی");
            traits.Add(code[2] == 'F' ? "احساسی" : "منطقی");
            traits.Add(code[3] == 'P' ? "منعطف" : "ساختارمند");
            CountryPersonalityFeatures.TryGetValue(country, out var features);
            string countryName = country.ToString();
            return $"به عنوان فردی {string.Join("، ", traits)}، {countryName} با {features} برای شما می‌تواند جذاب باشد.";
        }
        /// <summary>
        /// محاسبه امتیاز مهاجرت بر اساس ورودی‌های متقاضی.
        /// </summary>
        public ImmigrationResult CalculateImmigration(ImmigrationInput input)
        {
            // ابتدا برای هر فاکتور یک امتیاز پایه محاسبه می‌کنیم. این امتیازها
            // مستقل از کشور هستند و سپس برای هر کشور ضرایب متفاوت اعمال می‌شود.

            // امتیاز سن: بر اساس بازه‌های دقیق‌تر برای بهبود دقت.
            double ageScore = CalculateAgeScore(input.Age);
            // امتیاز تحصیلات به کمک مقطع تحصیلی و دسته رشته/شغل.
            double educationScore = CalculateEducationScore(input.DegreeLevel, input.FieldCategory);
            // امتیاز سابقه کاری با توجه به سال‌های تجربه.
            double experienceScore = CalculateExperienceScore(input.WorkExperienceYears);
            // امتیاز زبان بر اساس نوع مدرک یا سطح.
            double languageScore = CalculateLanguageScore(input.LanguageCertificate);
            // امتیاز ترجیح ویزا.
            double visaPreferenceScore = CalculateVisaPreferenceScore(input.VisaType);
            // امتیاز سرمایه.
            double investmentScore = CalculateInvestmentScore(input.InvestmentAmount);
            // امتیاز وضعیت تاهل.
            double maritalScore = CalculateMaritalScore(input.MaritalStatus);
            // امتیاز تمایل به تحصیل.
            double studyScore = input.WillingToStudy ? 10 : 0;
            // سابقه کیفری امتیاز را کاهش می‌دهد؛ در غیر این‌صورت امتیاز پایه دریافت می‌کند.
            double criminalScore = input.HasCriminalRecord ? -20 : 5;
            // امتیاز شخصیت MBTI.
            double personalityScore = CalculatePersonalityScore(input.MBTIPersonality);
            // امتیاز شغل و دسته رشته. رشته‌های مورد تقاضا امتیاز بیشتری دارند.
            double jobScore = CalculateJobScore(input.JobTitle, input.FieldCategory, input.JobCategory);

            // امتیاز هم‌افزایی: در صورت هم‌زمانی فاکتورهای خاص امتیاز اضافی داده می‌شود.
            double synergyScore = 0;
            // تمایل به تحصیل و انتخاب ویزای تحصیلی.
            if (input.VisaType == VisaType.Study && input.WillingToStudy)
                synergyScore += 5;
            // سرمایه بالا و ویزای سرمایه‌گذاری.
            if (input.VisaType == VisaType.Investment && input.InvestmentAmount >= 100000)
                synergyScore += 5;
            // تجربه کاری بیش از ۵ سال و ویزای کاری.
            if (input.VisaType == VisaType.Work && input.WorkExperienceYears >= 5)
                synergyScore += 3;

            // امتیاز کلی پایه از مجموع همه فاکتورها و هم‌افزایی
            double baseTotal = ageScore + educationScore + experienceScore + languageScore + visaPreferenceScore + investmentScore + maritalScore + studyScore + criminalScore + personalityScore + jobScore + synergyScore;

            // جدول کشورها با ضرایب خاص هر فاکتور. ضرایب بر اساس اهمیت فاکتور در سیستم
            // امتیازدهی هر کشور تنظیم شده‌اند. برای تحصیل، آلمان وزن بیشتری دارد؛ برای
            // سن و تجربه، استرالیا؛ و برای زبان و تحصیلات، کانادا.
            var countryWeights = new Dictionary<CountryType, Dictionary<string, double>>
            {
                // کانادا: تأکید بر تحصیلات و مهارت زبان با نقش پررنگ سرمایه برای برنامه‌های سرمایه‌گذاری.
                [CountryType.Canada] = new Dictionary<string, double>
                {
                    ["Age"] = 0.20,
                    ["Education"] = 0.25,
                    ["Experience"] = 0.15,
                    ["Language"] = 0.20,
                    ["Investment"] = 0.10,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // استرالیا: وزن بالا برای سن و تجربه کاری، همچنین اهمیت زیاد زبان انگلیسی.
                [CountryType.Australia] = new Dictionary<string, double>
                {
                    ["Age"] = 0.25,
                    ["Education"] = 0.20,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.20,
                    ["Investment"] = 0.05,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // آلمان: تأکید بر تحصیلات و زبان، به‌ویژه برای کارت فرصت.
                [CountryType.Germany] = new Dictionary<string, double>
                {
                    ["Age"] = 0.10,
                    ["Education"] = 0.25,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.20,
                    ["Investment"] = 0.05,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.10,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // ایالات متحده: تحصیلات و زبان انگلیسی بسیار مهم هستند، در کنار تجربه کاری.
                [CountryType.USA] = new Dictionary<string, double>
                {
                    ["Age"] = 0.15,
                    ["Education"] = 0.20,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.20,
                    ["Investment"] = 0.15,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // بریتانیا: سیستم امتیازدهی ۷۰ امتیازی با تأکید بر پیشنهاد کار، حقوق و زبان انگلیسی【796060067679459†L245-L260】.
                [CountryType.UK] = new Dictionary<string, double>
                {
                    ["Age"] = 0.10,
                    ["Education"] = 0.15,
                    ["Experience"] = 0.25,
                    ["Language"] = 0.20,
                    ["Investment"] = 0.10,
                    ["Visa"] = 0.10,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // نیوزیلند: وزن متعادل برای سن، تحصیلات، تجربه و زبان.
                [CountryType.NewZealand] = new Dictionary<string, double>
                {
                    ["Age"] = 0.20,
                    ["Education"] = 0.15,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.15,
                    ["Investment"] = 0.10,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // هلند: تأکید بر تحصیلات و زبان در کنار سرمایه‌گذاری برای ویزای استارتاپ.
                [CountryType.Netherlands] = new Dictionary<string, double>
                {
                    ["Age"] = 0.10,
                    ["Education"] = 0.25,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.15,
                    ["Investment"] = 0.10,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.10,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // اسپانیا: زبان اسپانیایی و سرمایه‌گذاری نقش پررنگ‌تری دارند.
                [CountryType.Spain] = new Dictionary<string, double>
                {
                    ["Age"] = 0.10,
                    ["Education"] = 0.15,
                    ["Experience"] = 0.15,
                    ["Language"] = 0.15,
                    ["Investment"] = 0.25,
                    ["Visa"] = 0.10,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // سوئد: تحصیلات و تجربه اهمیت دارند و تحصیل ارزش بیشتری دارد.
                [CountryType.Sweden] = new Dictionary<string, double>
                {
                    ["Age"] = 0.15,
                    ["Education"] = 0.25,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.15,
                    ["Investment"] = 0.05,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // دانمارک: تأکید بر تحصیلات و تجربه؛ همچنین مشاغل موردنیاز.
                [CountryType.Denmark] = new Dictionary<string, double>
                {
                    ["Age"] = 0.15,
                    ["Education"] = 0.20,
                    ["Experience"] = 0.25,
                    ["Language"] = 0.15,
                    ["Investment"] = 0.05,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // ایتالیا: اهمیت تحصیلات و زبان ایتالیایی در کنار تجربه و سرمایه متوسط.
                [CountryType.Italy] = new Dictionary<string, double>
                {
                    ["Age"] = 0.15,
                    ["Education"] = 0.25,
                    ["Experience"] = 0.15,
                    ["Language"] = 0.15,
                    ["Investment"] = 0.10,
                    ["Visa"] = 0.10,
                    ["Study"] = 0.05,
                    ["Job"] = 0.03,
                    ["Marital"] = 0.02
                },
                // سوئیس: تحصیلات و تجربه و مهارت زبان اهمیت زیادی دارند؛ سرمایه‌گذاری نیز مهم است.
                [CountryType.Switzerland] = new Dictionary<string, double>
                {
                    ["Age"] = 0.10,
                    ["Education"] = 0.25,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.20,
                    ["Investment"] = 0.15,
                    ["Visa"] = 0.05,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                },
                // عمان: تأکید بر تجربه کاری و سرمایه‌گذاری، زبان و تحصیلات نقش کمتری دارند.
                [CountryType.Oman] = new Dictionary<string, double>
                {
                    ["Age"] = 0.15,
                    ["Education"] = 0.10,
                    ["Experience"] = 0.20,
                    ["Language"] = 0.10,
                    ["Investment"] = 0.25,
                    ["Visa"] = 0.10,
                    ["Study"] = 0.05,
                    ["Job"] = 0.05,
                    ["Marital"] = 0.05
                }
            };

            // ساخت لیست توصیه‌ها
            var recommendations = new List<CountryRecommendation>();
            foreach (var kvp in countryWeights)
            {
                var country = kvp.Key;
                var weights = kvp.Value;
                // برای هر کشور امتیاز نهایی را به نسبت ضرایب محاسبه می‌کنیم.
                double score =
                    ageScore * weights["Age"] +
                    educationScore * weights["Education"] +
                    experienceScore * weights["Experience"] +
                    languageScore * weights["Language"] +
                    investmentScore * weights["Investment"] +
                    visaPreferenceScore * weights["Visa"] +
                    studyScore * weights["Study"] +
                    jobScore * weights["Job"] +
                    maritalScore * weights["Marital"] +
                    criminalScore +
                    synergyScore;

                // امتیاز اضافی بر اساس تطبیق کشور با تیپ شخصیتی متقاضی
                double personalityBonus = GetPersonalityCountryBonus(input.MBTIPersonality, country);
                score += personalityBonus;

                // نوع ویزای پیشنهادی بر اساس نوع درخواست ورودی و کشور
                string visaRecommendation = GetVisaRecommendation(country, input.VisaType);
                string conditions = GetCountryConditions(country);
                // بازیابی اطلاعات اضافی (بازار کار، تحصیل و اقتصاد) برای این کشور
                var extra = GetCountryExtraInfo(country);
                recommendations.Add(new CountryRecommendation
                {
                    Country = country,
                    Score = Math.Round(score, 2),
                    RecommendedVisaType = visaRecommendation,
                    Conditions = conditions,
                    PersonalityReport = GetPersonalityExplanation(input.MBTIPersonality, country),
                    JobInfo = extra.Job,
                    EducationInfo = extra.Education,
                    EconomyInfo = extra.Economy
                });
            }

            // مرتب‌سازی بر اساس امتیاز و انتخاب سه کشور برتر
            var top = recommendations.OrderByDescending(r => r.Score).Take(3).ToList();
            return new ImmigrationResult { TopCountries = top };
        }

        #region Private helper methods
        private double CalculateAgeScore(int age)
        {
            if (age < 18) return 0;
            if (age <= 19) return 15;
            if (age <= 29) return 20;
            if (age <= 34) return 18;
            if (age <= 39) return 15;
            if (age <= 44) return 10;
            return 5;
        }

        // نسخه جدید محاسبه امتیاز تحصیلات بر اساس Enum های استاندارد. نسخه قدیمی که رشته را به صورت متن می‌پذیرفت حذف شده است.
        private double CalculateEducationScore(DegreeLevelType degreeLevel, FieldCategoryType fieldCategory)
        {
            // امتیاز پایه بر اساس مقطع تحصیلی. مقاطع بالاتر امتیاز بیشتری دارند.
            double baseScore = degreeLevel switch
            {
                DegreeLevelType.Doctorate => 15,
                DegreeLevelType.Master => 12,
                DegreeLevelType.Bachelor => 10,
                DegreeLevelType.Associate => 8,
                DegreeLevelType.Diploma => 6,
                DegreeLevelType.HighSchool => 4,
                _ => 0
            };
            // افزایش امتیاز برای رشته‌های پرتقاضا در بازار جهانی.
            switch (fieldCategory)
            {
                case FieldCategoryType.IT:
                case FieldCategoryType.Engineering:
                case FieldCategoryType.Medicine:
                case FieldCategoryType.Science:
                    baseScore += 3;
                    break;
                case FieldCategoryType.Business:
                    baseScore += 2;
                    break;
                case FieldCategoryType.Arts:
                    baseScore += 1;
                    break;
                default:
                    break;
            }
            return baseScore;
        }

        private double CalculateExperienceScore(int years)
        {
            if (years <= 0) return 0;
            if (years <= 3) return 5;
            if (years <= 7) return 8;
            return 10;
        }

        // نسخه جدید محاسبه امتیاز زبان که از نوع Enum استفاده می‌کند.
        private double CalculateLanguageScore(LanguageCertificateType certificate)
        {
            return certificate switch
            {
                LanguageCertificateType.GermanC2 => 10,
                LanguageCertificateType.GermanC1 => 9,
                LanguageCertificateType.GermanB2 => 8,
                LanguageCertificateType.GermanB1 => 7,
                LanguageCertificateType.GermanA2 => 5,
                LanguageCertificateType.IELTS => 8,
                LanguageCertificateType.TOEFL => 8,
                LanguageCertificateType.Other => 5,
                LanguageCertificateType.None => 0,
                _ => 5
            };
        }

        // نسخه جدید محاسبه امتیاز ترجیح ویزا که از نوع Enum استفاده می‌کند.
        private double CalculateVisaPreferenceScore(VisaType visaType)
        {
            return visaType switch
            {
                // تحصیل معمولاً مسیر محبوبی برای مهاجرت و کسب اقامت است
                VisaType.Study => 10,
                // اقامت دائم یا اجازه اقامت بلندمدت امتیاز بالایی دارد زیرا هدف نهایی بسیاری از متقاضیان است
                VisaType.Residence => 9,
                // ویزای کاری، استارتاپ و ویزای تحقیقاتی فرصت‌های شغلی و علمی مشابهی فراهم می‌کنند
                VisaType.Work => 8,
                VisaType.Startup => 8,
                VisaType.Research => 8,
                // ویزای سرمایه‌گذاری و کارآفرینی به سرمایه و تلاش قابل‌توجهی نیاز دارد
                VisaType.Investment => 7,
                // الحاق خانواده معمولاً ساده‌تر است اما محدود به شرایط خاص
                VisaType.Family => 6,
                // ویزاهای فریلنسری و دیجیتال‌نوماد امتیاز متوسط دارند
                VisaType.Freelancer => 5,
                VisaType.DigitalNomad => 5,
                // ویزاهای بازنشستگی به سرمایه و سن بالاتر نیاز دارند
                VisaType.Retirement => 5,
                // ویزاهای پناهندگی، فرهنگی و انسان‌دوستانه معمولاً سخت‌تر هستند و امتیاز کمتری دارند
                VisaType.Asylum => 4,
                VisaType.Cultural => 4,
                VisaType.Humanitarian => 4,
                // ویزای توریستی امتیاز کمی دارد زیرا مسیری موقت است
                VisaType.Tourist => 3,
                // سایر انواع ویزا امتیاز متوسط دارند
                _ => 5
            };
        }

        private double CalculateInvestmentScore(decimal amount)
        {
            if (amount <= 0) return 0;
            if (amount < 50000) return 3;
            if (amount < 100000) return 6;
            return 10;
        }

        // نسخه جدید محاسبه امتیاز تاهل که از Enum استفاده می‌کند.
        private double CalculateMaritalScore(MaritalStatusType maritalStatus)
        {
            return maritalStatus == MaritalStatusType.Married ? 10 : 5;
        }

        private double CalculatePersonalityScore(PersonalityType type)
        {
            // اگر تیپ مشخص نشده باشد امتیاز صفر است.
            if (type == PersonalityType.Unknown)
                return 0;

            // تعیین امتیاز بر اساس چهار حرف تیپ شخصیتی.
            // حروف به ترتیب نمایانگر برون‌گرایی/درون‌گرایی، شهود/حس‌گرایی، احساس/تفکر و ادراک/قضاوت هستند.
            string code = type.ToString();
            double score = 0;
            // برون‌گرایی (E) امتیاز بیشتری دارد زیرا بیشتر با محیط‌های اجتماعی غربی سازگار است.
            score += code.StartsWith("E") ? 2 : 1;
            // شهود (N) نشان از نوآوری و تطبیق‌پذیری دارد.
            score += code[1] == 'N' ? 2 : 1;
            // احساس (F) به روابط و جامعه اهمیت می‌دهد.
            score += code[2] == 'F' ? 2 : 1;
            // ادراک (P) انعطاف‌پذیری بیشتری دارد؛ قضاوت (J) ساختارمند است.
            score += code[3] == 'P' ? 1 : 1;
            return score;
        }

        private double CalculateJobScore(string? jobTitle, FieldCategoryType fieldCategory, JobCategoryType jobCategory)
        {
            // اگر فرد هیچ شغلی ندارد، امتیاز پایه پایین‌تری دریافت می‌کند.
            if (jobCategory == JobCategoryType.None)
                return 2;

            double score = 0;
            // امتیاز بر اساس دسته شغلی. دسته‌های با تقاضای بالا امتیاز بیشتری می‌گیرند.
            switch (jobCategory)
            {
                case JobCategoryType.IT:
                    score = 10;
                    break;
                case JobCategoryType.Engineering:
                    score = 9;
                    break;
                case JobCategoryType.Healthcare:
                    score = 9;
                    break;
                case JobCategoryType.Telecommunications:
                    score = 9;
                    break;
                case JobCategoryType.Energy:
                    score = 9;
                    break;
                case JobCategoryType.Science:
                    score = 8;
                    break;
                case JobCategoryType.Finance:
                case JobCategoryType.Business:
                case JobCategoryType.Legal:
                    score = 7;
                    break;
                case JobCategoryType.Manufacturing:
                case JobCategoryType.Logistics:
                case JobCategoryType.Transportation:
                    score = 6;
                    break;
                case JobCategoryType.Education:
                case JobCategoryType.Sales:
                case JobCategoryType.RealEstate:
                    score = 6;
                    break;
                case JobCategoryType.Arts:
                case JobCategoryType.Services:
                case JobCategoryType.Agriculture:
                case JobCategoryType.Hospitality:
                    score = 5;
                    break;
                default:
                    score = 5;
                    break;
            }

            // هم‌افزایی: اگر دسته شغل با دسته تحصیلی/حرفه‌ای مرتبط باشد امتیاز اضافه می‌گیرد.
            // مثال: فردی که مهندسی خوانده و در دسته شغلی Engineering کار می‌کند.
            if ((jobCategory == JobCategoryType.IT && fieldCategory == FieldCategoryType.IT) ||
                (jobCategory == JobCategoryType.Engineering && fieldCategory == FieldCategoryType.Engineering) ||
                (jobCategory == JobCategoryType.Healthcare && fieldCategory == FieldCategoryType.Medicine) ||
                (jobCategory == JobCategoryType.Science && fieldCategory == FieldCategoryType.Science) ||
                (jobCategory == JobCategoryType.Business && fieldCategory == FieldCategoryType.Business) ||
                (jobCategory == JobCategoryType.Legal && fieldCategory == FieldCategoryType.Business) ||
                (jobCategory == JobCategoryType.Manufacturing && fieldCategory == FieldCategoryType.Engineering) ||
                (jobCategory == JobCategoryType.Hospitality && fieldCategory == FieldCategoryType.Business) ||
                (jobCategory == JobCategoryType.Logistics && (fieldCategory == FieldCategoryType.Business || fieldCategory == FieldCategoryType.Engineering)) ||
                (jobCategory == JobCategoryType.Transportation && fieldCategory == FieldCategoryType.Engineering) ||
                (jobCategory == JobCategoryType.RealEstate && fieldCategory == FieldCategoryType.Business) ||
                (jobCategory == JobCategoryType.Telecommunications && (fieldCategory == FieldCategoryType.IT || fieldCategory == FieldCategoryType.Engineering)) ||
                (jobCategory == JobCategoryType.Energy && (fieldCategory == FieldCategoryType.Engineering || fieldCategory == FieldCategoryType.Science)) ||
                (jobCategory == JobCategoryType.Arts && fieldCategory == FieldCategoryType.Arts))
            {
                score += 2;
            }

            // هم‌افزایی اضافی برای دسته‌های بسیار پرتقاضا.
            var highDemand = new[] { JobCategoryType.IT, JobCategoryType.Engineering, JobCategoryType.Healthcare, JobCategoryType.Finance, JobCategoryType.Business, JobCategoryType.Energy, JobCategoryType.Telecommunications, JobCategoryType.Legal, JobCategoryType.Science };
            if (highDemand.Contains(jobCategory))
            {
                score += 1;
            }

            // اگر عنوان شغلی شامل کلمات کلیدی پرتقاضا باشد، اندکی امتیاز اضافه می‌شود.
            if (!string.IsNullOrWhiteSpace(jobTitle))
            {
                var titleLower = jobTitle.ToLower();
                var keywords = new[]
                {
                    "برنامه نویس", "توسعه", "نرم افزار", "پرستار", "پزشک", "معلم", "حسابدار", "مالی", "مدیر", "حقوقی",
                    "engineer", "developer", "software", "nurse", "doctor", "teacher", "accountant", "manager", "lawyer", "analyst", "data"
                };
                foreach (var k in keywords)
                {
                    if (titleLower.Contains(k))
                    {
                        score += 1;
                        break;
                    }
                }
            }
            return score;
        }

        private string GetVisaRecommendation(CountryType country, VisaType requested)
        {
            // پیشنهاد نوع ویزا بر اساس کشور و نوع درخواست کاربر.
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
            // پیش‌فرض
            return requested switch
            {
                VisaType.Study => "ویزای تحصیلی",
                VisaType.Investment => "ویزای سرمایه‌گذاری",
                VisaType.Work => "ویزای کاری",
                _ => "ویزای کاری"
            };
        }

        private string GetCountryConditions(CountryType country)
        {
                switch (country)
                {
                    case CountryType.Canada:
                        return "کانادا برای امتیازدهی، سن بین 20 تا 35، تحصیلات دانشگاهی و مهارت زبان انگلیسی یا فرانسوی را مهم می‌داند. تجربه کاری و سرمایه اولیه نیز می‌تواند به اخذ ویزای اکسپرس اینتری کمک کند";
                    case CountryType.Australia:
                        return "استرالیا برای ویزای نیروی ماهر 189 به افرادی بین 18 تا 45 سال با مهارت زبان انگلیسی بالا و حداقل سه سال سابقه کاری در رشته مرتبط امتیاز می‌دهد";
                    case CountryType.Germany:
                        return "برای دریافت کارت فرصت آلمان باید حداقل 6 امتیاز در شاخص‌هایی مانند سن زیر 35 سال، تجربه کاری، مدارک تحصیلی و مهارت زبان آلمانی یا انگلیسی کسب کنید";
                    case CountryType.USA:
                        return "ایالات متحده شرایط ورود سخت‌گیرانه دارد: داشتن پیشنهاد شغلی معتبر (مانند H‑1B)، مدرک تحصیلی دانشگاهی و مهارت زبان انگلیسی لازم است. برای ویزای سرمایه‌گذاری باید حداقل سرمایه مشخصی در برنامه EB‑5 سرمایه‌گذاری کنید";
                    case CountryType.UK:
                        return "بریتانیا طبق سیستم امتیازدهی ۷۰ امتیازی عمل می‌کند؛ داشتن پیشنهاد کار از کارفرمای تایید شده، مهارت زبان انگلیسی و سطح حقوق مناسب از شرایط کلیدی هستند【796060067679459†L245-L260】. ویزای تحصیلی نیازمند پذیرش از دانشگاه و تمکن مالی است";
                    case CountryType.NewZealand:
                        return "نیوزیلند برای مهاجرت مهارتی به سن زیر 55 سال، سابقه کار مرتبط و مهارت زبان انگلیسی نیاز دارد. ویزای سرمایه‌گذاری به سرمایه قابل توجه و اقامت طولانی مدت نیاز دارد";
                    case CountryType.Netherlands:
                        return "هلند برای ویزای نیروی متخصص به پیشنهاد کاری با حداقل حقوق، مدرک دانشگاهی و مهارت زبان انگلیسی یا هلندی نیاز دارد. برای سرمایه‌گذاری باید طرح کسب‌وکار نوآورانه ارائه دهید";
                    case CountryType.Spain:
                        return "اسپانیا برای اقامت کاری به پیشنهاد شغلی و مهارت زبان اسپانیایی یا انگلیسی نیاز دارد. ویزای طلایی با سرمایه‌گذاری در املاک یا کسب‌وکار فراهم می‌شود. برای تحصیل، پذیرش دانشگاه و اثبات تمکن مالی لازم است";
                    case CountryType.Sweden:
                        return "سوئد برای اجازه کار، قرارداد کاری و حقوق کافی الزامی دارد. برای ویزای تحصیلی باید پذیرش دانشگاه سوئدی و اثبات منابع مالی داشته باشید. ویزای استارتاپ به طرح کسب‌وکار نوآور نیاز دارد";
                    case CountryType.Denmark:
                        return "دانمارک برای طرح مثبت لیست داشتن پیشنهاد کار در مشاغل مورد نیاز و مدرک تحصیلی مرتبط را می‌طلبد. برای تحصیل باید پذیرش دانشگاه و اثبات منابع مالی ارائه کنید. ویزای استارتاپ دانمارک نیازمند طرح کسب‌وکار و سرمایه اولیه است";
                    case CountryType.Italy:
                    return "ایتالیا برای ویزای کاری نیاز به پیشنهاد شغلی معتبر، قرارداد کاری و اثبات مهارت زبان دارد. برای ویزای تحصیلی باید پذیرش دانشگاه، بیمه درمانی و تمکن مالی ارائه کنید. ویزای سرمایه‌گذاری یا طلایی با سرمایه‌گذاری در اوراق دولتی یا املاک به ارزش بالا امکان‌پذیر است. اعضای خانواده می‌توانند از طریق ویزای پیوست خانواده اقدام کنند";
                    case CountryType.Switzerland:
                    return "سوئیس قوانین مهاجرتی سختی دارد: برای اجازه کار باید پیشنهاد شغلی با اولویت کارگران سوئیسی و اتحادیه اروپا داشته باشید و حقوق و شرایط کاری مناسب را اثبات کنید. برای تحصیل نیاز به پذیرش دانشگاه و تمکن مالی دارید. برنامه‌های سرمایه‌گذاری یا اقامت مالیاتی برای افراد دارای سرمایه قابل توجه ارائه می‌شود. الحاق خانواده نیز مستلزم تامین مالی است";
                    case CountryType.Oman:
                    return "عمان برای اخذ ویزای کار، اسپانسر عمانی و قرارداد کاری لازم دارد. ویزای سرمایه‌گذاری با حداقل سرمایه مشخص و ایجاد کسب‌وکار در کشور صادر می‌شود. ویزای دانشجویی برای تحصیل در دانشگاه‌های عمان ارائه می‌شود و نسبتاً شرایط ساده‌تری دارد. ویزای توریستی نیز به صورت آنلاین در دسترس است و مدت محدودی دارد";
                    default:
                        return string.Empty;
                }
        }
        #endregion
    }
}