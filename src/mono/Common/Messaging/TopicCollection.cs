using System;
using System.Collections.Generic;

namespace Chartoscope.Common.Messaging
{
	public class TopicCollection
	{
		private Dictionary<Guid, Topic> _topics;
			
		public Topic this[Guid topicId]
		{
			get { return _topics[topicId]; }
		}
		
		public Dictionary<Guid, string>  Topics 
		{
			get{
				Dictionary<Guid, string> topics= new Dictionary<Guid, string>();
				
				foreach(KeyValuePair<Guid, Topic> topic in _topics)
				{
					topics.Add(topic.Key, topic.Value.Name);
				}
				return topics;
			}
		}
		
		private IPubServer _tcpPubServer;
		
		internal TopicCollection (IPubServer tcpPubServer)			
		{
			_topics= new Dictionary<Guid, Topic>();
			_tcpPubServer= tcpPubServer;
		}
		
		public void Add(Guid topicId, string topicName)
		{
			_topics.Add(topicId, new Topic(topicId, topicName, _tcpPubServer));
		}		
		
		public int Count { get { return _topics.Count; } }
	}
}

