using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chartoscope.Core.Messaging
{
    public class MessagePayloadBase
    {
        protected byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                return ms.ToArray();
            };
        }
    }
}
