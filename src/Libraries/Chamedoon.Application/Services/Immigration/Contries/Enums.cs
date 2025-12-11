using System;
using System.ComponentModel;

namespace CountryPoints
{
    /// <summary>
    /// اینام‌های مشترک برای عنوان شغل، رشته تحصیلی و مقطع تحصیلی.
    /// از کاراکترهای فارسی و خط فاصله زیر برای جلوگیری از فاصله استفاده شده است.
    /// </summary>
    public enum JobTitle
    {
        /// <summary>
        /// توسعه‌دهنده نرم‌افزار
        /// </summary>
        [Description("توسعه‌دهنده نرم‌افزار")]
        SoftwareDeveloper,

        /// <summary>
        /// مهندس برق و الکترونیک
        /// </summary>
        [Description("مهندس برق و الکترونیک")]
        ElectricalEngineer,

        /// <summary>
        /// مهندس مکانیک و تولید
        /// </summary>
        [Description("مهندس مکانیک و تولید")]
        MechanicalEngineer,

        /// <summary>
        /// مهندس عمران و ساخت
        /// </summary>
        [Description("مهندس عمران و ساخت")]
        CivilEngineer,

        /// <summary>
        /// دانشمند داده و تحلیل داده
        /// </summary>
        [Description("دانشمند داده و تحلیل داده")]
        DataScientist,

        /// <summary>
        /// تحلیلگر کسب و کار
        /// </summary>
        [Description("تحلیلگر کسب و کار")]
        BusinessAnalyst,

        /// <summary>
        /// مدیر پروژه
        /// </summary>
        [Description("مدیر پروژه")]
        ProjectManager,

        /// <summary>
        /// متخصص شبکه و امنیت
        /// </summary>
        [Description("متخصص شبکه و امنیت")]
        NetworkSecuritySpecialist,

        /// <summary>
        /// حسابدار و امور مالی
        /// </summary>
        [Description("حسابدار و امور مالی")]
        Accountant,

        /// <summary>
        /// مدیر منابع انسانی
        /// </summary>
        [Description("مدیر منابع انسانی")]
        HumanResourcesManager,

        /// <summary>
        /// متخصص بازاریابی و دیجیتال مارکتینگ
        /// </summary>
        [Description("متخصص بازاریابی و دیجیتال مارکتینگ")]
        MarketingSpecialist,

        /// <summary>
        /// پژوهشگر علمی و دانشگاهی
        /// </summary>
        [Description("پژوهشگر علمی و دانشگاهی")]
        Researcher,

        /// <summary>
        /// آشپز و خدمات رستوران
        /// </summary>
        [Description("آشپز و خدمات رستوران")]
        Chef,

        /// <summary>
        /// پرستار و متخصص سلامت
        /// </summary>
        [Description("پرستار و متخصص سلامت")]
        Nurse,

        /// <summary>
        /// معلم و آموزش
        /// </summary>
        [Description("معلم و آموزش")]
        Teacher
    }

    /// <summary>
    /// اینام رشته‌های تحصیلی. هر مقدار نام رشته را به صورت فارسی بدون فاصله ارائه می‌کند.
    /// </summary>
    public enum FieldName
    {
        /// <summary>
        /// مهندسی نرم‌افزار
        /// </summary>
        [Description("مهندسی نرم‌افزار")]
        SoftwareEngineering,

        /// <summary>
        /// مهندسی برق و الکترونیک
        /// </summary>
        [Description("مهندسی برق و الکترونیک")]
        ElectricalAndElectronicEngineering,

        /// <summary>
        /// مهندسی مکانیک
        /// </summary>
        [Description("مهندسی مکانیک")]
        MechanicalEngineering,

        /// <summary>
        /// مهندسی عمران
        /// </summary>
        [Description("مهندسی عمران")]
        CivilEngineering,

        /// <summary>
        /// علوم داده
        /// </summary>
        [Description("علوم داده")]
        DataScience,

        /// <summary>
        /// تحلیل و مدیریت کسب و کار
        /// </summary>
        [Description("تحلیل و مدیریت کسب و کار")]
        BusinessAnalysisAndManagement,

        /// <summary>
        /// مدیریت پروژه
        /// </summary>
        [Description("مدیریت پروژه")]
        ProjectManagement,

        /// <summary>
        /// حسابداری و مالی
        /// </summary>
        [Description("حسابداری و مالی")]
        AccountingAndFinance,

        /// <summary>
        /// مدیریت منابع انسانی
        /// </summary>
        [Description("مدیریت منابع انسانی")]
        HumanResourceManagement,

