using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Common.Messaging
{
	public class MessageEncoding
	{
		private const string MSG_HEADER= "<$Chartosope$>";
		private const byte GET_TOPIC_MSG= 1;
		
		public static byte[] EncodeTopics(Guid requestId, Dictionary<Guid, string> topics)
		{		
			int msgHeaderLength= MSG_HEADER.Length;
			int totalItems= topics.Count;
			int totalLength= 0;
			
			foreach(string topic in topics.Values)
			{
				totalLength+= topic.Length;
			}
			
			int totalMsgSize= msgHeaderLength + 1 + 16 + 2 + (totalItems * (16 + 2)) + totalLength;
				
			byte[] encoded= new byte[totalMsgSize];
			
			ASCIIEncoding.ASCII.GetBytes(MSG_HEADER).CopyTo(encoded,0);
			encoded[msgHeaderLength]= GET_TOPIC_MSG;
			requestId.ToByteArray().CopyTo(encoded, msgHeaderLength + 1);
			BitConverter.GetBytes((short) totalItems).CopyTo(encoded, msgHeaderLength + 1 + 16);
			
			int offset= msgHeaderLength + 1 + 16 + 2;		
			foreach(KeyValuePair<Guid,string> topic in topics)
			{
				topic.Key.ToByteArray().CopyTo(encoded, offset);
				offset+= 16;
				byte[] encodedStr= EncodeString(topic.Value);				
				Array.Copy(encodedStr,0 , encoded, offset,  encodedStr.Length);
				offset+= encodedStr.Length;
			}
				
			return encoded;
		}
		
		public static void DecodeTopics(byte[] encoded, ref Guid requestId, ref Dictionary<Guid, string> topics)
		{
			string msgHeader= ASCIIEncoding.ASCII.GetString(encoded,0,MSG_HEADER.Length);
			if (msgHeader==MSG_HEADER)
			{	
				byte topicMsgId= encoded[MSG_HEADER.Length];
				if (topicMsgId==GET_TOPIC_MSG)				{
					byte[] decodedRequestId= new byte[16];
					Array.Copy(encoded, MSG_HEADER.Length + 1, decodedRequestId, 0, 16);
					requestId= new Guid(decodedRequestId);
					short topicCount= BitConverter.ToInt16(encoded, MSG_HEADER.Length + 1 + 16);
					topics= new Dictionary<Guid, string>();
					
					int offset= MSG_HEADER.Length + 1 + 16 + 2;
					
					for(int i= 0; i<topicCount; i++)
					{
						byte[] decodedTopicId= new byte[16];
						Array.Copy(encoded, offset, decodedTopicId, 0, 16);
						offset+= 16;
						short topicLength= 0;
						topics.Add(new Guid(decodedTopicId),DecodeString(encoded, offset, out topicLength));
						offset+= 2 + topicLength;
					}
					
				}
				else
				{
					throw new Exception("Unexpeced message type id '" + topicMsgId.ToString() + "'");
				}
			}	
			else
			{
				throw new Exception("Unrecognized message header '" + msgHeader + "'");
			}
	}
		
		public static byte[] EncodeString(string input)
		{
			short inputLen= (short) input.Length;
			byte[] result= new byte[inputLen + 2];
			
			BitConverter.GetBytes(inputLen).CopyTo(result,0);
			ASCIIEncoding.ASCII.GetBytes(input).CopyTo(result,2);
			
			return result;
		}
		
		public static string DecodeString(byte[] input, int startIndex, out short stringLength)
		{
			stringLength= BitConverter.ToInt16(input, startIndex);
			startIndex+= 2;
			return ASCIIEncoding.ASCII.GetString(input, startIndex, stringLength);
		}
	}
}

