using System;
using System.Collections.Generic;

namespace Chartoscope.Common.Messaging
{
	/// <summary>
	/// Interface definition for the in-process subscriber
	/// </summary>
	public interface IInProcSubscriber
	{
		Dictionary<Guid, string> GetTopics();	
		void Subscribe(Guid topicId, MessagingDelegates.ReceiveMessage receiveNotification);
	}
}

