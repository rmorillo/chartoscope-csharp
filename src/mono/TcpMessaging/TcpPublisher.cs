using System;
using Chartoscope.Common;
using Chartoscope.Common.Messaging;

namespace Chartoscope.TcpMessaging
{
	public class TcpPublisher: IPubServer
	{
		public TcpPublisher ()
		{
		}

		#region IPubServer implementation
		public void Start (MessagingDelegates.GetTopics getTopics, MessagingDelegates.Subscribe subscribe)
		{
			throw new NotImplementedException ();
		}

		public IPubService Accept ()
		{
			throw new NotImplementedException ();
		}

		public void Send (byte[] message)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

