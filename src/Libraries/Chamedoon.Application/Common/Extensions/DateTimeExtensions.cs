using System.Globalization;

namespace Chamedoon.Application.Common.Extensions;

public static class DateTimeExtensions
{
    public static string ConvertMiladiToShamsi(this System.DateTime? DateTime, string seperator = "/")
    {
        try
        {
            if (DateTime.HasValue == false || DateTime == default)
            {
                return "";
            }

            if (string.IsNullOrEmpty(seperator))
            {
                seperator = string.Empty;
            }
            PersianCalendar PerCal = new PersianCalendar();
            string Year, Day, Month;
            Year = PerCal.GetYear(DateTime.Value).ToString();
            Month = PerCal.GetMonth(DateTime.Value).ToString();
            Day = PerCal.GetDayOfMonth(DateTime.Value).ToString();
            if (Day.Length == 1)
            {
                Day = PerCal.GetDayOfMonth(DateTime.Value).ToString().Insert(0, "0");
            }
            if (Month.Length == 1)
            {
                Month = PerCal.GetMonth(DateTime.Value).ToString().Insert(0, "0");
            }
            return Year + seperator + Month + seperator + Day;
        }
        catch (Exception)
        {

            return "";
        }


    }
}
