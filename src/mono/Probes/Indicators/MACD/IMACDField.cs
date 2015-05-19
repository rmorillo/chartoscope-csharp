using System;

namespace Chartoscope.Builtin.Probes
{
	public interface IMACDField
	{
		float FastEMA { get; }
		float SlowEMA { get; }
		float MACD { get; }
		float EMAofMACD { get; }
		float Histogram { get; }			
	}
}

