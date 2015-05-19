using System;
using Chartoscope.Common;
using Chartoscope.Analyser;

namespace Chartoscope.Builtin.Probes
{
	public class SMAOfNumbers: NumericIndicator
	{
		private MovingSumOfNumbers _movingSum;			
		
		public SMAOfNumbers(int period): base(period)
		{			
			_movingSum= new MovingSumOfNumbers(period);
			Dependency.Register(_movingSum);				
		}	
		
		public override void Evaluate()
		{			
			_indicator.Write(_movingSum.Current/(float)_period);
		}					
	}
}

