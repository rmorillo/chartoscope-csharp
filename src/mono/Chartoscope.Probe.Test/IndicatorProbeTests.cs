using System;
using NUnit.Framework;
using Chartoscope.Probes;

namespace Chartoscope.Probe.Test
{
	[TestFixture()]
	public class IndicatorProbeTests
	{
		[Test()]
		public void BasicTest ()
		{
			IndicatorProbeFactory factory= new IndicatorProbeFactory();
			
			IndicatorProbe probe= factory.CreateSMAProbe(10);
		}
	}
}

