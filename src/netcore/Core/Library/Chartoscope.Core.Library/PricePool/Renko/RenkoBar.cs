using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class RenkoBar : ByteMap, IRenkoBar
    {
        public RenkoBar(long timeStamp, double open, double close)
            : base(RenkoBarEntity.Header.TotalByteSize)
        {
            Write(timeStamp, open, close);
        }
        internal void Write(long timeStamp, double open, double close)
        {
            Timestamp = timeStamp;
            Open = open;
            Close = close;
        }

        public long Timestamp { get; private set; }

        public double Open { get; private set; }   

        public double Close { get; private set; }
    }
}
