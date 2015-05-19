using System;
using System.Collections.Generic;
using Chartoscope.Common.Messaging;
using NUnit.Framework;

namespace Chartoscope.Common.Test
{
	[TestFixture()]
	public class MessageEncodingTests
	{
		[Test()]
		public void StringTest ()
		{
			string expectedString= "abc";
			byte[] encoded= MessageEncoding.EncodeString(expectedString);
			short stringLength;
			string actualString= MessageEncoding.DecodeString(encoded, 0, out stringLength);
			Assert.AreEqual(expectedString, actualString);
		}
		
		[Test()]
		public void TopicTest ()
		{
			Dictionary<Guid, string> topics= new Dictionary<Guid, string>();
			Guid expectedId1= Guid.NewGuid();
			string expectedTopic1= "123";
			string expectedTopic2= "xyy";
			Guid expectedId2= Guid.NewGuid();
			topics.Add(expectedId1, expectedTopic1);
			topics.Add(expectedId2, expectedTopic2);
			
			Guid encodedRequestId= Guid.NewGuid();
			
			byte[] encoded= MessageEncoding.EncodeTopics(encodedRequestId, topics);
			
			Dictionary<Guid, string> decodedTopics= null;
			Guid decodedRequestId= Guid.NewGuid();
			
			MessageEncoding.DecodeTopics(encoded, ref decodedRequestId, ref decodedTopics);
			
			Assert.AreEqual(encodedRequestId, decodedRequestId);
			
			Assert.AreEqual(topics.Count, decodedTopics.Count);
			
			Assert.IsTrue(decodedTopics.ContainsKey(expectedId1));
			Assert.AreEqual(topics[expectedId1], decodedTopics[expectedId1]);
			
			Assert.IsTrue(decodedTopics.ContainsKey(expectedId2));
			Assert.AreEqual(topics[expectedId2], decodedTopics[expectedId2]);				
		}
	}
}

