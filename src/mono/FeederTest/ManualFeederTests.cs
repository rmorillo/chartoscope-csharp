using System;
using NUnit.Framework;

namespace Chartoscope.Feeder.Test
{
	[TestFixture()]
	public class ManualFeederTests
	{
		[Test()]
		public void BasicTest ()
		{
			FeederFactory factory= new FeederFactory();
			ManualFeeder manual= factory.CreateManualFeeder();
			
			manual.Feed(DateTime.Now,  1, 2, 3, 4);
		}
	}
}

