using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chartoscope.Core.Messaging
{
    public class MessageSerializer
    {
        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                return ms.ToArray();
            };
        }

        public static T Deserialize<T>(byte[] input)
        {
            using (MemoryStream ms = new MemoryStream(input))
            {
                ms.Seek(0, SeekOrigin.Begin);
                return Serializer.Deserialize<T>(ms);
            }

        }
    }
}
