using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Core.Messaging
{
    [ProtoContract]
    public class MessageFrame<THeader, TPayload>
    {
        [ProtoMember(1)]
        public THeader MessageType { get; set; }
        [ProtoMember(2)]
        public TPayload Payload { get; set; }
    }
}
