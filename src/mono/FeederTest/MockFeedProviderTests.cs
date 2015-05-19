using System;
using NUnit.Framework;

namespace Chartoscope.Feeder.Test
{
	[TestFixture()]
	public class MockFeedProviderTests
	{
		[Test()]
		public void BasicTest ()
		{
			MockFeedProvider provider= new MockFeedProvider();
			provider.Start();
		}
		
	}
}

