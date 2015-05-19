using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Brokers
{
    public class BacktestSession
    {
        private TickerType  tickerType;
        private string barDataFile;

        private SessionIndicators indicators = null;

        public SessionIndicators Indicators { get { return indicators; } }

        private SessionSignals signals = null;

        public SessionSignals Signals { get { return signals; } }

        //private SessionStrategies strategies = null;

        //public SessionStrategies Strategies { get { return strategies; } }

        private SessionPriceActions priceActions= null;

        public SessionPriceActions PriceActions { get { return priceActions; } }

        public event DataFeederDelegates.NewBarHandler NewBar;
        public event DataFeederDelegates.NewQuoteHandler NewQuote;

        public event BacktestSignalDelegates.ClosePositionHandler SignalClosePosition;

        public event BacktestSignalDelegates.OpenPositionHandler SignalOpenPosition;

        public event BacktestStrategyDelegates.ClosePositionHandler StrategyClosePosition;

        public event BacktestStrategyDelegates.OpenPositionHandler StrategyOpenPosition;

        private Guid sessionId;

        public Guid SessionId
        {
            get { return sessionId; }
        }

        private Guid accountId = Guid.Empty;

        private bool cachingEnabled = true;
        private Guid cacheId = Guid.Empty;

        public BacktestSession(Guid accountId, TickerType tickerType, string barDataFile, Guid cacheId, bool cachingEnabled= true)
        {
            this.accountId = accountId;

            this.tickerType = tickerType;
            this.barDataFile = barDataFile;
            this.sessionId = Guid.NewGuid();
            indicators = new SessionIndicators();
            signals = new SessionSignals(indicators, cacheId, cachingEnabled);           
            signals.EntrySignal += OnEntrySignal;
            signals.ExitSignal += OnExitSignal;
            //strategies = new SessionStrategies(signals, indicators, cacheId, cachingEnabled);
            //strategies.EntryPosition += OnEntryStrategy;
            //strategies.ExitPosition += OnExitStrategy;
            this.cacheId = cacheId;
            this.cachingEnabled = cachingEnabled;
        }

        private void OnEntryStrategy(long sequence, PositionMode position)
        {
            if (StrategyOpenPosition != null)
            {
                StrategyOpenPosition(accountId, tickerType, position, dataFeeder.LastPrice.Close, dataFeeder.LastPrice.Close, dataFeeder.LastPrice);
            }
        }

        private void OnExitStrategy(long sequence)
        {
            if (StrategyClosePosition != null)
            {
                StrategyClosePosition(accountId, tickerType, dataFeeder.LastPrice.Close, dataFeeder.LastPrice.Close, dataFeeder.LastPrice);
            }
        }

       
        private void OnEntrySignal(long sequence, PositionMode position)
        {
            if (SignalOpenPosition != null)
            {
                SignalOpenPosition(accountId, tickerType, position, dataFeeder.LastPrice.Close, dataFeeder.LastPrice.Close, dataFeeder.LastPrice);
            }
        }

        private void OnExitSignal(long sequence)
        {
            if (SignalClosePosition != null)
            {
                SignalClosePosition(accountId, tickerType, dataFeeder.LastPrice.Close, dataFeeder.LastPrice.Close, dataFeeder.LastPrice);
            }
        }

        private BacktestDataFeeder dataFeeder = null;

        private void MergeBarItemTypes(List<BarItemType> source, List<BarItemType> destination)
        {
            foreach (BarItemType barType in source)
            {
                if (!BarItemTypeExists(destination, barType.Code))
                {
                    destination.Add(barType);
                }
            }
        }

        private bool BarItemTypeExists(List<BarItemType> barItemTypeList, string barItemTypeCode)
        {
            bool exists = false;
            foreach (BarItemType barType in barItemTypeList)
            {
                if (barType.Code == barItemTypeCode)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        public void Start()
        {
            List<BarItemType> barItemTypes = new List<BarItemType>();
            List<BarItemType> indicatorBarItemTypes = indicators.GetBarItemTypes();
            MergeBarItemTypes(indicatorBarItemTypes, barItemTypes);
            List<BarItemType> signalBarItemsTypes = signals.GetBarItemTypes();
            MergeBarItemTypes(signalBarItemsTypes, barItemTypes);
            
            dataFeeder = new BacktestDataFeeder(barDataFile, barItemTypes, cacheId);

            dataFeeder.NewBar += OnNewBar;
            dataFeeder.BarGap += OnBarGap;
            dataFeeder.NewQuote += OnNewQuote;

            priceActions = new SessionPriceActions(barItemTypes);

            dataFeeder.Start();

            OnExitSignal(long.MaxValue);
        }

        public PriceBars GetPriceBars(BarItemType barType)
        {
            return priceActions.GetPriceAction(barType);
        }

        public void Pause()
        {
            dataFeeder.Pause();
        }

        public void Resume(Guid accountId)
        {
            dataFeeder.Resume();
        }

        public void OnNewBar(BarItemType barItemType, BarItem barItem)
        {
            if (NewBar != null)
            {
                NewBar(barItemType, barItem);
            }

            priceActions.ReceiveBarItem(barItemType, barItem);
            indicators.ReceivePriceAction(barItemType, priceActions.GetPriceAction(barItemType));
            signals.ReceivePriceAction(barItemType, priceActions.GetPriceAction(barItemType));
            //strategies.ReceivePriceAction(barItemType, priceActions.GetPriceAction(barItemType));
            //DebugHelper.WriteBarItem(barItem);
        }

        public void OnNewQuote(double bidPrice, double askPrice)
        {
            if (NewQuote != null)
            {
                NewQuote(bidPrice, askPrice);
            }

            //.RecieveQuote(bidPrice, askPrice);
        }

        public void OnBarGap(BarItemType barItemType, DateTime dateTime)
        {
            //DebugHelper.WriteBarItemGap(dateTime);
        }
    }
}
