using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class CandlestickBars : LookBehindPool<ICandlestickBar>, INotifyUpdate<ICandlestickBar, CandlestickPriceOption>, IByteArrayEncoder
    {
        public event Delegates.BarUpdateNotificationEventHandler<ICandlestickBar> BarUpdated;
        public event Delegates.PriceUpdateNotificationEventHandler<CandlestickPriceOption, double> PriceUpdated;
        private IPersistenceWriter _persistenceWriter;

        public CandlestickBars(int capacity) : base(capacity)
        {            
        }

        public CandlestickBars(int capacity, IPersistenceWriter persistenceService) : base(capacity)
        {
            _persistenceWriter = persistenceService;
            _persistenceWriter.Initialize(CandlestickEntity.Header.ToByteArray());
        }

        protected override ICandlestickBar CreatePoolItem()
        {
            return new CandlestickBar(DateTime.Now.Ticks,0,0,0,0,0,0,0,false);
        }

        public void Write(IPriceBar priceBar)
        {
            Write(priceBar.Timestamp, priceBar.Open, priceBar.High, priceBar.Low, priceBar.Close);

        }

        public void Write(long timeStamp, double open, double high, double low, double close)
        {
            var CandlestickBar = (CandlestickBar)NextPoolItem;

            if (open > close)
            {
                CandlestickBar.Write(timeStamp, open, high, low, close, open - close, high - open, close - low, false);
            }
            else
            {
                CandlestickBar.Write(timeStamp, open, high, low, close, close - open, high - close, open - low, true);
            }

            MoveNext();            

            if (_persistenceWriter != null)
            {
                _persistenceWriter.Append(CandlestickBar.ToByteArray());
            }
       
            BarUpdated?.Invoke(Current);
            
            PriceUpdated?.Invoke(Current.Timestamp, CandlestickPriceOption.Close, Current.Close);            
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] encoded)
        {
            throw new NotImplementedException();
        }
    }
}
