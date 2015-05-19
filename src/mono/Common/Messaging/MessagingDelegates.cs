using System;
using System.Collections.Generic;

namespace Chartoscope.Common.Messaging
{
	public static class MessagingDelegates
	{
		public delegate void ReceiveMessage (byte[] msg, int msgIndex, int msgCount);
		public delegate Dictionary<Guid, string> GetTopics();
		public delegate void Subscribe(Guid sessionId, Guid topicId);
	}
}

