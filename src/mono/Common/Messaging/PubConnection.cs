using System;
using  System.Collections.Generic;

namespace Chartoscope.Common.Messaging
{
	public class PubConnection
	{
		public PubConnection ()
		{
		}
		
		public Dictionary<Guid, string> GetTopics()
		{
			return new Dictionary<Guid, string>();
		}
	}
}

