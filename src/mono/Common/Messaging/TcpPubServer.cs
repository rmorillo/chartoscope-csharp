using System;

namespace Chartoscope.Common
{
	public class TcpPubServer: IPubServer
	{
		public TcpPubServer ()
		{
		}

		#region ITcpPubServer implementation
		public void Start (MessagingDelegates.GetTopics getTopics)
		{
			throw new NotImplementedException ();
		}

		public IPubService Accept ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

