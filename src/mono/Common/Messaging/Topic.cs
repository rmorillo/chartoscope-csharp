using System;
using System.Collections.Generic;

namespace Chartoscope.Common.Messaging
{
	public class Topic
	{
		private Dictionary<Guid, Session> _sessions;
		private IPubServer _tcpPubServer;
		
		private string _name;
		public string Name { get { return _name; } }
		
		private Guid _id;
		public Guid Id { get { return _id; } }
		
		public Topic (Guid id, string topicName, IPubServer tcpPubServer)
		{
			_id= id;
			_name= topicName;
			_sessions= new Dictionary<Guid, Session>();
			_tcpPubServer= tcpPubServer;
		}
		
		public Guid CreateSession()
		{
			IPubService pubSocket= _tcpPubServer.Accept();
			Guid sessionId= Guid.NewGuid();
			_sessions.Add(sessionId, new Session(sessionId, pubSocket));
			return sessionId;
		}
		
		public void PushNotification(byte[] notificationMsg)
		{
			_tcpPubServer.Send(notificationMsg);
		}
	}
}

