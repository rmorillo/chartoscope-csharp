using System;
using NUnit.Framework;

namespace Chartoscope.TcpMessaging.Test
{
	[TestFixture()]
	public class TcpSubscriberTests
	{
		[Test()]
		public void BasicTest ()
		{
			TcpSubscriber tcpSub= new TcpSubscriber("localhost",11001);
		}
	}
}

