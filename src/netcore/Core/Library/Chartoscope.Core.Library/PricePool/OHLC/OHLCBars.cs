using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class OHLCBars : LookBehindPool<IOHLCBar>, INotifyUpdate<IOHLCBar, OHLCPriceOption>, IByteArrayEncoder
    {
        public event Delegates.BarUpdateNotificationEventHandler<IOHLCBar> BarUpdated;
        public event Delegates.PriceUpdateNotificationEventHandler<OHLCPriceOption, double> PriceUpdated;

        private IPersistenceWriter _persistenceWriter;

        public OHLCBars(int capacity) : base(capacity)
        {
        }

        public OHLCBars(int capacity, IPersistenceWriter persistenceService) : base(capacity)
        {
            _persistenceWriter = persistenceService;
            _persistenceWriter.Initialize(CandlestickEntity.Header.ToByteArray());
        }


        protected override IOHLCBar CreatePoolItem()
        {
            return new OHLCBar(DateTime.Now.Ticks, 0, 0, 0, 0);
        }

        public void Write(IPriceBar priceBar)
        {
            Write(priceBar.Timestamp, priceBar.Open, priceBar.High, priceBar.Low, priceBar.Close);

        }

        public void Write(long timeStamp, double open, double high, double low, double close)
        {
            var ohlcBar = (OHLCBar)NextPoolItem;

            ohlcBar.Write(timeStamp, open, high, low, close);

            MoveNext();

            if (_persistenceWriter != null)
            {
                _persistenceWriter.Append(ohlcBar.ToByteArray());
            }

            BarUpdated?.Invoke(Current);
            PriceUpdated?.Invoke(Current.Timestamp, OHLCPriceOption.Close, Current.Close);
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
