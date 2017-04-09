using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class HeikenAshiBars : LookBehindPool<IHeikenAshiBar>, INotifyUpdate<IHeikenAshiBar, HeikenAshiPriceOption>
    {
        private IPersistenceWriter _persistenceWriter;
        public HeikenAshiBars(int capacity) : base(capacity)
        {
        }

        public HeikenAshiBars(int capacity, IPersistenceWriter persistenceService) : base(capacity)
        {
            _persistenceWriter = persistenceService;
            _persistenceWriter.Initialize(CandlestickEntity.Header.ToByteArray());
        }

        public event Delegates.BarUpdateNotificationEventHandler<IHeikenAshiBar> BarUpdated;
        public event Delegates.PriceUpdateNotificationEventHandler<HeikenAshiPriceOption, double> PriceUpdated;

        protected override IHeikenAshiBar CreatePoolItem()
        {
            return new HeikenAshiBar(DateTime.MinValue.Ticks,0,0,0,0,0,0,0,false);
        }

        public void Write(IPriceBar priceBar)
        {
            Write(priceBar.Timestamp, priceBar.Open, priceBar.High, priceBar.Low, priceBar.Close);
        }

        public void Write(long timeStamp, double open, double high, double low, double close)
        {
            var heikenAshiBar = (HeikenAshiBar)NextPoolItem;

            var haClose = (open + high + low + close) / 4;
            var haOpen = this._Length == 0 ? (open + close) / 2 : (Current.Open + Current.Close) / 2;
            var haHigh = Math.Max(Math.Max(high, haOpen), haClose);
            var haLow = Math.Min(Math.Min(low, haOpen), haClose);            

            if (open > close)
            {
                heikenAshiBar.Write(timeStamp, haOpen, haHigh, haLow, haClose,haOpen-haClose,haHigh-haOpen,haClose-haLow, false);
            }
            else
            {
                heikenAshiBar.Write(timeStamp, haOpen, haHigh, haLow, haClose, haClose-haOpen, haHigh - haClose, haOpen - haLow, true);
            }
            
            MoveNext();

            if (_persistenceWriter != null)
            {
                _persistenceWriter.Append(heikenAshiBar.ToByteArray());
            }

            NotifyUpdate();
        }

        private void NotifyUpdate()
        {
            BarUpdated?.Invoke(Current);
            PriceUpdated?.Invoke(Current.Timestamp, HeikenAshiPriceOption.Close, Current.Close);      
        }

        public void Write(ICandlestickBar CandlestickBar)
        {
            Write(CandlestickBar.Timestamp, CandlestickBar.Open, CandlestickBar.High, CandlestickBar.Low, CandlestickBar.Close);
        }
    }
}
