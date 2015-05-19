using System;

namespace Chartoscope.Builtin.Probes
{
	public class MACDField: IMACDField
	{
		public MACDField ()
		{
		}
		
		public void Write(float fastEMA, float slowEMA, float macd, float emaOfMACD, float histogram)
		{
			_fastEMA= fastEMA;
			_slowEMA= slowEMA;
			_macd= macd;
			_emaOfMACD= emaOfMACD;
			_histogram= histogram;
		}
		
		#region IMACDField implementation
		private float _fastEMA;
		public float FastEMA {
			get {
				return _fastEMA;
			}
		}
		
		private float _slowEMA;
		public float SlowEMA {
			get {
				return _slowEMA;
			}
		}
		
		private float _macd;
		public float MACD {
			get {
				return _macd;
			}
		}
		
		private float _emaOfMACD;
		public float EMAofMACD {
			get {
				return _emaOfMACD;
			}
		}
		
		private float _histogram;
		public float Histogram {
			get {
				return _histogram;
			}
		}
		#endregion
	}
}

