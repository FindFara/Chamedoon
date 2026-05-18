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

    public static string ConvertMiladiToShamsi(this System.DateTime dateTime, string seperator = "/")
    {
        return ((System.DateTime?)dateTime).ConvertMiladiToShamsi(seperator);
    }

    public static string ConvertMiladiToShamsi(this System.DateTimeOffset dateTime, string seperator = "/")
    {
        return ((System.DateTimeOffset?)dateTime).ConvertMiladiToShamsiOffset(seperator);
    }

    public static string ConvertMiladiToShamsiOffset(this System.DateTimeOffset? DateTime, string seperator = "/")
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
            Year = PerCal.GetYear(DateTime.Value.DateTime).ToString();
            Month = PerCal.GetMonth(DateTime.Value.DateTime).ToString();
            Day = PerCal.GetDayOfMonth(DateTime.Value.DateTime).ToString();
            if (Day.Length == 1)
            {
                Day = PerCal.GetDayOfMonth(DateTime.Value.DateTime).ToString().Insert(0, "0");
            }
            if (Month.Length == 1)
            {
                Month = PerCal.GetMonth(DateTime.Value.DateTime).ToString().Insert(0, "0");
            }
            return Year + seperator + Month + seperator + Day;
        }
        catch (Exception)
        {

            return "";
        }
    }


    public static string ConvertMiladiToShamsiWithTime(this System.DateTime dateTime, string dateSeperator = "/")
    {
        return ((System.DateTime?)dateTime).ConvertMiladiToShamsiWithTime(dateSeperator);
    }

    public static string ConvertMiladiToShamsiWithTime(this System.DateTimeOffset dateTime, string dateSeperator = "/")
    {
        return ((System.DateTimeOffset?)dateTime).ConvertMiladiToShamsiWithTime(dateSeperator);
    }

    public static string ConvertMiladiToShamsiWithTime(this System.DateTime? dateTime, string dateSeperator = "/")
    {
        if (dateTime.HasValue == false || dateTime == default)
        {
            return string.Empty;
        }

        var shamsiDate = dateTime.ConvertMiladiToShamsi(dateSeperator);
        if (string.IsNullOrWhiteSpace(shamsiDate))
        {
            return string.Empty;
        }

        return $"{shamsiDate} {dateTime.Value:HH:mm}";
    }

    public static string ConvertMiladiToShamsiWithTime(this System.DateTimeOffset? dateTime, string dateSeperator = "/")
    {
        if (dateTime.HasValue == false || dateTime == default)
        {
            return string.Empty;
        }

        var shamsiDate = dateTime.ConvertMiladiToShamsiOffset(dateSeperator);
        if (string.IsNullOrWhiteSpace(shamsiDate))
        {
            return string.Empty;
        }

        return $"{shamsiDate} {dateTime.Value:HH:mm}";
    }

}
