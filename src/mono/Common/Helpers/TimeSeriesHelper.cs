using System;

namespace Chartoscope.Common
{
	/// <summary>
	/// Time series helper
	/// </summary>
	public static class TimeSeriesHelper
    {
		/// <summary>
		/// Gets the next the roll over time based on the given base time and timeframe
		/// </summary>
		/// <returns>
		/// The next roll over time.
		/// </returns>
		/// <param name='baseTime'>
		/// Base time
		/// </param>
		/// <param name='timeFrame'>
		/// Time frame
		/// </param>
        public static DateTime NextRollOver(DateTime baseTime, IntervalTypeOption timeFrame)
        {
            DateTime returnDate = DateTime.MinValue;
            switch (timeFrame)
            {
                case IntervalTypeOption.M1:
                    returnDate = LastRollOver(baseTime, timeFrame).AddMinutes(1);
                    break;
                case IntervalTypeOption.M5:
                    returnDate = LastRollOver(baseTime, timeFrame).AddMinutes(5);
                    break;
                case IntervalTypeOption.M15:
                    returnDate = LastRollOver(baseTime, timeFrame).AddMinutes(15);
                    break;
                case IntervalTypeOption.M30:
                    returnDate = LastRollOver(baseTime, timeFrame).AddMinutes(30);
                    break;
                case IntervalTypeOption.H1:
                    returnDate = LastRollOver(baseTime, timeFrame).AddHours(1);
                    break;
                case IntervalTypeOption.H4:
                    returnDate = LastRollOver(baseTime, timeFrame).AddHours(4);
                    break;
                case IntervalTypeOption.D1:
                    returnDate = LastRollOver(baseTime, timeFrame).AddDays(1);
                    break;
                case IntervalTypeOption.W1:
                    returnDate = LastRollOver(baseTime, timeFrame).AddDays(7);
                    break;
            }

            return returnDate;
        }
		
		/// <summary>
		/// Get the last the roll over given the base time and timeframe.
		/// </summary>
		/// <returns>
		/// The last roll over time
		/// </returns>
		/// <param name='baseTime'>
		/// Base time
		/// </param>
		/// <param name='timeFrame'>
		/// Time frame
		/// </param>
        public static DateTime LastRollOver(DateTime baseTime, IntervalTypeOption timeFrame)
        {
            DateTime returnDate = DateTime.MinValue;
            switch (timeFrame)
            {
                case IntervalTypeOption.M1:
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, baseTime.Hour, baseTime.Minute, 0);
                    break;
                case IntervalTypeOption.M5:
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, baseTime.Hour, (baseTime.Minute / 5) * 5, 0);
                    break;
                case IntervalTypeOption.M15:
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, baseTime.Hour, (baseTime.Minute / 15) * 15, 0);
                    break;
                case IntervalTypeOption.M30:
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, baseTime.Hour, (baseTime.Minute / 30) * 30, 0);
                    break;
                case IntervalTypeOption.H1:
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, baseTime.Hour, 0, 0);
                    break;
                case IntervalTypeOption.H4:
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, (baseTime.Hour / 4) * 4, 0, 0);
                    break;
                case IntervalTypeOption.D1:
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, 0, 0, 0);
                    break;
                case IntervalTypeOption.W1:
                    int dayOfWeek = ((int)baseTime.DayOfWeek) == 0 ? 7 : ((int)baseTime.DayOfWeek);
                    returnDate = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, 0, 0, 0).AddDays(1 - dayOfWeek);
                    break;
            }

            return returnDate;
        }			
    }
}

