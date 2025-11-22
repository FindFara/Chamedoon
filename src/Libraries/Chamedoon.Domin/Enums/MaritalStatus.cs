using System.ComponentModel;

namespace Chamedoon.Domin.Enums
{
    public enum MaritalStatus
    {
        [Description("مجرد")]
        Single = 0,

        [Description("متأهل")]
        Married = 1,

        [Description("طلاق گرفته")]
        Divorced = 2,

        [Description("بیوه")]
        Widowed = 3
    }
}
