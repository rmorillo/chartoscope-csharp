using System;
using Chartoscope.Common;
using Chartoscope.Analyser;

namespace Chartoscope.Builtin.Probes
{
	public class SMAOfPrices: PriceIndicator<float, float>
	{
		private MovingSumOfPrices _movingSum;			
		
		public SMAOfPrices(int period): base(period)
		{			
			_movingSum= new MovingSumOfPrices(period);
			Dependency.Register(_movingSum);				
		}	
		
		public override void Evaluate()
		{			
			_indicator.Write(_movingSum.Current/(float)_period);
		}					
	}
}

