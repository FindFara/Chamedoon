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

    /// <summary>
    /// مقاطع تحصیلی برای محاسبه امتیاز.
    /// </summary>
    public enum DegreeLevelType
    {
        [Display(Name = "دیپلم دبیرستان")] HighSchool,
        [Display(Name = "دیپلم فنی یا هنرستان")] Diploma,
        [Display(Name = "کاردانی (فوق‌دیپلم)")] Associate,
        [Display(Name = "کارشناسی")] Bachelor,
        [Display(Name = "کارشناسی ارشد")] Master,
        [Display(Name = "دکترا")] Doctorate
    }

    /// <summary>
    /// وضعیت تاهل متقاضی.
    /// </summary>
    public enum MaritalStatusType
    {
        [Display(Name = "مجرد")] Single,
        [Display(Name = "متأهل")] Married
    }

    /// <summary>
    /// نوع مدرک یا سطح زبان. مقادیر آلمانی بر اساس سطح CEFR دسته‌بندی شده‌اند.
    /// </summary>
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

    /// <summary>
    /// دسته‌بندی کلی رشته یا زمینه تحصیل/شغل.
    /// </summary>
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

    /// <summary>
    /// دسته‌بندی مشاغل برای امتیازدهی دقیق‌تر. شامل گزینه‌ای برای افراد بدون شغل نیز می‌شود.
    /// </summary>
    public enum JobCategoryType
    {
        /// <summary>
        /// بدون شغل یا بیکار. مناسب برای افرادی که در حال حاضر شغلی ندارند یا فقط در حال تحصیل هستند.
        /// </summary>
        [Display(Name = "بدون شغل")] None,
        /// <summary>
        /// حوزه فناوری اطلاعات و نرم‌افزار.
        /// </summary>
        [Display(Name = "فناوری اطلاعات")] IT,
        /// <summary>
        /// مهندسی در رشته‌های مختلف مانند عمران، مکانیک، برق و غیره.
        /// </summary>
        [Display(Name = "مهندسی")] Engineering,
        /// <summary>
        /// مراقبت‌های بهداشتی، پزشکی و پرستاری.
        /// </summary>
        [Display(Name = "سلامت و پزشکی")] Healthcare,
        /// <summary>
        /// آموزش و تدریس در سطوح مختلف.
        /// </summary>
        [Display(Name = "آموزش و تدریس")] Education,
        /// <summary>
        /// امور مالی، حسابداری، بانکداری و بیمه.
        /// </summary>
        [Display(Name = "مالی و حسابداری")] Finance,
        /// <summary>
        /// کسب و کار، مدیریت و کارآفرینی.
        /// </summary>
        [Display(Name = "کسب‌وکار و مدیریت")] Business,
        /// <summary>
        /// علوم پایه و تحقیقاتی مانند فیزیک، شیمی و ریاضیات.
        /// </summary>
        [Display(Name = "علوم پایه")] Science,
        /// <summary>
        /// هنر، طراحی و صنایع خلاق.
        /// </summary>
        [Display(Name = "هنر و خلاقیت")] Arts,
        /// <summary>
        /// خدمات مانند هتل‌داری، گردشگری و پذیرایی.
        /// </summary>
        [Display(Name = "خدمات")] Services,
        /// <summary>
        /// کشاورزی، دامپروری و صنایع غذایی.
        /// </summary>
        [Display(Name = "کشاورزی")] Agriculture,
        /// <summary>
        /// فروش و بازاریابی.
        /// </summary>
        [Display(Name = "فروش و بازاریابی")] Sales,
        /// <summary>
        /// حقوق، وکالت و امور قضایی.
        /// </summary>
        [Display(Name = "حقوق و وکالت")] Legal,
        /// <summary>
        /// تولید و کارخانه‌ای.
        /// </summary>
        [Display(Name = "تولید و صنعت")] Manufacturing,
        /// <summary>
        /// مهمان‌داری و گردشگری مانند رستوران و هتل.
        /// </summary>
        [Display(Name = "مهمان‌داری و گردشگری")] Hospitality,
        /// <summary>
        /// لجستیک، زنجیره تأمین و انبارداری.
        /// </summary>
        [Display(Name = "لجستیک و زنجیره تأمین")] Logistics,
        /// <summary>
        /// حمل‌ونقل زمینی، هوایی و دریایی.
        /// </summary>
        [Display(Name = "حمل‌ونقل")] Transportation,
        /// <summary>
        /// املاک و مستغلات و مدیریت دارایی‌ها.
        /// </summary>
        [Display(Name = "املاک و مستغلات")] RealEstate,
        /// <summary>
        /// مخابرات و ارتباطات.
        /// </summary>
        [Display(Name = "مخابرات و ارتباطات")] Telecommunications,
        /// <summary>
        /// انرژی، نفت و گاز و منابع طبیعی.
        /// </summary>
        [Display(Name = "انرژی و منابع")] Energy
    }

    /// <summary>
    /// تیپ‌های شخصیتی MBTI. این فهرست شامل 16 تیپ رایج به‌همراه گزینه ناشناخته است.
    /// استفاده از Enum به کاربر امکان می‌دهد تا شخصیت خود را دقیق‌تر انتخاب کند و الگوریتم
    /// بر اساس آن توصیه‌های مناسب‌تری ارائه دهد.
    /// </summary>
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

    /// <summary>
    /// فهرست کشورهایی که سیستم امتیازدهی از آن‌ها پشتیبانی می‌کند. استفاده از Enum موجب می‌شود
    /// در سراسر برنامه از نام‌های ثابت و بدون خطا استفاده شود. مقدار ToString این Enum به زبان انگلیسی باقی می‌ماند
    /// تا قابلیت استفاده در کلیدها و نمایش لیست‌ها حفظ شود.
    /// </summary>
    public enum CountryType
    {
        Canada,
        Australia,
        Germany,
        USA,
        UK,
        NewZealand,
        Netherlands,
        Spain,
        Sweden,
        Denmark,
        Italy,
        Switzerland,
        Oman
    }
}