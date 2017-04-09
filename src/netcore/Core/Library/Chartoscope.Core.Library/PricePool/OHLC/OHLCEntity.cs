using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class OHLCBarEntity
    {
        public static readonly EntityField Timestamp;
        public static readonly EntityField Open;
        public static readonly EntityField High;
        public static readonly EntityField Low;
        public static readonly EntityField Close;
        public static readonly EntityField UpperWick;
        public static readonly EntityField Body;
        public static readonly EntityField LowerWick;
        public static readonly EntityField IsFilled;

        public static readonly EntityHeader Header;
        static OHLCBarEntity()
        {
            Header = new EntityHeader("ohlc", "OHLC", "OHLC");

            Timestamp = new EntityField("timestamp", "Timestamp", "Timestamp", typeof(long), sizeof(long)).Register(Header);
            Open = new EntityField("open", "Open", "Open", typeof(double), sizeof(double)).Register(Header);
            High = new EntityField("high", "High", "High", typeof(double), sizeof(double)).Register(Header);
            Low = new EntityField("low", "Low", "Timestamp", typeof(double), sizeof(double)).Register(Header);
            Close = new EntityField("close", "Close", "Timestamp", typeof(double), sizeof(double)).Register(Header);            
        }
    }
}
