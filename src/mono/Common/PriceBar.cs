using System;

namespace Chartoscope.Common
{
	public class PriceBar: IPriceBar
	{
		public PriceBar ()
		{
		}
				
		public void Write(DateTime timeStamp, float open, float high, float low, float close, float volume= 0)
		{
			_timeStamp= timeStamp;
			_open= open;
			_high= high;
			_low= low;
			_close= close;
			_volume= volume;
		}
		
		#region IPriceBar implementation
		private DateTime _timeStamp;
		public DateTime TimeStamp {
			get {
				return _timeStamp;
			}
		}
		
		private float _open;
		public float Open {
			get {
				return _open;
			}
		}

		private float _high;
		public float High {
			get {
				return _high;
			}
		}
		
		private float _low;
		public float Low {
			get {
				return _low;
			}
		}
		
		private float _close;		
		public float Close {
			get {
				return _close;
			}
		}
		
		private float _volume;
		public float Volume {
			get {
				return _volume;
			}
		}
		#endregion
	}
}

