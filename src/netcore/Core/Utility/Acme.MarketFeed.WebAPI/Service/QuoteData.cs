using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.MarketFeed.WebAPI
{
    [ProtoContract]
    public class QuoteData
    {
        [ProtoMember(1)]
        public double Bid { get; set; }

        [ProtoMember(2)]
        public double Ask { get; set; }
    }
}
