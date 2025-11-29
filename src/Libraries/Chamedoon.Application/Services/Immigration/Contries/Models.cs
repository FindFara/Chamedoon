using System;
using System.Collections.Generic;

namespace CountryPoints
{
    /// <summary>
    /// اطلاعات مربوط به یک شغل شامل اینام، توضیحات، امتیاز و اثر سابقهٔ کاری.
    /// از این کلاس در تمامی کشورها استفاده می‌شود.
    /// </summary>
    public class JobInfo
    {
        /// <summary>
        /// عنوان شغل به صورت اینام از نوع JobTitle. این مقدار از خطای تایپ جلوگیری می‌کند.
        /// </summary>
        public JobTitle Job { get; set; }

        /// <summary>
        /// توضیحات فارسی دربارهٔ شرایط بازار کار و اهمیت این شغل در کشور مورد نظر.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// امتیاز شغل بین صفر تا صد. این امتیاز نسبت به سایر کشورها تعیین می‌شود تا مقایسهٔ بین‌المللی فراهم شود.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// توضیح دربارهٔ اینکه چگونه سابقهٔ کاری یا گواهینامه‌های تخصصی می‌تواند امتیاز را افزایش دهد.
        /// </summary>
        public string ExperienceImpact { get; set; }
    }

    /// <summary>
    /// اطلاعات مربوط به یک رشتهٔ تحصیلی شامل اینام رشته، توضیحات، امتیاز، مقطع تحصیلی و نیاز زبان.
    /// این ساختار برای مقایسهٔ رشته‌ها در بین کشورها استفاده می‌شود.
    /// </summary>
    public class EducationInfo
    {
        /// <summary>
        /// رشتهٔ تحصیلی به صورت اینام از نوع FieldName.
        /// </summary>
        public FieldName Field { get; set; }

        /// <summary>
        /// توضیحات فارسی دربارهٔ وضعیت تحصیل، بازار کار و فرصت‌های این رشته در کشور مورد نظر.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// امتیاز این رشته بین صفر تا صد. این امتیاز بیانگر جذابیت نسبی رشته در کشور است.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// مقطع تحصیلی مناسب برای این رشته (دیپلم، کارشناسی، کارشناسی ارشد، دکترا).
        /// </summary>
        public EducationLevel Level { get; set; }

        /// <summary>
        /// نیازهای زبانی یا الزامات پذیرش برای این رشته (مثلاً حداقل نمرهٔ آیلتس یا آزمون‌های زبان).
        /// </summary>
        public string LanguageRequirement { get; set; }
    }

    /// <summary>
    /// هزینه مسکن بر اساس متراژ (مترمربع)، تعداد اتاق و منطقه. این کلاس برای درج نمونه‌های متنوع هزینهٔ مسکن استفاده می‌شود.
    /// </summary>
    public class HousingCost
    {
        /// <summary>
        /// متراژ واحد مسکونی به متر مربع.
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// تعداد اتاق‌های مستقل.
        /// </summary>
        public int Rooms { get; set; }

        /// <summary>
        /// نام منطقه یا شهری که این واحد مسکونی در آن قرار دارد.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// هزینهٔ ماهانهٔ اجاره یا مسکن به واحد پول کشور مورد نظر.
        /// </summary>
        public double Cost { get; set; }
    }

    /// <summary>
    /// حداقل هزینه‌های زندگی شامل مسکن، غذا و پوشاک. این کلاس یک برداشت کلی از هزینه‌های پایه‌ای در یک کشور ارائه می‌دهد.
    /// </summary>
    public class MinimumLivingCosts
    {
        /// <summary>
        /// فهرست هزینه‌های مسکن برای واحدهای با متراژها و مناطق متفاوت.
        /// </summary>
        public List<HousingCost> Housing { get; set; }

        /// <summary>
        /// متوسط هزینه ماهانه برای یک خانواده چهار نفره (بدون اجاره).
        /// </summary>
        public string FamilyMonthly { get; set; }

        /// <summary>
        /// میانگین هزینه زندگی برای یک نفر (بدون اجاره).
        /// </summary>
        public string SingleMonthly { get; set; }

        /// <summary>
        /// هزینه حمل و نقل (مانند کارت ماهانه حمل‌ونقل عمومی).
        /// </summary>
        public string Transport { get; set; }

        /// <summary>
        /// خدمات رفاهی (برق، آب، گاز و زباله) برای آپارتمان معمولی.
        /// </summary>
        public string Utilities { get; set; }

        /// <summary>
        /// هزینه فعالیت‌های تفریحی مانند عضویت باشگاه یا بلیط سینما.
        /// </summary>
        public string Recreation { get; set; }
        
        /// <summary>
        /// میانگین اجاره آپارتمان یک‌خوابه در سه شهر بزرگ یا نمونه مرجع.
        /// </summary>
        public string RentOneBedroom { get; set; }

        /// <summary>
        /// میانگین اجاره آپارتمان سه‌خوابه در سه شهر بزرگ یا نمونه مرجع.
        /// </summary>
        public string RentThreeBedroom { get; set; }

        /// <summary>
        /// هزینه اینترنت نامحدود ماهانه.
        /// </summary>
        public string Internet { get; set; }
    }
}