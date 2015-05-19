using System;
using Chartoscope.Common;

namespace Chartoscope.Feeder.Test
{
	public class MockFeedProvider: FeedProviderBase
	{
		private MockFeeder _mock;
		
		private PriceBars _priceBars;
		
		public MockFeedProvider ()
		{
			_priceBars= new PriceBars(100);
			
			FeederFactory factory= new FeederFactory();
		}

		#region implemented abstract members of Chartoscope.Common.FeedProviderBase
		public override void Start (DateTime lastTimeStamp)
		{
			BackFill();
			
			_mock= factory.CreateMockFeeder(2, 100);			
			_mock.OnReceiveFeed(ReceiveFeed);
			_mock.Start();
		}

		public override void Stop ()
		{
			
		}
		#endregion
	}
}

