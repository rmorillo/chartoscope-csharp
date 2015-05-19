using System;
using System.Collections.Generic;
using System.Net;

namespace Chartoscope.Common.Messaging
{
	public class Publisher
	{
		private IPubServer _tcpPubServer;
		
		public Publisher (IPubServer tcpPubServer, int portNumber)
		{
			_tcpPubServer= tcpPubServer;
			_topics= new TopicCollection(_tcpPubServer);
			_sessions= new PubSessionCollection();
		}
		
		private TopicCollection _topics;
		public TopicCollection Topics { get { return _topics; } }
		
		public void Start()
		{
			_tcpPubServer.Start(GetTopics, Subscribe);
		}
		
		private Dictionary<Guid, string> GetTopics()
		{
			return _topics.Topics;
		}
		
		private void Subscribe(Guid sessionId, Guid topicId)
		{
			_sessions.Add(sessionId, new PubSession());
		}
		
		
		private PubSessionCollection _sessions;
		public PubSessionCollection Sessions { get { return _sessions; } }
	}
}

