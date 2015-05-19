using System;
using Chartoscope.Common;
using Chartoscope.Analyser;

namespace Chartoscope.Builtin.Probes
{
	public class MovingSumOfNumbers: NumericIndicator
	{	
		public MovingSumOfNumbers (int period): base(period)
		{
		}				
		
		public override void Evaluate()
		{
			float sum= 0;
			for(int i=0; i<_period; i++)
			{
				sum+= _numericList[i];
			}
			
			_indicator.Write(sum);
		}
	}
}

