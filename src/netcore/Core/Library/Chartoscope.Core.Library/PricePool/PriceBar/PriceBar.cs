using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class PriceBar: IPriceBar
    {
        public PriceBar()
        {
        }

        public PriceBar(long timeStamp, double open, double high, double low, double close, double volume = 0)
        {
            Write(timeStamp, open, high, low, close, volume);
        }

        public void Write(long timeStamp, double open, double high, double low, double close, double volume = 0)
        {
            _timeStamp = timeStamp;
            _open = open;
            _high = high;
            _low = low;
            _close = close;
            _volume = volume;
        }

        private long _timeStamp;
        public long Timestamp
        {
            get
            {
                return _timeStamp;
            }
        }

        private double _open;
        public double Open
        {
            get
            {
                return _open;
            }
        }

        private double _high;
        public double High
        {
            get
            {
                return _high;
            }
        }

        private double _low;
        public double Low
        {
            get
            {
                return _low;
            }
        }

        private double _close;
        public double Close
        {
            get
            {
                return _close;
            }
        }

        private double _volume;
        public double Volume
        {
            get
            {
                return _volume;
            }
        }
    }
}
