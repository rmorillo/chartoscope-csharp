using System;
using Chartoscope.Analyser;
using Chartoscope.Builtin.Probes;

namespace Chartoscope.Builtin.Probes
{
	public class IndicatorProbeFactory
	{
		public IndicatorProbeFactory ()
		{
		}
		
		public IndicatorProbe<SMAOfPrices, float, float> CreateSMAProbe(int period)
		{		
			IndicatorProbe<SMAOfPrices, float, float> indicatorProbe= new IndicatorProbe<SMAOfPrices, float, float>(new SMAOfPrices(period));
			return indicatorProbe;
		}				
	}
}

