using System;
using Chartoscope.Analyser;
using Chartoscope.Common;

namespace Chartoscope.Builtin.Probes
{
	public class EMAOfPrices: PriceIndicator<float, float>
    {        
        private float smoothingConstant = 0;
		
		private SMAOfPrices _sma;
		
        public EMAOfPrices(int period): base(period)
        {
            smoothingConstant = (float)2 / (period + 1);
            _sma= new SMAOfPrices(period);
			Dependency.Register(_sma);
        }

        public override void Evaluate ()
		{
			if (_sma.Count>1)
			{
				float prevSMA = _sma.Previous;
				float currentPrice= 0;		
				switch (priceBarValue)
				{
					case PriceBarValueOption.Open:
						currentPrice= priceBars.Current.Open;
						break;
					case PriceBarValueOption.High:
						currentPrice= priceBars.Current.High;
						break;
					case PriceBarValueOption.Low:
						currentPrice= priceBars.Current.Low;
						break;
					case PriceBarValueOption.Close:
						currentPrice= priceBars.Current.Close;
						break;
				}	
				
				float ema = (currentPrice * smoothingConstant) + (prevSMA * (1 - smoothingConstant)); 
	            _indicator.Write(ema);
			}
		}      
    }
}

