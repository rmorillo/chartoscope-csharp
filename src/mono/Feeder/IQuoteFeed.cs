using System;

namespace Chartoscope.Feeder
{
	public interface IQuoteFeed
	{
		void Feed (DateTime timeStamp, float bid, float ask);
	}
}

