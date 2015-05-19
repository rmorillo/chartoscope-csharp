using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Chartoscope.Core
{
	public class Publisher
	{
		private Dictionary<Guid, Topic> _topics;
		private TcpListener _tcpListener;
		
		public Publisher (int portNumber)
		{
			_topics= new Dictionary<Guid, Topic>();
			_tcpListener= new TcpListener(portNumber);
		}
		
		public void AddTopic(Guid id, string topic)
		{
			_topics.Add(Guid, new Topic(id, topic));
		}
		
		public void Start()
		{
			_tcpListener.Start();
		}
		
		
	}
}

