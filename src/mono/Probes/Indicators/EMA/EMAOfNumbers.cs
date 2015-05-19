using System;
using Chartoscope.Analyser;

namespace Chartoscope.Builtin.Probes
{
	public class EMAOfNumbers: NumericIndicator
	{
		private float smoothingConstant = 0;
		
		private SMAOfNumbers _sma;
		
        public EMAOfNumbers(int period): base(period)
        {
            smoothingConstant = (float)2 / (period + 1);
            _sma= new SMAOfNumbers(period);
			Dependency.Register(_sma);
        }

        public override void Evaluate ()
		{
			float prevSMA = _sma.Previous;
			float currentVal= _numericList.Current;
			
			float ema = (currentVal * smoothingConstant) + (prevSMA * (1 - smoothingConstant)); 
            _indicator.Write(ema);
		}      
	}
}

