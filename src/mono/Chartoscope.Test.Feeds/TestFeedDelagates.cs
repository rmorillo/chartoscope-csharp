using System;
using Chartoscope.Common;

namespace Chartoscope.Test.Feeds
{
	public static class TestFeedDelagates
	{
		public delegate void PushFeed(DateTime timeStamp, float open, float high, float low, float close, float volume=0);
	}
}

