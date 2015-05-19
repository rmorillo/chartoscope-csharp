using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Strategies
{
    public abstract class ExtensibleStrategyBase: IStrategy
    {
        private List<AnalyticsItem> registeredAnalytics = null;

        private bool isInPosition = false;

        public bool IsInPosition { get { return isInPosition; } }

        private PositionMode position;

        public PositionMode Position { get { return position; } }

        protected BarItemType barType = null;

        public BarItemType BarType { get { return barType; } }

        public List<AnalyticsItem> RegisteredAnalytics { get { return this.registeredAnalytics; } }

        private long signalCounter = 0;

        public ExtensibleStrategyBase()
        {
            this.registeredAnalytics = new List<AnalyticsItem>();
        }

        public event StrategyDelegates.EntryStrategyHandler EnterPosition;

        public event StrategyDelegates.ExitStrategyHandler ExitPosition;

        void IStrategy.InPositionState(PriceBars priceBar, BarItemType barType)
        {
            InPosition(position, priceBar, barType);
        }

        void IStrategy.OutPositionState(PriceBars priceBar, BarItemType barType)
        {
            OutPosition(priceBar, barType);
        }

        protected virtual void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
        }

        protected virtual void OutPosition(PriceBars priceBar, BarItemType barType)
        {
        }

        private double openingPrice;

        public double OpeningPrice
        {
            get { return openingPrice; }
        }

        public double CurrentPrice
        {
            get 
            { 
                return position==PositionMode.Long? this.askPrice: position==PositionMode.Short? this.bidPrice: (this.bidPrice + this.askPrice) / 2;
            }
        }
        
        protected void Enter(PositionMode position)
        {
            if (EnterPosition != null)
            {
                signalCounter++;
                EnterPosition(this, signalCounter, position);
                this.position = position;
                this.isInPosition = true;
                
                if (position == PositionMode.Short)
                {
                    this.openingPrice = bidPrice;
                }
                else if (position == PositionMode.Long)
                {
                    this.openingPrice = askPrice;
                }
            }
        }

        protected void Exit()
        {
            if (ExitPosition != null)
            {
                foreach (AnalyticsItem analytics in this.registeredAnalytics)
                {
                    if (analytics.AnaylticsType == AnalyticsTypeOption.Signal)
                    {
                        ((ISignal)analytics.Instance).ForceExit();
                    }
                }

                signalCounter++;
                ExitPosition(this, signalCounter);
                this.isInPosition = false;                
            }
        }


        protected void Register(params object[] instances)
        {
            foreach (object instance in instances)
            {
                if (instance is ISignal)
                {
                    this.registeredAnalytics.Add(new AnalyticsItem(AnalyticsTypeOption.Signal, instance));
                }
            }
        }

        protected string identityCode = null;

        public string IdentityCode
        {
            get { return identityCode; }
        }

        private double bidPrice;

        public double BidPrice
        {
            get { return bidPrice; }
        }

        private double askPrice;

        public double AskPrice
        {
            get { return askPrice; }
            set { askPrice = value; }
        }

        public void NewQuote(double bidPrice, double askPrice)
        {
            this.bidPrice = bidPrice;
            this.askPrice = askPrice;

            this.ReceiveQuote(bidPrice, askPrice);
        }

        protected virtual void ReceiveQuote(double bidPrice, double askPrice)
        {
        }

        public double ProfitOrLossInPips
        {
            get
            {
                if (position == PositionMode.Long)
                {
                    return (CurrentPrice - OpeningPrice) * SpotForex.EURUSD.PipsFactor;
                }
                else if (position == PositionMode.Short)
                {
                    return (OpeningPrice - CurrentPrice) * SpotForex.EURUSD.PipsFactor;
                }
                else
                    return 0;
            }
        }    
    }
}
