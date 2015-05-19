using System;
using NUnit.Framework;
using Chartoscope.Common;
using Chartoscope.Builtin.Probes;
using Chartoscope.Analyser;
using Chartoscope.Test.Feeds;

namespace Chartoscope.Builtin.Probes.Test
{
	[TestFixture()]
	public class MovingSumTests
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
			
			MovingSumOfPrices msop= new MovingSumOfPrices(3);
			
			ProbeReader<float, float> reader= msop.GetReader();
			
			IPriceEvaluate evaluator= msop;
			
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(0, reader.Count);
			
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(0, reader.Count);
			
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(1, reader.Count);
			Assert.AreEqual(12, reader.Current);
			
			_maxwellFeed.PushNext(1);
			
			evaluator.Evaluate(_priceBarReader);
			
			Assert.AreEqual(2, reader.Count);
			Assert.AreEqual(16, reader.Current);
		}			
				
		private void OnPushFeed(DateTime timeStamp, float open, float high, float low, float close, float volume=0)
		{
			_priceBars.NextPoolItem.Write(timeStamp, open, high, low, close, volume);
			_priceBars.MoveNext();
		}
		
		[Test()]
		public void NumberBasicTest ()
		{
			LookbackArray<float> numbers= new LookbackArray<float>(100);
				
			MovingSumOfNumbers mson= new MovingSumOfNumbers(3);
			
			ProbeReader<float, float> reader= mson.GetReader();
			
			INumberEvaluate evaluator= mson;
			
			numbers.Write(4);
			
			evaluator.Evaluate(numbers);
			
			Assert.AreEqual(0, reader.Count);
			
			numbers.Write(4);
			evaluator.Evaluate(numbers);
			
			Assert.AreEqual(0, reader.Count);
			
			numbers.Write(4);
			evaluator.Evaluate(numbers);
			
			Assert.AreEqual(1, reader.Count);
			
			Assert.AreEqual(12, reader.Current);
			
		}
		
		
	}
}

