using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Events
{
    public class SMACrossValue: ISMACrossValue
    {
        public void Write(long timeStamp, EventStateOption status)
        {
            Timestamp = timeStamp;
            Status = status;
        }
        public long Timestamp { get; protected set; }
        public EventStateOption Status { get; protected set; }
}
}
