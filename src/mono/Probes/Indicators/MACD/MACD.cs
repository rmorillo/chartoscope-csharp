using System;
using Chartoscope.Analyser;
using Chartoscope.Common;

namespace Chartoscope.Builtin.Probes
{
	public class MACD: PriceIndicator<MACDField, IMACDField>
    {
   		private EMAOfPrices _fastEMA;
        private EMAOfPrices _slowEMA;
		private LookbackArray<float> _macd;
		private EMAOfNumbers _macdEMA;

        public MACD(int fastEMAPeriod, int slowEMAPeriod, int signalLinePeriod): base(fastEMAPeriod)
        {			
			_fastEMA= new EMAOfPrices(fastEMAPeriod);
			Dependency.Register(_fastEMA);
			
           	_slowEMA= new EMAOfPrices(slowEMAPeriod);
			Dependency.Register(_slowEMA);		
			
			_macd= new LookbackArray<float>(AnalyserConfig.MaxLookbackCapacity);
			
			_macdEMA= new EMAOfNumbers(signalLinePeriod);
        }      
		
		public override bool IsPrimed (int count)
		{
			return _slowEMA.IsPrimed(count);
		}	
		
		public override void Evaluate ()
		{
			float macd= _fastEMA.Current-_slowEMA.Current;				
			
			_macd.Write(macd);
			
			_macdEMA.Indicator.Evaluate(_macd);
			
			if (_macdEMA.HasValue)
			{					
				_indicator.NextPoolItem.Write(_fastEMA.Current, _slowEMA.Current, _macd.Current, _macdEMA.Current, _macd.Current-_macdEMA.Current);
				_indicator.MoveNext();
			}
		}	
    }
}

