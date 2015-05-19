using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public class DataFeedItem
    {
        protected double openingPrice;

        public double OpeningPrice
        {
            get { return openingPrice; }
            set { openingPrice = value; }
        }

        protected double highestPrice;

        public double HighestPrice
        {
            get { return highestPrice; }
            set { highestPrice = value; }
        }

        protected double lowestPrice;

        public double LowestPrice
        {
            get { return lowestPrice; }
            set { lowestPrice = value; }
        }

        protected double currentPrice;

        public double CurrentPrice
        {
            get { return currentPrice; }
            set { currentPrice = value; }
        }

    }
}
