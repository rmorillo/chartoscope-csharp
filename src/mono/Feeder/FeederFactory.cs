using System;

namespace Chartoscope.Feeder
{
	public class FeederFactory
	{
		public FeederFactory ()
		{
		}
		
		public ManualFeeder CreateManualFeeder()
		{
			return new ManualFeeder();
		}
		
		public IRemoteFeeder CreateTcpFeeder(string hostName, int portNumber)
		{
			return new RemoteFeeder(hostName, portNumber);
		}
		
		public MockFeeder CreateMockFeeder(int secondsDuration, int millisecondInterval)
		{
			return new MockFeeder(secondsDuration, millisecondInterval);
		}
	}
}

