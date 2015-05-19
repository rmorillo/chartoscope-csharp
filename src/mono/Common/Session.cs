using System;
using System.Net.Sockets;

namespace Chartoscope.Core
{
	public class Session
	{
		private Socket _socket;
		private Guid _sessionId;
		
		public Session (Guid sessionId, Socket socket)
		{
			_sessionId= sessionId;
			_socket= socket;
		}
	}
}

