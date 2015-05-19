using System;
using Chartoscope.Common;

namespace Chartoscope.Test.Feeds
{
	public class MaxwellFeed: TestFeedBase
	{
		internal MaxwellFeed (IntervalTypeOption interval, DateTime startDateTime):base(interval, startDateTime)
		{
			AddPrice(1,2,3,4);
			AddPrice(2,2,3,4);
			AddPrice(3,2,3,4);
			AddPrice(4,2,3,4);
			AddPrice(5,2,3,4);
		}		
	}
}

