using Chartoscope.Utility.RestClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oanda.Common.Data
{
    public class RateStreamResponse : IHeartbeat
    {
        public Heartbeat heartbeat;
        public Price tick;
        public bool IsHeartbeat()
        {
            return (heartbeat != null);
        }
    }
}
