using System;
using System.Collections.Generic;
using System.Threading;
using Chartoscope.Common.Messaging;
using NUnit.Framework;

namespace Chartoscope.Common.Test
{
	[TestFixture()]
	public class PublisherTests
	{
		private AutoResetEvent wait= new AutoResetEvent(true);
		private byte[] lastMessage;
		
		[Test()]
		public void BasicTest ()
		{
			InProcSubscriber inProc= new InProcSubscriber();
			
			Publisher pub= new Publisher(inProc, 11000);
			
			Assert.AreEqual(0, pub.Topics.Count);
			
			Guid topicId1= Guid.NewGuid();
			string topicName1= "EURUSD@TrueFx";
			Guid topicId2= Guid.NewGuid();
			string topicName2= "MSFT@YahooFinance";
			Guid topicId3= Guid.NewGuid();
			string topicName3= "USDJPY@OANDA-MT4";
			
			pub.Topics.Add(topicId1, topicName1);
			pub.Topics.Add(topicId2, topicName2);
			pub.Topics.Add(topicId3, topicName3);
			
			Assert.AreEqual(3, pub.Topics.Count);
						
			pub.Start();
			
			Assert.AreEqual(0, pub.Sessions.Count);
			
			Subscriber subs= new Subscriber(inProc);
			
			SubSession subSession= subs.BeginSession();
			
			Dictionary<Guid, string> topics= subSession.GetTopics();
			
			Assert.AreEqual(3, topics.Count);
			
			Assert.IsTrue(topics.ContainsKey(topicId1));
			Assert.AreEqual(topics[topicId1], topicName1);
			
			Assert.IsTrue(topics.ContainsKey(topicId2));
			Assert.AreEqual(topics[topicId2], topicName2);
			
			Assert.IsTrue(topics.ContainsKey(topicId3));
			Assert.AreEqual(topics[topicId3], topicName3);
			
			subSession.Subscribe(topicId1, ReceiveNotification);	
			
			Assert.AreEqual(1, pub.Sessions.Count);
							
			pub.Topics[topicId1].PushNotification(new byte[] {1,2,3});
			
			wait.WaitOne(1000);	
			
			Assert.AreEqual(3,lastMessage.Length);
			
			Assert.AreEqual(1, lastMessage[0]);
			Assert.AreEqual(2, lastMessage[1]);
			Assert.AreEqual(3, lastMessage[2]);
		}
		
		private void ReceiveNotification(byte[] msg, int msgIndex, int msgCount)
		{
			lastMessage= msg;
			wait.Set();
		}
	}
}

