using System;
using System.Collections.Generic;

namespace Chartoscope.Common.Messaging
{
	public class PubSessionCollection
	{
		private Dictionary<Guid, PubSession> _sessions;
		
		public PubSessionCollection ()
		{
			_sessions= new Dictionary<Guid, PubSession>();
		}
		
		public void Add(Guid sessionId, PubSession session)
		{
			_sessions.Add(sessionId, session);
		}
		
		public int Count { get { return _sessions.Count; } }
	}
}

