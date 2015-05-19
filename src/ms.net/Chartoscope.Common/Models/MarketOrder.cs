using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class MarketOrder
    {
        private DateTime openingBarTime;

        public DateTime OpeningBarTime
        {
            get { return openingBarTime; }
        }

        public MarketOrder(TickerType tickerType)
        {                     
            this.ticker = tickerType;           
        }

        public MarketOrder(TickerType tickerType, double openingBidPrice, double openingAskPrice, BarItem openingPriceBar)
        {
            this.ticker = tickerType;
        }

        public void OpenOrder(DateTime time, MarketOrderState orderState, double openingBidPrice, double openingAskPrice, DateTime openingBarTime)
        {
            this.orderTime = time;
            this.orderState = orderState;
            this.openingBidPrice = openingBidPrice;
            this.openingAskPrice = openingAskPrice;
            this.openingBarTime = openingBarTime;
            this.position = (PositionMode)Enum.Parse(typeof(PositionMode), Enum.GetName(typeof(MarketOrderState), orderState));
        }      

        private PositionMode position;

        public PositionMode Position
        {
            get { return position; }
            set { position = value; }
        }
        

        private DateTime closingBarTime;

        public DateTime ClosingBarTime
        {
            get { return closingBarTime; }
        }


        public void CloseOrder(DateTime time, double closingBidPrice, double closingAskPrice, DateTime closingBarTime)
        {
            this.closedTime= time;
            this.closingBidPrice= closingBidPrice;
            this.closingAskPrice= closingAskPrice;
            this.closingBarTime = closingBarTime;
        }

        private DateTime orderTime;

        public DateTime OrderTime
        {
            get { return orderTime; }
        }

        private TickerType ticker;

        public TickerType Ticker
        {
            get { return ticker; }
            set { ticker = value; }
        }
        
        private DateTime closedTime= DateTime.MinValue;

	    public DateTime ClosedTime
	    {
		    get { return closedTime;}
		    set { closedTime = value;}
	    }
	

        private MarketOrderState orderState;

        public MarketOrderState OrderState
        {
            get { return orderState; }
        }

        private double openingBidPrice;

        public double OpeningBidPrice
        {
            get { return openingBidPrice; }
            set { openingBidPrice = value; }
        }

        private double openingAskPrice;

        public double OpeningAskPrice
        {
            get { return openingAskPrice; }
        }

        private double closingBidPrice;

        public double ClosingBidPrice
        {
            get { return closingBidPrice; }
        }


        private double closingAskPrice;

        public double ClosingAskPrice
        {
            get { return closingAskPrice; }
        }

        public double ProfitOrLossInPips
        {
            get
            {
                return position == PositionMode.Long ? closingAskPrice - openingAskPrice : openingBidPrice - closingBidPrice;
            }
        }

        public double ProfitInPips
        {
            get
            {
                return ProfitOrLossInPips > 0 ? Math.Abs(ProfitOrLossInPips) : 0;
            }
        }

        public double LossInPips
        {
            get
            {
                return ProfitOrLossInPips < 0 ? Math.Abs(ProfitOrLossInPips) : 0;
            }
        }

    }
}
