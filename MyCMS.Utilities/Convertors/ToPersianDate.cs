using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyCMS.Utilities.Convertors
{
    public static class ToPersianDate
    {
        public static string ConvertToPersianDate(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(dateTime).ToString("00"),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00")
                );
        }
    }
}
