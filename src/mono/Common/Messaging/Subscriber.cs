using System;

namespace Chartoscope.Common.Messaging
{
	public class Subscriber
	{
		private ISubClient _tcpSubClient;
		
		public Subscriber (ISubClient tcpSubClient)
		{			
			_tcpSubClient= tcpSubClient;
		}
		
		public SubSession BeginSession()
		{
			_tcpSubClient.Connect();
			
			return new SubSession(_tcpSubClient);
		}
		
	}
}

