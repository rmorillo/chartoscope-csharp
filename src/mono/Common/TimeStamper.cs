using System;

namespace Chartoscope.Common
{
	public class TimeStamper
	{
		private IntervalTypeOption _interval;
		private DateTime _currentDateTime= DateTime.MinValue;
		
		public TimeStamper (IntervalTypeOption interval, DateTime startDateTime)
		{
			_interval= interval;
			_currentDateTime= startDateTime;
		}
		
		public DateTime Next
		{
			get
			{
				_currentDateTime= TimeSeriesHelper.NextRollOver(_currentDateTime, _interval);
				return _currentDateTime;
			}
		}
	}
}

