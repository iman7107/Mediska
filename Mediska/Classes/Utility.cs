using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Mediska.Classes
{
    public static class Utility
    {

        public static DateTime myConvertShamsiToMiladi(string date)
        {
            var year = int.Parse(date.Substring(0, 4));
            var month = int.Parse(date.Substring(5, 2));
            var day = int.Parse(date.Substring(8, 2));
            var persianCalendar = new PersianCalendar();
            var dateTime = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);

            var result = DateTime.Parse($"{dateTime.Year}/{dateTime.Month}/{dateTime.Day} {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}");
            return result;
        }

        public static string myConvertMiladiToShamsi(DateTime date)
        {
            try
            {
                var dateString = date == null ? DateTime.Now : DateTime.Parse(date.ToString());

                var persianCalendar = new PersianCalendar();
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(persianCalendar.GetYear(dateString).ToString("0000"));
                stringBuilder.Append("/");
                stringBuilder.Append(persianCalendar.GetMonth(dateString).ToString("00"));
                stringBuilder.Append("/");
                stringBuilder.Append(persianCalendar.GetDayOfMonth(dateString).ToString("00"));
                var result = stringBuilder.ToString();
                return result;
            }
            catch
            {
                return null;
            }
        }


    }
}