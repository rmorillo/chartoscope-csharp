using System;

namespace Chartoscope.Common
{
	public class PriceBars: LookBehindPool<PriceBar>
	{
		public PriceBars (int length):base(length)
		{
		}
		
		protected override PriceBar CreatePoolItem ()
		{
			return new PriceBar();
		}		
	}
}

