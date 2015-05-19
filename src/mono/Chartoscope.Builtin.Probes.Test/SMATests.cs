using System;
using NUnit.Framework;
using Chartoscope.Common;
using Chartoscope.Test.Feeds;
using Chartoscope.Analyser;

namespace Chartoscope.Builtin.Probes.Test
{
	[TestFixture()]
	public class SMATests
	{
		private PriceBars _priceBars;
		private PriceBarReader _priceBarReader;
		private ITestFeed _maxwellFeed;
		
		[Test()]
		public void PriceBasicTest ()
		{
			_priceBars= new PriceBars(100);
			_priceBarReader= new PriceBarReader(_priceBars);
			
			TestFeedFactory testFeed= new TestFeedFactory();
			_maxwellFeed= testFeed.CreateMaxwellFeed(IntervalTypeOption.M1, DateTime.Now);	
			
			_maxwellFeed.PushFeed+= OnPushFeed;
			
			SMAOfPrices sma= new SMAOfPrices(3);
			
			ProbeReader<float, float> reader= sma.GetReader();
			
			IPriceEvaluate evaluator= sma;
			
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(0, reader.Count);
			
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(0, reader.Count);
				
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(1, reader.Count);
			Assert.AreEqual(4, reader.Current);
			
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(2, reader.Count);
			Assert.AreEqual(4, reader.Current);
		}
		
		private void OnPushFeed(DateTime timeStamp, float open, float high, float low, float close, float volume=0)
		{
			_priceBars.NextPoolItem.Write(timeStamp, open, high, low, close, volume);
			_priceBars.MoveNext();
		}
	}
}

