using System;

namespace Chartoscope.Feeder
{
	public interface IFeedProvider
	{
		IBlackBox BlackBox { get ; set;}
		event FeederDelegates.MarketOpenedHandler MarketOpened;
		event FeederDelegates.MarketClosedHandler MarketClosed;
		event FeederDelegates.FeedWarningHandler FeedWarning;
		event FeederDelegates.FeedErrorHandler FeedError;
		void Start();
		void Stop();
	}
}

