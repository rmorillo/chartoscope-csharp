using System;

namespace Chartoscope.Common.Messaging
{
	public class ShortMessageBuffer: LookBehindPool<byte[]>
	{
		public const int MESSAGE_SIZE= 1024;
		
		public ShortMessageBuffer (int length):base(length)
		{
		}
		
		protected override byte[] CreatePoolItem ()
		{
			return new byte[MESSAGE_SIZE];
		}
	}
}

