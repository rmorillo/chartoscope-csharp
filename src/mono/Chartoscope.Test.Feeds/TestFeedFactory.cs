using System;
using Chartoscope.Common;

namespace Chartoscope.Test.Feeds
{
	public class TestFeedFactory
	{
		public TestFeedFactory ()
		{
		}
		
		public ITestFeed CreateMaxwellFeed(IntervalTypeOption interval, DateTime startDateTime)
		{
			return new MaxwellFeed(interval, startDateTime);
		}
	}
}

