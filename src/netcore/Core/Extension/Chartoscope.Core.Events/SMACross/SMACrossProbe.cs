using Chartoscope.Core.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Events
{
    public class SMACrossProbe : EventProbe<ISMACrossValue, SMACrossEvent>,
        IRenkoSubscriber
    {    
        public SMACrossProbe(TickerReference ticker, int firstSMAPeriod, int secondSMAPeriod, Action crossed)
            :base(ticker)
        {
            Probe = new SMACrossEvent(PoolSizeConfig.GetPoolSize(typeof(SMAIndicator)), firstSMAPeriod, secondSMAPeriod, crossed);
        }

        public override void Subscribe(IPriceFeedService priceFeedService)
        {
            TrySubscribe(() => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, OHLCPriceActionItem),
                         () => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, OHLCPriceActionOption),
                         () => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, PriceBarPriceActionItem),
                         () => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, PriceBarPriceActionOption),
                         () => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, CandlestickPriceActionItem),
                         () => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, CandlestickPriceActionOption),
                         () => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, HeikenAshiPriceActionItem),
                         () => PriceActionSubscriber.TrySubscribe(Ticker, priceFeedService, HeikenAshiPriceActionOption),
                         () => RenkoFeed.TrySubscribe(Ticker, priceFeedService, RenkoPriceActionItem),
                         () => RenkoFeed.TrySubscribe(Ticker, priceFeedService, RenkoPriceActionOption));                                    
        }

        #region "OHLC feed support"
        private void OHLCPriceActionItem(TickerReference tickerReference, IOHLCBar priceAction)
        {
            Probe.Evaluate(priceAction.Timestamp, priceAction.Close);
        }

        private void OHLCPriceActionOption(TickerReference tickerReference, long timestamp, OHLCPriceOption priceOption, double value)
        {
            Probe.Evaluate(timestamp, value);
        }
        #endregion

        #region "Renko feed support"
        public void RenkoPriceActionItem(TickerReference tickerReference, IRenkoBar renkoBar)
        {
            Probe.Evaluate(renkoBar.Timestamp, renkoBar.Close);
        }

        public void RenkoPriceActionOption(TickerReference tickerReference, long timestamp, RenkoPriceOption priceOption, double value)
        {
            Probe.Evaluate(timestamp, value);
        }
        #endregion

        #region "Heiken-Ashi feed support"
        private void HeikenAshiPriceActionItem(TickerReference tickerReference,IHeikenAshiBar heikenAshiBar)
        {
            Probe.Evaluate(heikenAshiBar.Timestamp, heikenAshiBar.Close);
        }

        private void HeikenAshiPriceActionOption(TickerReference tickerReference, long timestamp, HeikenAshiPriceOption priceOption, double value)
        {
            Probe.Evaluate(timestamp, value);
        }
        #endregion

        #region "Candlestick feed support"
        private void CandlestickPriceActionItem(TickerReference tickerReference, ICandlestickBar candlestickBar)
        {
            Probe.Evaluate(candlestickBar.Timestamp, candlestickBar.Close);
        }
        
        private void CandlestickPriceActionOption(TickerReference tickerReference, long timestamp, CandlestickPriceOption priceOption, double value)
        {
            Probe.Evaluate(timestamp, value);
        }
        #endregion

        #region "Pricebar feed support"
        private void PriceBarPriceActionItem(TickerReference tickerReference, IPriceBar priceBar)
        {
            Probe.Evaluate(priceBar.Timestamp, priceBar.Close);
        }

        private void PriceBarPriceActionOption(TickerReference tickerReference, long timestamp, PriceBarOption priceOption, double value)
        {
            Probe.Evaluate(timestamp, value);
        }
        #endregion

    }
}
