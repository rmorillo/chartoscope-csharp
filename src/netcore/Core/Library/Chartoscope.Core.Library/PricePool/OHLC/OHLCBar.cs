using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class OHLCBar: ByteMap, IOHLCBar
    {
        public OHLCBar(long timeStamp, double open, double high, double low, double close)
            : base(OHLCBarEntity.Header.TotalByteSize)
        {
            Write(timeStamp, open, high, low, close);

            Encoding
                .Map<long>(CandlestickEntity.Timestamp, () => Timestamp, (s) => Timestamp = s)
                .Map<Double>(CandlestickEntity.Open, () => Open, (s) => Open = s)
                .Map<Double>(CandlestickEntity.High, () => High, (s) => High = s)
                .Map<Double>(CandlestickEntity.Low, () => Low, (s) => Low = s)
                .Map<Double>(CandlestickEntity.Close, () => Close, (s) => Close = s);                
        }

        public void Write(long timeStamp, double open, double high, double low, double close)
        {
            Timestamp = timeStamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        public long Timestamp { get; private set; }

        public double Open { get; private set; }

        public double High { get; private set; }

        public double Low { get; private set; }

        public double Close { get; private set; }
    }
}
