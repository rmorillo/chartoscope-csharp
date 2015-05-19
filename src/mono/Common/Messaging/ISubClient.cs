using System;

namespace Chartoscope.Common.Messaging
{
	public interface ISubClient
	{
		void Connect();
		void Send(byte[] message, MessagingDelegates.ReceiveMessage receiveMessage);
	}
}

