using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class HeikenAshiEntity
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
        static HeikenAshiEntity()
        {
            Header = new EntityHeader("heikenashi", "Heiken Ashi", "Heiken Ashi");

            Timestamp = new EntityField("timestamp", "Timestamp", "Timestamp", typeof(long), sizeof(long)).Register(Header);
            Open = new EntityField("open", "Open", "Open", typeof(double), sizeof(double)).Register(Header);
            High = new EntityField("high", "High", "High", typeof(double), sizeof(double)).Register(Header);
            Low = new EntityField("low", "Low", "Timestamp", typeof(double), sizeof(double)).Register(Header);
            Close = new EntityField("close", "Close", "Timestamp", typeof(double), sizeof(double)).Register(Header);
            UpperWick = new EntityField("upper_wick", "Upper Wick", "Timestamp", typeof(double), sizeof(double)).Register(Header);
            Body = new EntityField("body", "Body", "Body", typeof(double), sizeof(double)).Register(Header);
            LowerWick = new EntityField("lower_wick", "Lower Wick", "Lower Wick", typeof(double), sizeof(double)).Register(Header);
            IsFilled = new EntityField("is_filled", "Timestamp", "Timestamp", typeof(bool), sizeof(bool)).Register(Header);
        }
    }
}
