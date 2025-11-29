using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Application.Services.Immigration
{
    /// <summary>
    /// نوع ویزای درخواستی.
    /// </summary>
    public enum VisaType
    {
        /// <summary>
        /// ویزای کاری معمولی یا تخصصی برای اشتغال در کشور مقصد.
        /// </summary>
        Work,
        /// <summary>
        /// ویزای تحصیلی برای دانشجویان و محصلان.
        /// </summary>
        Study,
        /// <summary>
        /// ویزای سرمایه‌گذاری یا کارآفرینی.
        /// </summary>
        Investment,
        /// <summary>
        /// ویزای خانوادگی یا الحاق به خانواده.
        /// </summary>
        Family,
        /// <summary>
        /// ویزای توریستی یا بازدید کوتاه‌مدت.
        /// </summary>
        Tourist,
        /// <summary>
        /// ویزای نوآوری یا استارتاپ برای کارآفرینان.
        /// </summary>
        Startup,
        /// <summary>
        /// ویزای اقامت دائم یا اجازه اقامت بلندمدت.
        /// </summary>
        Residence,
        /// <summary>
        /// ویزای کاری از راه دور یا دیجیتال نوماد.
        /// این نوع ویزا برای افرادی است که می‌توانند از راه دور برای شرکت یا خود کار کنند.
        /// </summary>
        DigitalNomad,
        /// <summary>
        /// ویزای فریلنسری یا خوداشتغالی برای افراد آزادکار.
        /// </summary>
        Freelancer,
        /// <summary>
        /// ویزای تحقیقاتی یا پژوهشی برای اساتید و محققان.
        /// </summary>
        Research,
        /// <summary>
        /// ویزای بازنشستگی یا اقامت برای افراد بازنشسته با تمکن مالی.
        /// </summary>
        Retirement,
        /// <summary>
        /// ویزای پناهندگی یا پناهجویی.
        /// </summary>
        Asylum,
        /// <summary>
        /// ویزاهای فرهنگی یا هنری برای شرکت در رویدادهای فرهنگی.
        /// </summary>
        Cultural,
        /// <summary>
        /// ویزاهای بشر‌دوستانه و انسان‌دوستانه برای افراد آسیب‌پذیر.
        /// </summary>
        Humanitarian,
        /// <summary>
        /// سایر انواع ویزا که در دسته‌بندی‌های فوق نمی‌گنجد.
        /// </summary>
        Other
    }
    public enum DegreeLevelType
    {
        [Display(Name = "دیپلم دبیرستان")] HighSchool,
        [Display(Name = "دیپلم فنی یا هنرستان")] Diploma,
        [Display(Name = "کاردانی (فوق‌دیپلم)")] Associate,
        [Display(Name = "کارشناسی")] Bachelor,
        [Display(Name = "کارشناسی ارشد")] Master,
        [Display(Name = "دکترا")] Doctorate
    }
    public enum MaritalStatusType
    {
        [Display(Name = "مجرد")] Single,
        [Display(Name = "متأهل")] Married
    }

    public enum LanguageCertificateType
    {
        [Display(Name = "بدون مدرک")] None,
        [Display(Name = "آیلتس (IELTS)")] IELTS,
        [Display(Name = "تافل (TOEFL)")] TOEFL,
        [Display(Name = "آلمانی A2")] GermanA2,
        [Display(Name = "آلمانی B1")] GermanB1,
        [Display(Name = "آلمانی B2")] GermanB2,
        [Display(Name = "آلمانی C1")] GermanC1,
        [Display(Name = "آلمانی C2")] GermanC2,
        [Display(Name = "مدرک دیگر")] Other
    }
    public enum FieldCategoryType
    {
        [Display(Name = "سایر")] Other,
        [Display(Name = "فناوری اطلاعات")] IT,
        [Display(Name = "مهندسی")] Engineering,
        [Display(Name = "پزشکی")] Medicine,
        [Display(Name = "علوم پایه")] Science,
        [Display(Name = "کسب‌وکار")] Business,
        [Display(Name = "هنر")] Arts
    }

    public enum JobCategoryType
    {
        [Display(Name = "بدون شغل")] None,
        [Display(Name = "فناوری اطلاعات")] IT,
        [Display(Name = "مهندسی")] Engineering,
        [Display(Name = "سلامت و پزشکی")] Healthcare,
        [Display(Name = "آموزش و تدریس")] Education,
        [Display(Name = "مالی و حسابداری")] Finance,
        [Display(Name = "کسب‌وکار و مدیریت")] Business,
        [Display(Name = "علوم پایه")] Science,
        [Display(Name = "هنر و خلاقیت")] Arts,
        [Display(Name = "خدمات")] Services,
        [Display(Name = "کشاورزی")] Agriculture,
        [Display(Name = "فروش و بازاریابی")] Sales,
        [Display(Name = "حقوق و وکالت")] Legal,
        [Display(Name = "تولید و صنعت")] Manufacturing,
        [Display(Name = "مهمان‌داری و گردشگری")] Hospitality,
        [Display(Name = "لجستیک و زنجیره تأمین")] Logistics,
        [Display(Name = "حمل‌ونقل")] Transportation,
        [Display(Name = "املاک و مستغلات")] RealEstate,
        [Display(Name = "مخابرات و ارتباطات")] Telecommunications,
        [Display(Name = "انرژی و منابع")] Energy
    }
    public enum PersonalityType
    {
        Unknown,
        ENFP,
        ENFJ,
        ESFP,
        ESFJ,
        ENTP,
        ENTJ,
        ESTP,
        ESTJ,
        INFP,
        INFJ,
        ISFP,
        ISFJ,
        INTP,
        INTJ,
        ISTP,
        ISTJ
    }

    public enum ScoringAlgorithmType
    {
        [Display(Name = "استاندارد")] Standard,
        [Display(Name = "تحلیل با هوش مصنوعی")] AiEnhanced
    }
    public enum CountryType
    {
        [Description("کانادا")]
        Canada,

        [Description("استرالیا")]
        Australia,

        [Description("آلمان")]
        Germany,

        [Description("ایالات متحده آمریکا")]
        USA,

        [Description("بریتانیا")]
        UK,

        [Description("نیوزیلند")]
        NewZealand,

        [Description("هلند")]
        Netherlands,

        [Description("اسپانیا")]
        Spain,

        [Description("سوئد")]
        Sweden,

        [Description("دانمارک")]
        Denmark,

        [Description("ایتالیا")]
        Italy,

        [Description("سوئیس")]
        Switzerland,

        [Description("عمان")]
        Oman
    }
}