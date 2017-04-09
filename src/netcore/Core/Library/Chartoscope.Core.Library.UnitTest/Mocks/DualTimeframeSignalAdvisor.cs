using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest.Mocks
{
    public class DualTimeframeSignalAdvisor : SignalAdvisor, IPriceFeedSubscriber
    {
        private TickerReference _trendReferenceTimeframe;        

        private MockProbe _probe1;
        private MockProbe _probe2;
        private MockProbe _probe3;
        private MockProbe _probe4;
        public DualTimeframeSignalAdvisor(TickerReference marketSignalTimeframe, TickerReference trendReferenceTimeframe) : base(marketSignalTimeframe)
        {            
            _trendReferenceTimeframe = trendReferenceTimeframe;

            _probe1 = new MockProbe(1);
            _probe2 = new MockProbe(2);
            _probe3 = new MockProbe(3);
            _probe4 = new MockProbe(4);
        }

        private void TrendReferencePriceAction(TickerReference tickerReference, IOHLCBar priceBar)
        {

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

        public void Subscribe(IPriceFeedService priceFeedService)
        {
            priceFeedService.Subscribe(_SignalTimeframe, MarketOrderPriceAction)
                .Register(_probe1)
                .Register(_probe2);

            

            priceFeedService.Subscribe(_trendReferenceTimeframe, TrendReferencePriceAction)
                .Register(_probe3)
                .Register(_probe4);

        }
    }
}
