using System;

namespace Chartoscope.Feeder
{
	public static class FeederDelegates
	{
		public delegate void ReceiveFeed(DateTime timeStamp, float open, float high, float low, float close, float volume= 0);
		public delegate void MarketOpenedHandler();
		public delegate void MarketClosedHandler();
		public delegate void FeedWarningHandler();
		public delegate void FeedErrorHandler();
	}
}

