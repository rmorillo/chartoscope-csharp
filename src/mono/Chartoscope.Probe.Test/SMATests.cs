using System;
using NUnit.Framework;
using Chartoscope.Probes;
using Chartoscope.Common;

namespace Chartoscope.Probe.Test
{
	[TestFixture()]
	public class SMATests
	{
		[Test()]
		public void BasicTest ()
		{
			SMA sma= new SMA(3);		
						
			PriceBars priceBars= new PriceBars(10);
			
			priceBars.NextPoolItem.Set(DateTime.Now, 1, 2, 3, 4);
			priceBars.MoveNext();	
			
			sma.Compute(priceBars);
			
			Assert.AreEqual(0, sma.Count);
			
			priceBars.NextPoolItem.Set(DateTime.Now, 5, 6, 7, 8);			
			priceBars.MoveNext();	
			
			sma.Compute(priceBars);
			Assert.AreEqual(0, sma.Count);
			
			priceBars.NextPoolItem.Set(DateTime.Now, 9, 10, 11, 12);			
			priceBars.MoveNext();
			
			sma.Compute(priceBars);
			Assert.AreEqual(1, sma.Count);
			
			Assert.AreEqual((4 + 8 + 12)/3, sma.Current);
		}
	}
}

