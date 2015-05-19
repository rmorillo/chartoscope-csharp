using System;
using NUnit.Framework;
using Chartoscope.Common;

namespace Chartoscope.Feeder.Test
{
	[TestFixture()]
	public class MockFeederTests
	{
		private PriceBars _priceBars;
		[Test()]
		public void BasicTest ()
		{
			_priceBars= new PriceBars(100);
			
			FeederFactory factory= new FeederFactory();
			MockFeeder mock= factory.CreateMockFeeder(2, 100);
			mock.OnReceiveFeed(ReceiveFeed);
			mock.Start();
			
			Assert.AreNotEqual(0,_priceBars.Count);
		}
		
		public void ReceiveFeed(DateTime timeStamp, float open, float high, float low, float close, float volume= 0)
		{
			_priceBars.NextPoolItem.Write(timeStamp, open, high, low, close, volume);
			_priceBars.MoveNext();
		}
	}
}

