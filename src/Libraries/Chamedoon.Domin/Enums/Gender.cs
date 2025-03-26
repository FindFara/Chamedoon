using System.ComponentModel;
namespace Chamedoon.Domin.Enums;

public enum Gender
{
    [Description("نامشخص")]
    Unknown = 0,

    [Description("مرد")]
    Male =1,

    [Description("زن")]
    Female =2,

    [Description("سایر")]
    Other =3,
}
