using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class TimeframeHelper
    {
        public static DateTime NextRolling(DateTime baseTime, TimeBarItemType timeBarItemType)
        {
            DateTime returnDate = DateTime.MinValue;
            int timeBarValue = (int)timeBarItemType.Value;
            switch (timeBarItemType.Tag)
            {
                case "M":
                    returnDate = LastRolling(baseTime, timeBarItemType).AddMinutes(timeBarValue);
                    break;                
                case "H":
                    returnDate = LastRolling(baseTime, timeBarItemType).AddHours(timeBarValue);
                    break;
                case "D":
                    returnDate = LastRolling(baseTime, timeBarItemType).AddDays(1);
                    break;
                case "W":
                    returnDate = LastRolling(baseTime, timeBarItemType).AddDays(7);
                    break;
                case "MN":
                    returnDate = LastRolling(baseTime, timeBarItemType).AddMonths(1);
                    break;
            }

            return returnDate;
        }

        public static DateTime LastRolling(DateTime baseTime, TimeBarItemType timeBarItemType)
        {
            DateTime returnDate = DateTime.MinValue;
            int timeBarValue = (int) timeBarItemType.Value;
            switch (timeBarItemType.Tag)
            {
                case "M":
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, baseTime.Hour, (baseTime.Minute / timeBarValue) * timeBarValue, 0);
                    break;                
                case "H":
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, (baseTime.Hour / timeBarValue) * timeBarValue, 0, 0);
                    break;
                case "D":
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, 0, 0, 0);
                    break;
                case "W":
                    int dayOfWeek = ((int)baseTime.DayOfWeek) == 0 ? 7 : ((int)baseTime.DayOfWeek);
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, 0, 0, 0).AddDays(1 - dayOfWeek);
                    break;
                case "MN":
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, 1, 0, 0, 0);
                    break;
            }

            return returnDate;
        }
    }
}
