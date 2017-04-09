using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceActionSubscriber
    {
        #region "OHLC"
        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, IOHLCBar> priceBarAction)
        {
            if (ticker.PriceFeedType == typeof(OHLCPriceOption) && ((OHLCPriceOption)ticker.PriceFeedOption) == OHLCPriceOption.All)
            {
                priceFeedService.Subscribe(ticker, priceBarAction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, long, OHLCPriceOption, double> priceOptionAction)
        {
            if (ticker.PriceFeedType == typeof(OHLCPriceOption))
            {
                priceFeedService.Subscribe(ticker, (OHLCPriceOption)ticker.PriceFeedOption, priceOptionAction);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region "Pricebar"
        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, IPriceBar> priceBarAction)
        {
            if (ticker.PriceFeedType == typeof(PriceBarOption) && ((PriceBarOption)ticker.PriceFeedOption) == PriceBarOption.All)
            {
                priceFeedService.Subscribe(ticker, priceBarAction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, long, PriceBarOption, double> priceOptionAction)
        {
            if (ticker.PriceFeedType == typeof(PriceBarOption))
            {
                priceFeedService.Subscribe(ticker, (PriceBarOption)ticker.PriceFeedOption, priceOptionAction);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "Candlestick"
        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, ICandlestickBar> priceBarAction)
        {
            if (ticker.PriceFeedType == typeof(CandlestickPriceOption) && ((CandlestickPriceOption)ticker.PriceFeedOption) == CandlestickPriceOption.All)
            {
                priceFeedService.Subscribe(ticker, priceBarAction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, long, CandlestickPriceOption, double> priceOptionAction)
        {
            if (ticker.PriceFeedType == typeof(CandlestickPriceOption))
            {
                priceFeedService.Subscribe(ticker, (CandlestickPriceOption)ticker.PriceFeedOption, priceOptionAction);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "Heiken-Ashi"
        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, IHeikenAshiBar> priceBarAction)
        {
            if (ticker.PriceFeedType == typeof(HeikenAshiPriceOption) && ((HeikenAshiPriceOption)ticker.PriceFeedOption) == HeikenAshiPriceOption.All)
            {
                priceFeedService.Subscribe(ticker, priceBarAction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TrySubscribe(TickerReference ticker, IPriceFeedService priceFeedService,
            Action<TickerReference, long, HeikenAshiPriceOption, double> priceOptionAction)
        {
            if (ticker.PriceFeedType == typeof(HeikenAshiPriceOption))
            {
                priceFeedService.Subscribe(ticker, (HeikenAshiPriceOption)ticker.PriceFeedOption, priceOptionAction);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "Renko"
        
        #endregion
    }
}
