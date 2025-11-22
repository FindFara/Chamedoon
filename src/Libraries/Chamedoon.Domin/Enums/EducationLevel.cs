using System.ComponentModel;

namespace Chamedoon.Domin.Enums
{
    public enum EducationLevel
    {
        [Description("دیپلم")]
        Diploma = 0,

        [Description("کاردانی")]
        Associate = 1,

        [Description("کارشناسی")]
        Bachelor = 2,

        [Description("کارشناسی ارشد")]
        Master = 3,

        [Description("دکتری")]
        Doctorate = 4,

        [Description("سایر")]
        Other = 5
    }
}
