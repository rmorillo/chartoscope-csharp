using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class HeikenAshiBar: ByteMap, IHeikenAshiBar
    {
        public HeikenAshiBar(long timeStamp, double open, double high, double low, double close, double upperWick, double body, double lowerWick, bool isFilled)
            :base(HeikenAshiEntity.Header.TotalByteSize)
        {
            Write(timeStamp, open, high, low, close, upperWick, body, lowerWick, isFilled);

            Encoding
                .Map<long>(HeikenAshiEntity.Timestamp, () => Timestamp, (s) => Timestamp = s)
                .Map<Double>(HeikenAshiEntity.Open, () => Open, (s) => Open = s)
                .Map<Double>(HeikenAshiEntity.High, () => High, (s) => High = s)
                .Map<Double>(HeikenAshiEntity.Low, () => Low, (s) => Low = s)
                .Map<Double>(HeikenAshiEntity.Close, () => Close, (s) => Close = s)
                .Map<Double>(HeikenAshiEntity.UpperWick, () => UpperWick, (s) => UpperWick = s)
                .Map<Double>(HeikenAshiEntity.Body, () => Body, (s) => Body = s)
                .Map<Double>(HeikenAshiEntity.LowerWick, () => LowerWick, (s) => LowerWick = s)
                .Map<bool>(HeikenAshiEntity.IsFilled, () => IsFilled, (s) => IsFilled = s);
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
