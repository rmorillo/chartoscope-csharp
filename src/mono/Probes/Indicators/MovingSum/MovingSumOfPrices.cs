using System;
using Chartoscope.Common;
using Chartoscope.Analyser;

namespace Chartoscope.Builtin.Probes
{
	public class MovingSumOfPrices: PriceIndicator<float, float>
	{	
		public MovingSumOfPrices (int period): base(period)
		{
		}
		
		public override void Evaluate()
		{
			float sum= 0;
			for(int i=0; i<_period; i++)
			{
				switch (priceBarValue)
				{
					case PriceBarValueOption.Open:
						sum+= priceBars[i].Open;
						break;
					case PriceBarValueOption.High:
						sum+= priceBars[i].High;
							break;
					case PriceBarValueOption.Low:
						sum+= priceBars[i].Low;
							break;
					case PriceBarValueOption.Close:
						sum+= priceBars[i].Close;
							break;
				}					
			}
			
			_indicator.Write(sum);			
		}	
		
	}
}