        /// <summary>
        /// بازاریابی و دیجیتال مارکتینگ
        /// </summary>
        [Description("بازاریابی و دیجیتال مارکتینگ")]
        MarketingAndDigitalMarketing,

        /// <summary>
        /// علوم بهداشتی و پرستاری
        /// </summary>
        [Description("علوم بهداشتی و پرستاری")]
        HealthSciencesAndNursing,

        /// <summary>
        /// آموزش
        /// </summary>
        [Description("آموزش")]
        Education,

        /// <summary>
        /// علوم پایه و پژوهش
        /// </summary>
        [Description("علوم پایه و پژوهش")]
        PureSciencesAndResearch,

        /// <summary>
        /// هنر و طراحی
        /// </summary>
        [Description("هنر و طراحی")]
        ArtsAndDesign,

        /// <summary>
        /// گردشگری و مهمان‌داری
        /// </summary>
        [Description("گردشگری و مهمان‌داری")]
        TourismAndHospitality
    }

    /// <summary>
    /// اینام مقاطع تحصیلی متداول. این مقاطع برای نشان دادن سطح تحصیل استفاده می‌شوند.
    /// </summary>
    public enum EducationLevel
    {
        /// <summary>
        /// دیپلم
        /// </summary>
        [Description("دیپلم")]
        Diploma,

        /// <summary>
        /// کارشناسی
        /// </summary>
        [Description("کارشناسی")]
        Bachelor,

        /// <summary>
        /// کارشناسی ارشد
        /// </summary>
        [Description("کارشناسی ارشد")]
        Master,

        /// <summary>
        /// دکترا
        /// </summary>
        [Description("دکترا")]
        Doctorate
    }

    /// <summary>
    /// اینام برای وضعیت تأهل. مقادیر انگلیسی است و توضیح فارسی با Description افزوده می‌شود.
    /// </summary>
    public enum MaritalStatus
    {
        /// <summary>
        /// مجرد
        /// </summary>
        [Description("مجرد")]
        Single,

        /// <summary>
        /// متأهل
        /// </summary>
        [Description("متأهل")]
        Married
    }

    /// <summary>
    /// اینام تیپ‌های شخصیتی بر اساس شاخص ام‌بی‌تی‌آی (MBTI). مقادیر به صورت کدهای انگلیسی ارائه شده‌اند و برای توصیف ویژگی‌های شخصیتی استفاده می‌شوند.
    /// این لیست می‌تواند برای تعیین سازگاری فرهنگی افراد با کشورهای مختلف مورد استفاده قرار گیرد.
    /// </summary>
    public enum PersonalityType
    {
        /// <summary>
        /// نامشخص یا تعریف نشده
        /// </summary>
        Unknown,

        /// <summary>
        /// شخصیت ENFP (Campaigner)
        /// </summary>
        ENFP,

        /// <summary>
        /// شخصیت ENFJ (Protagonist)
        /// </summary>
        ENFJ,

        /// <summary>
        /// شخصیت ESFP (Entertainer)
        /// </summary>
        ESFP,

        /// <summary>
        /// شخصیت ESFJ (Consul)
        /// </summary>
        ESFJ,

        /// <summary>
        /// شخصیت ENTP (Debater)
        /// </summary>
        ENTP,

        /// <summary>
        /// شخصیت ENTJ (Commander)
        /// </summary>
        ENTJ,

        /// <summary>
        /// شخصیت ESTP (Entrepreneur)
        /// </summary>
        ESTP,

        /// <summary>
        /// شخصیت ESTJ (Executive)
        /// </summary>
        ESTJ,

        /// <summary>
        /// شخصیت INFP (Mediator)
        /// </summary>
        INFP,

        /// <summary>
        /// شخصیت INFJ (Advocate)
        /// </summary>
        INFJ,

        /// <summary>
        /// شخصیت ISFP (Adventurer)
        /// </summary>
        ISFP,

        /// <summary>
        /// شخصیت ISFJ (Defender)
        /// </summary>
        ISFJ,

        /// <summary>
        /// شخصیت INTP (Logician)
        /// </summary>
        INTP,

        /// <summary>
        /// شخصیت INTJ (Architect)
        /// </summary>
        INTJ,

        /// <summary>
        /// شخصیت ISTP (Virtuoso)
        /// </summary>
        ISTP,

        /// <summary>
        /// شخصیت ISTJ (Logistician)
        /// </summary>
        ISTJ
    }
}