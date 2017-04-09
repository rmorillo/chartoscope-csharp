using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class TickerReference
    {
        private Guid _id;
        private Type _priceFeedType;
        private int _priceFeedOption;
        public TickerReference(Guid id, TickerSymbol symbol, FeedInterval interval, Type priceFeedType, int priceFeedOption)
        {
            _id = id;
            _priceFeedType = priceFeedType;
            _priceFeedOption = priceFeedOption;
            Symbol = symbol;
            Interval = interval;
        }
        public Guid Id { get { return _id; } }
        public Type PriceFeedType { get { return _priceFeedType; } }
        public int PriceFeedOption { get { return _priceFeedOption; } }

        public TickerSymbol Symbol { get; private set; }

        public FeedInterval Interval { get; private set; }
       
    }
}
