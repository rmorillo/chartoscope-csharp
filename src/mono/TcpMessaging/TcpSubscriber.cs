using System;
using Chartoscope.Common;
using Chartoscope.Common.Messaging;

namespace Chartoscope.TcpMessaging
{
	public class TcpSubscriber: ISubClient
	{
		public TcpSubscriber(string hostName, int portNumber)
		{
			_hostName= hostName;
			_portNumber= portNumber;
		}
		
		private string _hostName;
		public string HostName {
			get {
				return _hostName;
			}
		}
		
		private int _portNumber;
		public int PortNumber {
			get {
				return _portNumber;
			}
		}

		#region ISubClient implementation
		public void Connect ()
		{
			throw new NotImplementedException ();
		}

		public void Send (byte[] message, MessagingDelegates.ReceiveMessage receiveMessage)
		{
			throw new NotImplementedException ();
		}

		
		#endregion
	}
}

