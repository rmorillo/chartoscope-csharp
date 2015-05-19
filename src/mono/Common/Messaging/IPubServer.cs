using System;

namespace Chartoscope.Common.Messaging
{
	public interface IPubServer
	{
		void Start(MessagingDelegates.GetTopics getTopics, MessagingDelegates.Subscribe subscribe);
		IPubService Accept();
		void Send(byte[] message);
	}
}

