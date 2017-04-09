using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest.Mocks
{
    public class SingleTimeframeSignalAdvisor : SignalAdvisor, IPriceFeedSubscriber
    {
        private MockProbe _probe1;
        private MockProbe _probe2;

        public SingleTimeframeSignalAdvisor(TickerReference marketSignalTimeframe) : base(marketSignalTimeframe)
        {
            _probe1 = new MockProbe(1);
            _probe2 = new MockProbe(2);                       
        }

        public void Subscribe(IPriceFeedService priceFeedService)
        {
            priceFeedService.Subscribe(_SignalTimeframe, MarketOrderPriceAction)
                .Register(_probe1)
                .Register(_probe2);
        }

        private void MarketOrderPriceAction(TickerReference tickerReference, IOHLCBar priceBar)
        {
            if (priceBar.Open > priceBar.Close && priceBar.Open-priceBar.Close >= 10)
            {                
                OpenPosition(PositionOption.Sell);
            }
            else if (priceBar.Open < priceBar.Close && priceBar.Close - priceBar.Open >= 10)
            {                
                OpenPosition(PositionOption.Buy);
            }
            else if (priceBar.Open==priceBar.Close)
            {
                ClosePosition();
            }
        }
    }
}
