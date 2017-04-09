using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators.UnitTest
{
    public class MockDataFactory
    {
        public const string PriceSwingBeginMarker= "PriceSwingBegin";
        public const string PriceSwingEndMarker = "PriceSwingEnd";

        private static List<IPriceBar> _mockPriceHistory;

        private static Dictionary<string, int> _priceHistoryMarker;
        static MockDataFactory()
        {
            _mockPriceHistory = new List<IPriceBar>();
            _priceHistoryMarker = new Dictionary<string, int>();

            Mark(PriceSwingBeginMarker);
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 4));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 5));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 6));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 7));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 8));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 7));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 6));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 5));
            _mockPriceHistory.Add(new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 4));
            Mark(PriceSwingEndMarker);
        }

        private static void Mark(string marker)
        {
            _priceHistoryMarker.Add(marker, _mockPriceHistory.Count);
        }        
        
        public static List<IPriceBar> ExtractPrices(string startMarker, string endMarker, int tailOffset)
        {
            var extracted = new List<IPriceBar>();

            for (int i = _priceHistoryMarker[startMarker]; i < _mockPriceHistory.Count; i++)
            {
                extracted.Add(_mockPriceHistory[i]);
                if (i == _priceHistoryMarker[endMarker] + tailOffset - 1)
                {
                    break;
                }
            }

            return extracted;
        }

        public static List<IPriceBar> ExtractPrices(string startMarker, string endMarker)
        {
            var extracted = new List<IPriceBar>();

            for (int i = _priceHistoryMarker[startMarker]; i < _mockPriceHistory.Count; i++)
            {
                extracted.Add(_mockPriceHistory[i]);
                if (i == _priceHistoryMarker[endMarker])
                {
                    break;
                }
            }

            return extracted;
        }

        public static List<IOHLCBar> ExtractOHLCPrices(string startMarker, string endMarker, int tailOffset)
        {
            var priceBars = ExtractPrices(startMarker, endMarker, tailOffset);
            var ohlcBars = new List<IOHLCBar>();
            priceBars.ForEach((s) => ohlcBars.Add(new OHLCBar(s.Timestamp, s.Open, s.High, s.Low, s.Close)));
            return ohlcBars;
        }

        public static List<IOHLCBar> ExtractOHLCPrices(string startMarker, string endMarker)
        {
            var priceBars = ExtractPrices(startMarker, endMarker);
            var ohlcBars = new List<IOHLCBar>();
            priceBars.ForEach((s) => ohlcBars.Add(new OHLCBar(s.Timestamp, s.Open, s.High, s.Low, s.Close)));
            return ohlcBars;
        }

        public static List<ICandlestickBar> ExtractCandestickPrices(string startMarker, string endMarker, int tailOffset)
        {
            var priceBars = ExtractPrices(startMarker, endMarker, tailOffset);
            var candlestickBars = new List<ICandlestickBar>();
            priceBars.ForEach((s) => candlestickBars.Add(new CandlestickBar(s.Timestamp, s.Open, s.High, s.Low, s.Close, 0, 0, 0, true)));
            return candlestickBars;
        }

        public static List<ICandlestickBar> ExtractCandlestickPrices(string startMarker, string endMarker)
        {
            var priceBars = ExtractPrices(startMarker, endMarker);
            var candlestickBars = new List<ICandlestickBar>();
            priceBars.ForEach((s) => candlestickBars.Add(new CandlestickBar(s.Timestamp, s.Open, s.High, s.Low, s.Close, 0,0,0, true)));
            return candlestickBars;
        }

        public static List<IHeikenAshiBar> ExtractHeikenAsshiPrices(string startMarker, string endMarker, int tailOffset)
        {
            var priceBars = ExtractPrices(startMarker, endMarker, tailOffset);
            var heikenAshiBars = new List<IHeikenAshiBar>();
            priceBars.ForEach((s) => heikenAshiBars.Add(new HeikenAshiBar(s.Timestamp, s.Open, s.High, s.Low, s.Close, 0, 0, 0, true)));
            return heikenAshiBars;
        }

        public static List<IHeikenAshiBar> ExtractHeikenAshiPrices(string startMarker, string endMarker)
        {
            var priceBars = ExtractPrices(startMarker, endMarker);
            var heikenAshiBars = new List<IHeikenAshiBar>();
            priceBars.ForEach((s) => heikenAshiBars.Add(new HeikenAshiBar(s.Timestamp, s.Open, s.High, s.Low, s.Close, 0, 0, 0, true)));
            return heikenAshiBars;
        }

        public static List<IRenkoBar> ExtractRenkoPrices(string startMarker, string endMarker, int tailOffset)
        {
            var priceBars = ExtractPrices(startMarker, endMarker, tailOffset);
            var RenkoBars = new List<IRenkoBar>();
            priceBars.ForEach((s) => RenkoBars.Add(new RenkoBar(s.Timestamp, s.Open, s.Close)));
            return RenkoBars;
        }

        public static List<IRenkoBar> ExtractRenkoPrices(string startMarker, string endMarker)
        {
            var priceBars = ExtractPrices(startMarker, endMarker);
            var RenkoBars = new List<IRenkoBar>();
            priceBars.ForEach((s) => RenkoBars.Add(new RenkoBar(s.Timestamp, s.Open, s.Close)));
            return RenkoBars;
        }
    }
}
