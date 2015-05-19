using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Chartoscope.Core
{
	public class Topic
	{
		private Dictionary<Guid, Session> _sessions;
		private TcpListener _tcpListener;
		
		public Topic (Guid id, string topicName, TcpListener tcpListener)
		{
			_sessions= new Dictionary<Guid, Session>();
			_tcpListener= tcpListener;
		}
		
		public Guid CreateSession()
		{
			Socket socket= tcpListener.AcceptSocket();
			Guid sessionId= Guid.NewGuid();
			_sessions.Add(sessionId, new Session(sessionId, socket));
			return sessionId;
		}
	}
}

