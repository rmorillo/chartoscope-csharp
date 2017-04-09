using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceFeedService : IPriceFeedService
    {
        private IPriceFeeder _priceFeeder;
        public OHLCFeed OHLC { get; private set; }
        public CandlestickFeed Candlesticks { get; private set;}
        public HeikenAshiFeed HeikenAshi { get; private set; }
        public RenkoFeed Renko { get; set; }
        public PriceBarFeed PriceBar { get; set; }

        public PriceFeedService(IPriceFeeder priceFeeder)
        {
            _priceFeeder = priceFeeder;
            _priceFeeder.MinutePriceBarFeed += MinutePriceBarFeed;

            OHLC = new OHLCFeed();
            Candlesticks = new CandlestickFeed();
            HeikenAshi = new HeikenAshiFeed();
            Renko = new RenkoFeed();
            PriceBar = new PriceBarFeed();
        }

        private void MinutePriceBarFeed(TickerReference tickerReference, IPriceBar priceBar)
        { 
            OHLC.Aggregator.MinutePriceAction(tickerReference, priceBar);
            PriceBar.Aggregator.MinutePriceAction(tickerReference, priceBar);
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, OHLCPriceOption ohlcPriceOption)
        {
            return OHLC.Setup(tickerSymbol, interval, ohlcPriceOption);
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, CandlestickPriceOption candlestickPriceOption)
        {
            return Candlesticks.Setup(tickerSymbol, interval, candlestickPriceOption);
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, HeikenAshiPriceOption heikenAshiPriceOption)
        {
            return HeikenAshi.Setup(tickerSymbol, interval, heikenAshiPriceOption);
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, PriceBarOption priceBarOption)
        {
            return PriceBar.Setup(tickerSymbol, interval, priceBarOption);
        }

        public TickerReference Setup(TickerSymbol tickerSymbol, FeedInterval interval, RenkoPriceOption priceBarOption)
        {
            return Renko.Setup(tickerSymbol, interval, priceBarOption);
        }

        public void Start()
        {
            OHLC.Wireup();
            PriceBar.Wireup();
            Candlesticks.Wireup();
            HeikenAshi.Wireup();
            Renko.Wireup();
        }

        #region "OHLC"
        public IPriceItemFeed<IOHLCBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IOHLCBar> priceAction)
        {
            return OHLC.Subscribe(tickerReference, priceAction);
        }

        public IPriceOptionFeed<OHLCPriceOption, double> Subscribe(TickerReference tickerReference, OHLCPriceOption priceOption, Action<TickerReference, long, OHLCPriceOption, double> priceAction)
        {
            return OHLC.Subscribe(tickerReference, priceOption, priceAction);
        }
        #endregion

        #region "Candlestick"
        public IPriceItemFeed<ICandlestickBar> Subscribe(TickerReference tickerReference, Action<TickerReference, ICandlestickBar> priceAction)
        {
            return Candlesticks.Subscribe(tickerReference, priceAction);
        }

        public IPriceOptionFeed<CandlestickPriceOption, double> Subscribe(TickerReference tickerReference, CandlestickPriceOption priceOption, Action<TickerReference, long, CandlestickPriceOption, double> priceAction)
        {
            return Candlesticks.Subscribe(tickerReference, priceOption, priceAction);
        }
        #endregion

        #region "Pricebar"
        public IPriceItemFeed<IPriceBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IPriceBar> priceAction)
        {
            return PriceBar.Subscribe(tickerReference, priceAction);
        }

        public IPriceOptionFeed<PriceBarOption, double> Subscribe(TickerReference tickerReference, PriceBarOption priceOption, Action<TickerReference, long, PriceBarOption, double> priceAction)
        {
            return PriceBar.Subscribe(tickerReference, priceOption, priceAction);
        }
        #endregion

        #region "Heiken-Ashi"
        public IPriceItemFeed<IHeikenAshiBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IHeikenAshiBar> priceAction)
        {
            return HeikenAshi.Subscribe(tickerReference, priceAction);
        }

        public IPriceOptionFeed<HeikenAshiPriceOption, double> Subscribe(TickerReference tickerReference, HeikenAshiPriceOption priceOption, Action<TickerReference, long, HeikenAshiPriceOption, double> priceAction)
        {
            return HeikenAshi.Subscribe(tickerReference, priceOption, priceAction);
        }
        #endregion

        #region "Renko"
        public IPriceItemFeed<IRenkoBar> Subscribe(TickerReference tickerReference, Action<TickerReference, IRenkoBar> priceAction)
        {
            return Renko.Subscribe(tickerReference, priceAction);
        }

        public IPriceOptionFeed<RenkoPriceOption, double> Subscribe(TickerReference tickerReference, RenkoPriceOption priceOption, Action<TickerReference, long, RenkoPriceOption, double> priceAction)
        {
            return Renko.Subscribe(tickerReference, priceOption, priceAction);
        }
        #endregion
    }
}
