using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class EMAProbe: IndicatorProbe<IEMAItem, EMAIndicator>, IPriceFeedSubscriber
    {
        private TickerReference _ticker;
        public EMAProbe(TickerReference ticker, int period)
        {
            _ticker = ticker;
            _Probe = new EMAIndicator(PoolSizeConfig.GetPoolSize(typeof(EMAIndicator)), period);
        }

        public void Subscribe(IPriceFeedService priceFeedService)
        {
            priceFeedService.Subscribe(_ticker, PriceAction);
        }

        public void PriceAction(TickerReference tickerReference, IOHLCBar priceBar)
        {
            _Probe.Calculate(priceBar.Timestamp, priceBar.Close);
        }
    }
}
