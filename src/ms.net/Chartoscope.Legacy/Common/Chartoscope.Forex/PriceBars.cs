using System;

namespace Chartoscope.Common
{
	public class PriceBars: LookBehindPool<PriceBar>
	{
		public PriceBars (int capacity):base(capacity)
		{
		}
		
		protected override PriceBar nextPoolItem ()
		{
			return new PriceBar();
		}		
	}
}

