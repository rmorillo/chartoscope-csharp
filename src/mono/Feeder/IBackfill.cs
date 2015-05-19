using System;

namespace Chartoscope.Feeder
{
	public interface IBackFill
	{
		void OnReceiveBackFill(FeederDelegates.ReceiveFeed backFill, int minBars, int maxBars);
	}
}

