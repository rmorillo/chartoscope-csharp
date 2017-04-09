using Chartoscope.Core.Messaging;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Shell
{
    [ProtoContract]
    public class AdminServicePayload
    {
        [ProtoMember(1)]
        public byte[] Data { get; set; } 
    }
}
