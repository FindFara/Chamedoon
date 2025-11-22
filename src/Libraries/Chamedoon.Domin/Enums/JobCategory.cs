using System.ComponentModel;

namespace Chamedoon.Domin.Enums
{
    public enum JobCategory
    {
        [Description("فناوری")]
        Technology = 0,

        [Description("بهداشت و درمان")]
        Healthcare = 1,

        [Description("مهندسی")]
        Engineering = 2,

        [Description("آموزش")]
        Education = 3,

        [Description("کسب‌وکار")]
        Business = 4,

        [Description("خدمات")]
        Services = 5,

        [Description("سایر")]
        Other = 6
    }
}
