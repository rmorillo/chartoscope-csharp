using System;
using System.Collections.Generic;

namespace Chartoscope.Common.Messaging
{
	public class SubSession
	{	
		private ISubClient _tcpSubClient;
		private IInProcSubscriber _inProcSubscriber;
		
		private MessagingDelegates.ReceiveMessage _receiveNotification;
		
		internal SubSession (object subClient)
		{
		   _tcpSubClient= subClient as ISubClient;
			if (subClient is IInProcSubscriber)
			{
				_inProcSubscriber= subClient as IInProcSubscriber;
			}
		}
		
		public Dictionary<Guid, string> GetTopics()
		{
			if (_inProcSubscriber==null)
			{
				byte[] buffer= new byte[] {1,2,3};
				_tcpSubClient.Send(buffer, ReceiveTopics);
				return new Dictionary<Guid, string>();
			}
			else
			{
				return _inProcSubscriber.GetTopics();
			}
		}
		
		public void ReceiveTopics(byte[] topics, int msgIndex, int maxMsg)
		{
			
		}
		
		public void Subscribe(Guid topicId, MessagingDelegates.ReceiveMessage receiveNotification)
		{
			_receiveNotification= receiveNotification;
			
			if (_inProcSubscriber==null)
			{
				byte[] buffer= new byte[] {1,2,3};
				_tcpSubClient.Send(buffer, ReceiveNotification);			
			}
			else
			{
				_inProcSubscriber.Subscribe(topicId, ReceiveNotification);
			}
		}
		
		public void ReceiveNotification(byte[] notificationMsg, int msgIndex, int maxMsg)
		{
			_receiveNotification(notificationMsg, msgIndex, maxMsg);
		}
	}
}

