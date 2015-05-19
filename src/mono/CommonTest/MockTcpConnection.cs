using System;
using System.Collections.Generic;

namespace Chartoscope.Common.Test
{
	public class MockTcpConnection: IPubServer, ISubClient, IInProcSubscriber
	{
		private ShortMessageBuffer _messages;
		private MessagingDelegates.GetTopics _getTopics;
		private MessagingDelegates.Subscribe _subscribe;
		private MessagingDelegates.ReceiveMessage _receiveNotification;
		
		public MockTcpConnection ()
		{
			_messages= new ShortMessageBuffer(100);
		}		
		
		#region ITcpPubServer implementation
		public void Start (MessagingDelegates.GetTopics getTopics, MessagingDelegates.Subscribe subscribe)
		{
			_getTopics= getTopics;
			_subscribe= subscribe;
		}
		
		public IPubService Accept ()
		{
			throw new NotImplementedException ();
		}
			
		#endregion

		#region ITcpSubClient implementation
		public void Connect ()
		{
			
		}

		public void Send (byte[] message, MessagingDelegates.ReceiveMessage receiveMessage)
		{
			Guid msgId= Guid.NewGuid();
			_messages.MoveNext();
			Buffer.BlockCopy(message, 0, _messages.Current, 0, message.Length);	
			receiveMessage(MessageEncoding.EncodeTopics(msgId, _getTopics()), 1, 1);
		}
		
		public string HostName {
			get {
				throw new NotImplementedException ();
			}
		}

		public string PortNumber {
			get {
				throw new NotImplementedException ();
			}
		}
		#endregion

		#region IInProcSubscriber implementation
		public Dictionary<Guid, string> GetTopics ()
		{
			return _getTopics();
		}
		#endregion

		#region IInProcSubscriber implementation
		public void Subscribe (Guid topicId, MessagingDelegates.ReceiveMessage receiveNotification)
		{
			_receiveNotification= receiveNotification;
			
			Guid sessionId= Guid.NewGuid();
			_subscribe(sessionId, topicId);
		}
		#endregion

		#region IPubServer implementation
		public void Send (byte[] message)
		{
			_receiveNotification(message, 1, 1);
		}
		#endregion
	}
}

