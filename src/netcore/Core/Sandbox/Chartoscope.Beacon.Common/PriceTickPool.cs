using Chartoscope.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Common
{
    public class PriceTickPool : LookBehindPool<IPriceTick>
    {
        public PriceTickPool(int capacity) : base(capacity)
        {
        }

        protected override IPriceTick CreatePoolItem()
        {
            return new PriceTick();
        }

        public void Write(long timeStamp, double bid, double ask)
        {
            var priceTick = (PriceTick)NextPoolItem;

            priceTick.Timestamp = timeStamp;
            priceTick.Bid = bid;
            priceTick.Ask = ask;

            MoveNext();            
        }
    }
}
