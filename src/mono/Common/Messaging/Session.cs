using System;

namespace Chartoscope.Common.Messaging
{
	public class Session
	{
		private IPubService _pubSocket;
		private Guid _sessionId;
		
		public Session (Guid sessionId, IPubService pubSocket)
		{
			_sessionId= sessionId;
			_pubSocket= pubSocket;
		}
	}
}

