using System;

namespace Chartoscope.Feeder
{
	public interface IPriceBarFeed
	{
		void Feed(DateTime timeStamp, float open, float high, float low, float close, float volume= 0);
		void OnReceiveFeed(FeederDelegates.ReceiveFeed receiveFeed);
	}
}

