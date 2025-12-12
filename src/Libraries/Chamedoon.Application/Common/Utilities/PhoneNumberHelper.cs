using System.Linq;

namespace Chamedoon.Application.Common.Utilities;

public static class PhoneNumberHelper
{
    public static string? Normalize(string phone)
    {
        var digitsOnly = new string((phone ?? string.Empty).Where(char.IsDigit).ToArray());

        if (digitsOnly.StartsWith("98") && digitsOnly.Length == 12)
        {
            digitsOnly = "0" + digitsOnly[2..];
        }

        if (digitsOnly.StartsWith("0098"))
        {
            digitsOnly = "0" + digitsOnly[4..];
        }

        return digitsOnly.Length == 11 && digitsOnly.StartsWith("09") ? digitsOnly : null;
    }

    public static bool IsValid(string phone)
    {
        return Normalize(phone) is not null;
    }
}
