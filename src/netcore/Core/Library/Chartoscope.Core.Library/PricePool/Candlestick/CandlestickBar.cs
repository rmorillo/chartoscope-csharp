using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class CandlestickBar : ByteMap, ICandlestickBar
    {        
        public CandlestickBar(long timeStamp, double open, double high, double low, double close, double upperWick, double body, double lowerWick, bool isFilled)
            :base(CandlestickEntity.Header.TotalByteSize)
        {
            Write(timeStamp, open, high, low, close, upperWick, body, lowerWick, isFilled);

            Encoding 
                .Map<long>(CandlestickEntity.Timestamp, () => Timestamp, (s) => Timestamp = s)
                .Map<Double>(CandlestickEntity.Open, () => Open, (s) => Open = s)
                .Map<Double>(CandlestickEntity.High, () => High, (s) => High = s)
                .Map<Double>(CandlestickEntity.Low, () => Low, (s) => Low = s)
                .Map<Double>(CandlestickEntity.Close, () => Close, (s) => Close = s)
                .Map<Double>(CandlestickEntity.UpperWick, () => UpperWick, (s) => UpperWick = s)
                .Map<Double>(CandlestickEntity.Body, () => Body, (s) => Body = s)
                .Map<Double>(CandlestickEntity.LowerWick, () => LowerWick, (s) => LowerWick = s)
                .Map<bool>(CandlestickEntity.IsFilled, () => IsFilled, (s) => IsFilled = s);
        }

        public void Write(long timeStamp, double open, double high, double low, double close, double upperWick, double body, double lowerWick, bool isFilled)
        {
            Timestamp = timeStamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Body = body;
            UpperWick = upperWick;
            LowerWick = lowerWick;
            IsFilled = isFilled;
        }

        public long Timestamp { get; private set; }

        public double Open { get; private set; }

        public double High { get; private set; }

        public double Low { get; private set; }

        public double Close { get; private set; }

        public double UpperWick { get; private set; }

        public double Body { get; private set; }

        public double LowerWick { get; private set; }

        public bool IsFilled { get; private set; }
    }
}
